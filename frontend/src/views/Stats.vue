<template>
<div>
    <v-tabs
        v-model="tab"
        align-with-title>
            <v-tabs-slider color="purple"></v-tabs-slider>

            <v-tab>
                Statistics
            </v-tab>
            <v-tab>
                Usefull Award
            </v-tab>
            <v-tab>
                Crazy Award
            </v-tab>
    </v-tabs>
    
    <v-tabs-items v-model="tab">
        <v-tab-item>    
             <v-layout>
                <v-btn   
                    absolute
                    dark
                    fab
                    top
                    right
                    color="primary">
                    <v-icon @click="refresh">mdi-refresh</v-icon>
                </v-btn>
                <v-btn  
                    absolute
                    dark
                    fab
                    bottom
                    right
                    color="error">
                    <v-icon @click="cleanDatabase">mdi-delete</v-icon>
                </v-btn>
            </v-layout>
                <v-layout
                    row
                    wrap>
                        <v-flex xs12>
                            <QuestionsChart :data="questionsData" />
                        </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs12>
                        <VickyMendozaChart :data="vickyMendozaData" />
                    </v-flex>
                </v-layout>
                <v-layout
                    row
                    wrap>
                        <v-flex xs12 md6>
                            <ColumnsChart :data="questionsUsefullData" />
                        </v-flex>
                        <v-flex xs6>
                            <ColumnsChart :data="questionsCrazyData" />
                        </v-flex>
                        <v-flex xs12 md6>
                            <ColumnsChart :data="peopleUsefullData" />
                        </v-flex>
                        <v-flex xs12 md6>
                            <ColumnsChart :data="peopleCrazyData" />
                        </v-flex>
                </v-layout>
        </v-tab-item>
        <v-tab-item>
            <v-card>
                <v-content>
                    <v-layout text-xs-center wrap>
                        <v-flex xs12 class="margin-top-40">
                            <img src="/img/award-1.gif" />
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                            <h1 class="display-2 font-weight-bold mb-3 primary--text">The one that contributes the most:</h1>
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                            <div class="winner">{{usefullWinner}}</div>
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                        </v-flex>
                    </v-layout>
                </v-content>
            </v-card>
        </v-tab-item>
          <v-tab-item>
             <v-card>
                <v-content>
                    <v-layout text-xs-center wrap>
                        <v-flex xs12 class="margin-top-40">
                            <img src="/img/award-2.gif" />
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                            <h1 class="display-2 font-weight-bold mb-3 primary--text">The Troll:</h1>
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                            <div class="winner">{{crazyWinner}}</div>
                        </v-flex>
                        <v-flex xs12 class="margin-top-40">
                        </v-flex>
                    </v-layout>
                </v-content>
            </v-card>
        </v-tab-item>
    </v-tabs-items>
</div>
</template>

<script lang="ts">
import { Vue, Component, MapGetter, MapAction } from 'types-vue';
import QuestionsChart from '@/components/QuestionsChart.vue';
import VickyMendozaChart from '@/components/VickyMendozaChart.vue';
import ColumnsChart from '@/components/ColumnsChart.vue';
import VoteService from '@/services/vote.service';
import { IUserVote, IQuestion } from '@/models';

const service = new VoteService();

@Component({
    components: {
        QuestionsChart,
        VickyMendozaChart,
        ColumnsChart
    }
})
export default class Stats extends Vue {
    public tab: number = 0;

    public votes: IUserVote[] = [];

    @MapGetter({ namespace: 'vote' })
    public questions!: IQuestion[];

    @MapAction({ namespace: 'app' })
    public loading!: (v: boolean) => void;

    public get questionsData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            ['Crazy', 'Usefull', 'tooltip']
        ];
        const counter: number[] = new Array(this.questions.length + 1);
        counter.fill(0);
        this.votes.forEach((v: IUserVote) => {
            if (v.questionId === 0) { return; }

            let item = result[v.questionId];
            if (!item) {
                const question = this.questions.find((q) => q.id === v.questionId);
                if (!question) { return; }

                item = [ 0, 0, question.id + '. ' + question.text];
                result[v.questionId] = item;
            }
            counter[v.questionId]++;
            item[0] = (item[0] as number) + v.crazy;
            item[1] = (item[1] as number) + v.usefull;
        });

        for (let i = 1; i < result.length; i++) {
            result[i][0] = (result[i][0] as number) / counter[i];
            result[i][1] = (result[i][1] as number) / counter[i];
        }

        return result;
    }

    public get vickyMendozaData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            ['Feature', 'Value', 'style']
        ];

        this.questionsData.forEach((v, index) => {
            if (index === 0) { return; }
            const value = (v[1] as number) - (v[0] as number);
            result.push([v[2], value, value === 0 ? 'gray' : value > 0 ? 'green' : 'red' ]);
        });

        return result;
    }

    public get questionsUsefullData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            ['Question', 'Usefull']
        ];
        this.votes.forEach((v: IUserVote) => {
            if (v.questionId === 0) { return; }

            let item = result[v.questionId];
            if (!item) {
                const question = this.questions.find((q) => q.id === v.questionId);
                if (!question) { return; }

                item = [ question.text, 0 ];
                result.push(item);
            }

            item[1] = (item[1] as number) + v.usefull;
        });

        return result;
    }

    public get questionsCrazyData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            ['Question', 'Crazy']
        ];
        this.votes.forEach((v: IUserVote) => {
            if (v.questionId === 0) { return; }

            let item = result[v.questionId];
            if (!item) {
                const question = this.questions.find((q) => q.id === v.questionId);
                if (!question) { return; }

                item = [ question.text, 0 ];
                result.push(item);
            }

            item[1] = (item[1] as number) + v.crazy;
        });

        return result;
    }

    public get peopleUsefullData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            [ 'Name', 'Usefull' ]
        ];
        this.votes.forEach((v: IUserVote) => {
            if (v.questionId === 0) { return; }

            let item = result.find((i) => i[0] === v.username);
            if (!item) {
                item = [v.username, 0];
                result.push(item);
            }

            item[1] = (item[1] as number) + v.usefull;
        });

        return result;
    }

    public get peopleCrazyData(): Array<Array<(string | number)>> {
        const result: Array<Array<(string | number)>> = [
            [ 'Name', 'Crazy' ]
        ];
        this.votes.forEach((v: IUserVote) => {
            if (v.questionId === 0) { return; }

            let item = result.find((i) => i[0] === v.username);
            if (!item) {
                item = [v.username, 0];
                result.push(item);
            }

            item[1] = (item[1] as number) + v.crazy;
        });

        return result;
    }

    public get usefullWinner(): string {
        let max: number = 0;
        let names: string[] = [];
        for (let i = 1; i < this.peopleUsefullData.length; i++) {
            if (this.peopleCrazyData[i][0] as string === '@dagope'
                || this.peopleCrazyData[i][0] as string === '@fernandoescolar') { continue; }
            if (max === this.peopleUsefullData[i][1]) {
                 names.push(this.peopleUsefullData[i][0] as string);
            } else if (max < this.peopleUsefullData[i][1]) {
                names = [ this.peopleUsefullData[i][0] as string ];
                max = this.peopleUsefullData[i][1] as number;
            }
        }

        return names[names.length - 1];
    }

    public get crazyWinner(): string {
        let max: number = 0;
        let names: string[] = [];
        for (let i = 1; i < this.peopleCrazyData.length; i++) {
            if (this.peopleCrazyData[i][0] as string === '@dagope'
                || this.peopleCrazyData[i][0] as string === '@fernandoescolar') { continue; }
            if (max === this.peopleCrazyData[i][1]) {
                 names.push(this.peopleCrazyData[i][0] as string);
            } else if (max < this.peopleCrazyData[i][1]) {
                names = [ this.peopleCrazyData[i][0] as string ];
                max = this.peopleCrazyData[i][1] as number;
            }
        }

        return names[names.length - 1];
    }

    public async cleanDatabase(): Promise<void> {
        try {
            this.loading(true);
            await service.clean();
        } catch (ex) {
            this.votes = [];
        }
        this.loading(false);
    }

    public async refresh(): Promise<void> {
        try {
            this.loading(true);
            this.votes = await service.get();
        } catch (ex) {
            this.votes = [];
        }
        this.loading(false);
    }

    public mounted() {
        this.refresh();
    }
}
</script>

<style>
.margin-top-40 {
    margin-top: 40px;
}

.winner {
    color: purple;
    font-size: 64px;
}
</style>
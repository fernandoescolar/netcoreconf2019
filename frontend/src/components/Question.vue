<template>
    <v-container>
        <v-layout
            row
            wrap
            class="text-xs-center">
                <v-flex xs12>
                    <h2 class="font-weight-bold mb-3">{{title}}</h2>
                </v-flex>
                <v-flex xs12 class="margin-top-20">
                    <h3>Usefull</h3>
                    <v-rating
                        v-model="hot"
                        label="Usefull"
                        color="red darken-3"
                        background-color="grey darken-1"
                        empty-icon="mdi-heart-outline"
                        full-icon="mdi-heart"
                        half-icon="mdi-heart-half-full"
                        half-increments
                        large
                        hover>
                    </v-rating>
                </v-flex>
                <v-flex xs12 class="margin-top-20">
                     <h3>Crazy</h3>
                      <v-rating
                        v-model="crazy"
                        label="Usefull"
                        color="yellow darken-3"
                        background-color="grey darken-1"
                        half-increments
                        large
                        hover>
                    </v-rating>
                </v-flex>
                <v-flex xs12 class="margin-top-20">
                    <v-btn color="primary" @click="next">Vote</v-btn>
                </v-flex>
        </v-layout>
    </v-container>
</template>

<script lang="ts">
  import { Vue, Component, Prop, Watch, MapGetter, MapAction} from 'types-vue';
  import { IVote, IQuestion } from '@/models';
  
  @Component
  export default class Question extends Vue {
    @Prop({ required: true })
    public questionId!: number;

    public hot: number = 0;

    public crazy: number = 0;

    public alreadyExists: boolean = false;

    public get currentQuestion(): IQuestion | undefined {
        return this.questions.find((q: IQuestion) => q.id === this.questionId);
    }

    public get title(): string {
        return this.currentQuestion ? this.currentQuestion.text : '';
    }

    @MapGetter({ namespace: 'vote' })
    public questions!: IQuestion[];

    @MapGetter({ namespace: 'vote' })
    public votes!: IVote[];

    @MapAction({ namespace: 'vote' })
    public addVote!: (v: IVote) => Promise<void>;

    @Watch('questionId', { immediate: true })
    public onQuestionChanged(value: number): void {
        if (this.currentQuestion) {
            const vote = this.votes.find((v: IVote) => v.questionId === this.questionId);
            if (vote) {
                this.alreadyExists = true;
                this.hot = vote.usefull / 2;
                this.crazy = vote.crazy / 2;
                return;
            }
        }

        this.alreadyExists = false;
        this.hot = 0;
        this.crazy = 0;
    }

    public async next(): Promise<void> {
        const vote: IVote = {
            questionId: this.questionId,
            usefull: this.hot * 2,
            crazy: this.crazy * 2
        };

        await this.addVote(vote);

        if (this.questionId === this.questions.length - 1) {
            this.$router.push({ name: 'summary' });
        } else {
            this.$router.push({ name: 'question', params: { id: (this.questionId + 1).toString() } });
        }
    }
  }
</script>

<style lang="scss">
.margin-top-20 {
    margin-top: 20px;
}
</style>
import { Module, VuexModule, Mutation, Action, Getter } from 'types-vue';
import { IVote, IQuestion, IUserVote } from '@/models';
import VoteService from '@/services/vote.service';

const service = new VoteService();
const storage: Storage = window.localStorage;
const UsernameKey: string = 'netcore-username';
const VotesKey: string = 'netcore-votes';
const Questions: IQuestion[] = [
    { id: 0, text: 'Practice question'},
    { id: 1, text: 'Nullable reference types'},
    { id: 2, text: 'Async streams'},
    { id: 3, text: 'Range and Index'},
    { id: 4, text: 'Recursive patterns'},
    { id: 5, text: 'Switch expressions'},
    { id: 6, text: 'Implicit constructors'},
    { id: 7, text: 'Using declaration'},
    { id: 8, text: 'Default Interfaces'}
];

@Module({ namespaced: true })
export default class extends VuexModule {
    private mQuestions: IQuestion[] = Questions;
    private mVotes: IUserVote[] = this.readVotesFromStorage();
    private mUsername: string = this.readUsernameFromStorage();

    @Getter()
    public questions(): IQuestion[] {
        return this.mQuestions;
    }

    @Getter()
    public votes(): IVote[] {
        return this.mVotes.map((v: IUserVote, index: number) => {
            return { questionId: v.questionId, usefull: v.usefull, crazy: v.crazy, saved: v.saved } as IVote;
        });
    }

    @Getter()
    public unsafeVotes(): IVote[] {
        const result: IVote[] = [];
        this.mVotes.forEach((v: IUserVote) => {
            if (!v.saved) {
                result.push({ questionId: v.questionId, usefull: v.usefull, crazy: v.crazy, saved: v.saved });
            }
        });

        return result;
    }

    @Getter()
    public username(): string {
        return this.mUsername;
    }

    @Mutation()
    public SET_USERNAME(username: string): void {
        this.mUsername = username;
        storage.setItem(UsernameKey, this.mUsername);
    }

    @Mutation()
    public ADD_VOTE(vote: IUserVote): void {
        let item = this.mVotes.find((v: IUserVote) => v.questionId === vote.questionId);
        if (!item) {
            item = vote;
            this.mVotes.push(item);
        } else {
            item.username = vote.username;
            item.usefull = vote.usefull;
            item.crazy = vote.crazy;
            item.saved = vote.saved;
        }

        const json = JSON.stringify(this.mVotes);
        storage.setItem(VotesKey, json);
    }

    @Action({ commit: 'SET_USERNAME' })
    public changeUsername(username: string): string {
        return username;
    }

    @Action({ useContext: true })
    public async addVote(ctx: any, vote: IVote): Promise<void> {
        const dto = {
            questionId: vote.questionId,
            username: ctx.state.mUsername,
            usefull: vote.usefull,
            crazy: vote.crazy,
            saved: false
        };

        try {
            dto.saved = await service.put(dto);
            for (const v of  ctx.state.mVotes) {
                if (!v.saved) {
                    v.saved = await service.put(v);
                }
            }
        } catch (ex) {
            // do nothing
        }

        ctx.commit('ADD_VOTE', dto);
    }

    private readUsernameFromStorage(): string {
        return storage.getItem(UsernameKey) || '';
    }

    private readVotesFromStorage(): IUserVote[] {
        const json = storage.getItem(VotesKey);
        let votes!: IUserVote[];
        if (json) {
            votes = JSON.parse(json) as IUserVote[];
        }

        return votes || [];
    }
}

import http from 'axios';
import { IUserVote } from '@/models';

const url = 'https://netcoreconf.azurewebsites.net/api/votes';

export default class VoteService {
    public put(vote: IUserVote): Promise<boolean> {
        const dto = {
            username: vote.username,
            questionId: vote.questionId,
            usefull: vote.usefull,
            crazy: vote.crazy
        };
        return new Promise<boolean>( (resolve) => {
            http.put(url, dto)
                .then(() => {
                    resolve(true);
                }).catch(() => {
                    resolve(false);
                });
        });
    }

    public get(): Promise<IUserVote[]> {
        return new Promise<IUserVote[]>( (resolve, reject) => {
            http.get(url)
                .then((res) => {
                    const result: IUserVote[] = [];
                    res.data.forEach((e: any) => {
                        result.push({
                            username: e.username,
                            questionId: e.questionId,
                            usefull: e.usefull,
                            crazy: e.crazy,
                            saved: true
                        });
                    });
                    resolve(result);
                }).catch( (err) => {
                    reject(err);
                });
        });
    }

    public clean(): Promise<void> {
        return new Promise<void>( (resolve, reject) => {
            http.delete(url)
                .then((res) => {
                    resolve();
                }).catch( (err) => {
                    reject(err);
                });
        });
    }
}

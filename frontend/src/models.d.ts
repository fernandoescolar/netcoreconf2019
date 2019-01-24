export interface INotification {
    text: string;
    color: string;
}

export interface IVote {
    questionId: number;
    usefull: number;
    crazy: number;
    saved?: boolean;
}

export interface IUserVote extends IVote {
    username: string;
}

export interface IQuestion {
    id: number;
    text: string;
}

export interface IItemCount {
    id?: number;
    text: string;
    count: number;
}

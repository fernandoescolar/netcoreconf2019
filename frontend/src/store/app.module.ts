import { Module, VuexModule, Mutation, Action, Getter } from 'types-vue';
import { INotification } from '@/models';

@Module({ namespaced: true })
export default class extends VuexModule {
    private mLoadingCounter: number = 0;
    private mNotifications: INotification[] = [];

    @Getter()
    public loadingVisible(): boolean {
        return this.mLoadingCounter > 0;
    }

    @Getter()
    public notifications(): INotification[] {
        return this.mNotifications;
    }

    @Mutation()
    public SET_LOADING(visible: boolean): void {
        this.mLoadingCounter += visible ? 1 : -1;
        if (this.mLoadingCounter < 0) {
            this.mLoadingCounter = 0;
        }
    }

    @Mutation()
    public ADD_NOTIFICATION(notification: INotification): void {
        this.mNotifications.push(notification);
    }

    @Mutation()
    public DELETE_NOTIFICATION(notification: INotification): void {
        const index = this.mNotifications.indexOf(notification);
        if (index < 0)  { return; }
        this.mNotifications.splice(index, 1);
    }

    @Action({ commit: 'SET_LOADING' })
    public loading(visible: boolean): boolean {
        return visible;
    }

    @Action({ commit: 'ADD_NOTIFICATION' })
    public notify(notification: INotification): INotification {
        return notification;
    }

    @Action({ commit: 'DELETE_NOTIFICATION' })
    public deleteNotification(notification: INotification): INotification {
        return notification;
    }
}

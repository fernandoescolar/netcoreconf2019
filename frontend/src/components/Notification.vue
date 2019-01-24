<template>
     <v-snackbar :timeout="notificationTimeout"
                :color="color"
                :top="y === 'top'"
                :bottom="y === 'bottom'"
                :right="x === 'right'"
                :left="x === 'left'"
                :multi-line="mode === 'multi-line'"
                :vertical="mode === 'vertical'"
                v-model="visible">
        <span> {{ text }}</span>        
        <v-btn dark flat @click.native="visible = false">{{ 'Close' }}</v-btn>
    </v-snackbar>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, MapAction } from 'types-vue';
import { INotification } from '@/models';

@Component
export default class Notification extends Vue {
    @Prop()
    public notification!: INotification;

    public visible: boolean = false;

    public color: string = '';

    public mode: string  = 'multi-line';

    public timeout: number = 3000;

    public timeoutError: number = 0;

    public y: string = 'top';

    public x: string = '';

    public text: string = '';

    public get notificationTimeout(): number {
        return this.color.toUpperCase() === 'ERROR' ? this.timeoutError : this.timeout;
    }

    @MapAction( {namespace: 'app'} )
    public deleteNotification!: (n: INotification) => void;

    @Watch('notification', { immediate: true })
    private onNotificationChanged(): void {
        if (!this.notification) { return; }
        this.text = this.notification.text;
        this.color = this.notification.color;
        this.visible = true;
    }

    @Watch('visible')
    private onVisibleChanged(val: boolean): void {
        if (val) { return; }
        setTimeout(() => this.deleteNotification(this.notification), 500);
    }
}
</script>
<template>
     <div class="notification__wrapper">
        <Notification v-for="notification in notifications" :notification="notification" :key="notifications.indexOf(notification)" />
    </div>
</template>

<script lang="ts">
import { Vue, Component, MapGetter } from 'types-vue';
import { INotification } from '@/models';
import Notification from './Notification.vue';

@Component({
    components: {
        Notification
    }
})
export default class Notifications extends Vue {
    @MapGetter({ namespace: 'app' })
    public notifications!: INotification[];
}
</script>

<style>
    .snack {
        z-index: 1000;
    }
    .notification__wrapper {
        z-index: 2000;
    }
</style>
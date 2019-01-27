<template>
  <v-app>
    <Loading />
    <Notifications />
    <v-toolbar app>
      <v-toolbar-title class="headline text-uppercase">
        <span class="font-weight-light">HOT CRAZY</span>
        <span> C#</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>

    <v-content>
      <router-view />
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component, MapGetter } from 'types-vue';
import { IVote, IQuestion } from '@/models';
import Loading from '@/components/Loading.vue';
import Notifications from '@/components/Notifications.vue';

@Component({
  components: {
    Loading,
    Notifications
  }
})
export default class App extends Vue  {
  @MapGetter({ namespace: 'vote' })
  public questions!: IQuestion[];

  @MapGetter({ namespace: 'vote' })
  public votes!: IVote[];

  @MapGetter({ namespace: 'vote' })
  public unsafeVotes!: IVote[];

  // public mounted(): void {
  //   if (this.votes.length >= this.questions.length && this.unsafeVotes.length === 0) {
  //     this.$router.push({ name: 'summary' });
  //   } else if (this.votes.length >= this.questions.length && this.unsafeVotes.length > 0) {
  //     let maxId: number = 1;
  //     this.unsafeVotes.forEach((v: IVote) => {
  //       if (maxId < v.questionId) {
  //         maxId = v.questionId;
  //       }
  //     });
  //     this.$router.push({ name: 'question', params: { id: maxId.toString() } });
  //   } else if (this.votes.length > 0) {
  //     let maxId: number = 1;
  //     this.votes.forEach((v: IVote) => {
  //       if (maxId < v.questionId) {
  //         maxId = v.questionId;
  //       }
  //     });
  //     this.$router.push({ name: 'question', params: { id: (maxId + 1).toString() } });
  //   }
  // }
}
</script>

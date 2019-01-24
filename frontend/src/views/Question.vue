<template>
  <div class="question">
    <Question :questionId="id"/>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'types-vue';
import Question from '@/components/Question.vue';

@Component({
  components: {
    Question
  }
})
export default class QuestionView extends Vue {
  get id(): number {
    return parseInt(this.$route.params.id, 10) || 0;
  }
}
</script>

<style lang="scss">
div.question {
  margin-top: 20px;
  h2 {
    margin-bottom: 20px;
  }
}
</style>

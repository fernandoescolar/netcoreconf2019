<template>
  <v-container>
    <v-layout
      text-xs-center
      wrap
    >
      <v-flex xs12>
        <v-img
          :src="require('../assets/logo.png')"
          class="my-3"
          contain
          height="150"
        ></v-img>
      </v-flex>

      <v-flex mb-4>
        <h1 class="display-2 font-weight-bold mb-3">
          Welcome to Hot Crazy C# APP
        </h1>
        <p class="subheading font-weight-regular">
          This application will help you voting the session poll. Please introduce one valid contact information (like a twitter account or your email) in order to start the poll:
        </p>
        <v-form  
          ref="form"
          v-model="valid"
          lazy-validation>
          <v-text-field 
            v-model="contactInfo" 
            label="Contact information" 
            counter="256" 
            :rules="[ value => !!value || 'Required.' ]" />
          <v-btn
            :disabled="!valid"
            color="success"
            @click="startPoll"
          >
            Start
          </v-btn>
        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
  import { Vue, Component, Watch, MapGetter, MapAction} from 'types-vue';
  
  @Component
  export default class HelloWorld extends Vue {
    public valid: boolean = true;
    public contactInfo: string = '';

    @MapGetter({ namespace: 'vote' })
    public username!: string;

    @MapAction({ namespace: 'vote' })
    public changeUsername!: (v: string) => Promise<void>;

    @Watch('username', { immediate: true })
    public onUsernameChanged(value: string): void {
      this.contactInfo = value;
    }

    public async startPoll(): Promise<void> {
      if ((this.$refs.form as HTMLFormElement).validate()) {
        await this.changeUsername(this.contactInfo);
        this.$router.push({ name: 'question', params: { id: '0' } });
      }
    }
  }
</script>

<style>

</style>

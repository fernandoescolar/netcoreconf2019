<template>
  <div class="summary">
    <v-container>
      <v-layout
        text-xs-center
        wrap
      >
        <v-flex xs12>
          <v-img
            :src="require('../assets/thanks.png')"
            class="my-3"
            contain
            height="150"
          ></v-img>
        </v-flex>

        <v-flex mb-4>
          <h1 class="display-2 font-weight-bold mb-3">
            Thanks for Voting!
          </h1>
          <p class="subheading font-weight-regular">
            Wait until the session ends to discover the end surprise, please.
          </p>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

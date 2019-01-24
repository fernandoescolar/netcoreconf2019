import Vue from 'vue';
import Vuex, { Store } from 'vuex';
import app from './app.module';
import vote from './vote.module';

Vue.use(Vuex);

const store = new Store({
    state: {},
    modules: {
      app,
      vote
    }
});

export default store;

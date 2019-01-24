import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/question/:id',
      name: 'question',
      component: () => import(/* webpackChunkName: "question" */ './views/Question.vue')
    },
    {
      path: '/summary',
      name: 'summary',
      component: () => import(/* webpackChunkName: "summary" */ './views/Summary.vue')
    },
    {
      path: '/stats',
      name: 'stats',
      component: () => import(/* webpackChunkName: "stats" */ './views/Stats.vue')
    }
  ]
});

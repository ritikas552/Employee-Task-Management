import Vue from 'vue';
import Router from 'vue-router';
import HomePage from '@/components/HomePage.vue';
import ManageEmployees from '@/components/ManageEmployees.vue';
import AssignTasks from '@/components/AssignTasks.vue';
import LoginForm from '@/components/LoginForm.vue';
import ManageTasks from '@/components/ManageTasks.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/home',
      name: 'HomePage',
      component: HomePage,
      children: [
        {
          path: 'manage-employees',
          name: 'ManageEmployees',
          component: ManageEmployees,
        },
        {
          path: 'assign-tasks',
          name: 'AssignTasks',
          component: AssignTasks,
        },
        {
          path: 'manage-tasks',
          name: 'ManageTasks',
          component: ManageTasks,
        },
      ],
    },
    {
      path: '/',
      name: 'LoginForm',
      component: LoginForm,
    },
  ],
});

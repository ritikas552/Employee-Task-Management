import Vue from 'vue';
import Router from 'vue-router';
import HomePage from './components/HomePage.vue';
import EmployeeList from './components/EmployeeList.vue';
import TaskManager from './components/TaskManager.vue';
import LoginForm from './components/LoginForm.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/home',
      name: 'HomePage',
      component: HomePage,
    },
    {
      path: '/manage-employees',
      name: 'EmployeeList',
      component: EmployeeList,
    },
    {
      path: '/assign-tasks',
      name: 'TaskManager',
      component: TaskManager,
    },
    {
      path: '/',
      name: 'LoginForm',
      component: LoginForm,
    }
  ]
});

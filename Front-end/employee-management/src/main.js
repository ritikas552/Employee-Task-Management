import Vue from 'vue';
import App from './App.vue';
import vuetify from './plugins/vuetify';
import router from './routerSetup';
import './plugins/vuetify-components';
import '@mdi/font/css/materialdesignicons.css'; // Import MDI styles

Vue.config.productionTip = false;

new Vue({
    router,
    vuetify,
    render: h => h(App),
}).$mount('#app');

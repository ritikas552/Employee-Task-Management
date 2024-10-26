
import Vue from 'vue';
import Vuetify from 'vuetify'; // Correct import for Vuetify
import 'vuetify/src/styles/main.sass'; // Import Vuetify styles

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        themes: {
            light: {
                primary: '#1976D2',
                secondary: '#424242',
                accent: '#82B1FF',
                error: '#FF5252',
                info: '#2196F3',
                success: '#4CAF50',
                warning: '#FFC107'
            },
        },
    },
});

<template>
<v-app class="background-image">
    <v-content>
        <div class="app-bar" style="display: flex; align-items: center; padding-left: 40rem; background-color: #1976D2; color: white;">
            <h1 style="flex-grow: 1; margin: 0;">
                <template v-if="role === 'Admin'">
                    Welcome to Employee Task Management
                </template>
                <template v-else>
                    Welcome {{ firstName }}
                </template>
            </h1>
            <v-spacer></v-spacer>
            <div>
                <router-link v-if="role === 'Admin'" to="/home" style="text-decoration: none;">
                    <v-btn color="green" style="margin-right: 1rem;">
                        Home
                    </v-btn>
                </router-link>
                <router-link v-if="role === 'Admin'" to="/home/manage-employees" style="text-decoration: none;">
                    <v-btn color="green" style="margin-right: 1rem;">
                        Manage Employees
                    </v-btn>
                </router-link>
                <router-link v-if="role === 'Admin'" to="/home/assign-tasks" style="text-decoration: none;">
                    <v-btn color="green" style="margin-right: 1rem;">
                        Assign Tasks
                    </v-btn>
                </router-link>

                <router-link v-else to="/home/manage-tasks" style="text-decoration: none;">
                    <v-btn color="green" style="margin-right: 1rem;">
                        Manage Tasks
                    </v-btn>
                </router-link>
                <v-btn color="green" @click="logout" style="margin-left: 1rem;">
                    Logout
                </v-btn>
            </div>
        </div>
        <router-view></router-view>
    </v-content>
</v-app>
</template>

<script>
export default {
    name: 'HomePage',
    data() {
        return {
            role: null,
            firstName: '',
        };
    },
    created() {
        this.role = localStorage.getItem('userRole');
        this.firstName = localStorage.getItem('userName');
    },
    methods: {
        logout() {
            localStorage.removeItem('authToken');
            localStorage.removeItem('userRole');
            localStorage.removeItem('userName');
            this.$router.push('/');
        },
    }
};
</script>

<style scoped>
.app-bar {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 1000;
}

.v-content {
    margin-top: 64px;
}
</style><style scoped>
.app-bar {
    background-color: #1976D2;
    color: white;
    padding: 16px;
    position: fixed;
    width: 100%;
    top: 0;
    z-index: 10;
}

.flex-column {
    display: flex;
    flex-direction: column;
    padding-top: 4rem;
    margin-bottom: 0;
}

.text-center {
    text-align: center;
}

.background-image {
    background-image: url("C:\Users\Admin\Downloads\large-room-with-large-window-that-has-plant-it_1048609-1170.avif");
}
</style>

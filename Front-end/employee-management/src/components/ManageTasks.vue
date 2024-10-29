<template>
    <v-container style="max-width: 1280px;">
        <v-data-table
          :headers="headers"
          :items="tasks"
          class="mt-5"
          item-key="id"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title>Tasks</v-toolbar-title>
            </v-toolbar>
          </template>
    
          <template slot="items" slot-scope="{ item }">
            <tr>
              <td>{{ item.title }}</td>
              <td>{{ item.email }}</td>
            </tr>
          </template>
    
        </v-data-table>
    </v-container>
    </template>
    
    <script>
    import axios from 'axios';
    
    export default {
        name: 'AssignTasks',
        data() {
            return {
                tasks: [],
                headers: [{
                        text: 'Task Name',
                        value: 'title'
                    },
                    {
                        text: 'Email',
                        value: 'email'
                    }
                ],
            };
        },
        created() {
            this.fetchTasks();
        },
        methods: {
            async fetchTasks() {
                try {
                    const token = localStorage.getItem('authToken');
    
                    if (!token) {
                        console.error('No authentication token found');
                        return;
                    }
    
                    const response = await axios.get('https://localhost:7046/api/Employee/GetAllTasks', {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    });
    
                    console.log("Task", response.data);
    
                    this.tasks = response.data.map(taskData => ({
                        id: taskData.id,
                        title: taskData.title,
                        email: taskData.assignedTo
                    }));
                } catch (error) {
                    console.error('Error fetching tasks:', error.response ? error.response.data : error.message);
                }
            },
        }
    }
    </script>
    
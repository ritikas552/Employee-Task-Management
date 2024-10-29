<template>
<v-container style="max-width: 1280px;">
    <div class="text-right">
        <v-btn color="primary" @click="showTaskForm = true">Create a new Task</v-btn>
    </div>

    <v-dialog v-model="showTaskForm" max-width="25%">
        <v-card>
            <v-card-title>
                <span class="headline">Create New Task</span>
            </v-card-title>

            <v-card-text>
                <v-form ref="taskForm" v-model="isValid">
                    <v-text-field label="Title" v-model="newTask.title" :rules="[v => !!v || 'Title is required']"></v-text-field>

                    <v-text-field label="Status" v-model="newTask.status"></v-text-field>

                    <v-textarea label="Description" v-model="newTask.description" :rules="[v => !!v || 'Description is required']"></v-textarea>

                    <v-select v-model="selectedEmployee" :items="employees.map(emp => emp.email)" label="Assign to Employee" :rules="[v => !!v || 'Employee is required']"></v-select>
                </v-form>
            </v-card-text>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="cancel">Cancel</v-btn>
                <v-btn color="primary" text @click="submitTask">Create</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

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
            showTaskForm: false,
            employees: [],
            selectedEmployee: '',
            newTask: {
                title: '',
                description: '',
                email: '',
                status: ''
            },
            isValid: false,
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
        this.fetchEmployees();
        this.fetchTasks();
    },
    methods: {
        cancel() {
            this.showTaskForm = false;
            this.resetForm();
        },
        async fetchEmployees() {
            try {
                const token = localStorage.getItem('authToken');

                if (!token) {
                    console.error('No authentication token found');
                    return;
                }

                const response = await axios.get('https://localhost:7046/api/Admin/GetAllEmployees', {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                this.employees = response.data.map(empData => ({
                    email: empData.email,
                    firstName: empData.firstName,
                    lastName: empData.lastName
                }));
            } catch (error) {
                console.error('Error fetching employees:', error.response ? error.response.data : error.message);
            }
        },
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
        resetForm() {
            this.newTask.title = '';
            this.newTask.description = '';
            this.newTask.status = '';
            this.newTask.email = '';
            this.selectedEmployee = '';
        },
        async submitTask() {
            const isFormValid = this.$refs.taskForm.validate();
            if (!isFormValid) return;

            this.newTask.email = this.selectedEmployee;

            try {
                const token = localStorage.getItem('authToken');

                const response = await axios.post('https://localhost:7046/api/Admin/CreateTask', this.newTask, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                console.log('Task created:', response.data);
                this.cancel();
                this.fetchTasks();
            } catch (error) {
                console.error('Error creating task:', error.response ? error.response.data : error.message);
            }
        }
    }
}
</script>

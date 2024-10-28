<template>
    <v-app class="background-image">
      <v-content>
        <div class="app-bar" style="display: flex;">
          <div>
            <h1 style="position: relative; margin-left: 42rem;">
              <template v-if="role === 'Admin'">
                Welcome to Employee Task Management
              </template>
              <template v-else>
                Welcome {{ firstName }}
              </template>
            </h1>
          </div>
          <v-spacer></v-spacer>
          <div>
            <v-btn style="color: white;" icon @click="CreateEmployees">
              <v-icon>mdi-account-plus</v-icon>
            </v-btn>
            <v-btn style="color: white;" icon @click="assignTasks">
              <v-icon>mdi-plus-circle</v-icon>
            </v-btn>
            <v-btn style="color: white;" icon @click="viewReports">
              <v-icon>mdi-file-document</v-icon>
            </v-btn>
          </div>
        </div>

  <v-container style="max-width: 1280px; padding-top: 5rem;">
    <div class="flex-container" style="display: flex; flex-direction: column; padding-top: 4rem;">
      <v-data-table
        v-if="role === 'Admin'"
        :headers="headers"
        :items="employees" 
        class="elevation-1"
        :search="search"
        :items-per-page="5"
        aria-disabled="true"
      >
        <template slot="top">
          <v-toolbar flat>
            <v-toolbar-title>Employees</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-text-field
              v-model="search"
              label="Search"
              single-line
              hide-details
            ></v-text-field>
          </v-toolbar>
        </template>
        <template slot="items" slot-scope="{ item }">
          <tr>
            <td>{{ item.firstName }}</td>
            <td>{{ item.lastName }}</td>
            <td>{{ item.email }}</td>
            <td>
              <v-icon small @click="editEmployee(item)">
                mdi-pencil
              </v-icon>
              <v-icon small @click="deleteEmployee(item)">
                mdi-delete
              </v-icon>
            </td>
          </tr>
        </template>
      </v-data-table>
    </div>
  </v-container>
  
  <v-dialog v-model="showForm" max-width="500px">
        <v-card>
          <v-card-title>
            <span class="headline">{{ isEditMode ? 'Edit Employee' : 'Add Employee' }}</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="form" v-model="valid">
              <v-text-field
                v-model="newEmployee.firstName"
                :rules="[v => !!v || 'First name is required']"
                label="First Name"
              ></v-text-field>
              <v-text-field
                v-model="newEmployee.lastName"
                :rules="[v => !!v || 'Last name is required']"
                label="Last Name"
              ></v-text-field>
              <v-text-field
                v-model="newEmployee.email"
                :rules="emailRules"
                label="Email"
                :disabled="isEditMode"
              ></v-text-field>
              <v-text-field 
                v-if="!isEditMode"
                v-model="newEmployee.password"
                label="Password"
                type="password"
              ></v-text-field>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-btn color="blue darken-1" text @click="cancel">Cancel</v-btn>
            <v-btn color="blue darken-1" text @click="submitForm">{{ isEditMode ? 'Update' : 'Add' }}</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      </v-content>
    </v-app>
  </template>
  
  <script>
import axios from "axios";

export default {
  name: 'HomePage',
  data() {
    return {
      role: null,
      firstName: '',
      search: '',
      employees: [],
      headers: [
        { text: 'First Name', value: 'firstName' },
        { text: 'Last Name', value: 'lastName' },
        { text: 'Email', value: 'email' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      showForm: false,
      newEmployee: {
        firstName: '',
        lastName: '',
        email: '',
        password: ''
      },
      valid: false,
      emailRules: [
        v => (this.submitted || !!v) || 'Email is required',
        v => /.+@.+\..+/.test(v) || 'Email must be valid',
      ],
      submitted: false,
      isEditMode: false
    };
  },
  created() {
    this.role = localStorage.getItem('userRole');
    this.firstName = localStorage.getItem('userName');
    if (this.role === 'Admin') {
      this.fetchEmployees();
    }
  },
  methods: {
    CreateEmployees() {
      this.newEmployee = {
        firstName: '',
        lastName: '',
        email: '',
        password: ''
      };
      this.showForm = true;
      this.submitted = false;
    },
    assignTasks() {
      this.$router.push('/assign-tasks');
    },
    viewReports() {
      this.$router.push('/view-reports');
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
        
        console.log("Employees Data", this.employees);
    
  } catch (error) {
    console.error('Error fetching employees:', error.response ? error.response.data : error.message);
  }
    },
    editEmployee(employee) {
      this.isEditMode = true;
      this.newEmployee.email = employee.email;
      this.newEmployee.firstName = employee.firstName;
      this.newEmployee.lastName = employee.lastName;
      this.showForm = true;
    },
    async deleteEmployee(employee) {
    const token = localStorage.getItem('authToken');
    
    try {
      // Send DELETE request to the API to remove the employee
      await axios.delete(`https://localhost:7046/api/Admin/DeleteEmployee/${employee.email}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      
      // Fetch updated employees after deletion
      await this.fetchEmployees();
      console.log(`Employee ${employee.email} deleted successfully.`);
      
    } catch (error) {
      console.error('Error deleting employee:', error.response ? error.response.data : error.message);
    }
  },
    async submitForm() {
      this.submitted = true;
      if (this.$refs.form.validate()) {
        try {
          const token = localStorage.getItem('authToken');
          if (this.isEditMode) {
            console.log("this.newEmployee.id", this.newEmployee)
            await axios.put(`https://localhost:7046/api/Admin/UpdateEmployee/${this.newEmployee.email}`, this.newEmployee, {
              headers: {
                Authorization: `Bearer ${token}`
              }
            });
          } else {
            await axios.post('https://localhost:7046/api/Admin/CreateEmployee', this.newEmployee, {
              headers: {
                Authorization: `Bearer ${token}` 
              }
            });
          }
          this.cancel();
          await this.fetchEmployees();
        } catch (error) {
          console.error('Error saving employee:', error);
        }
      }
    },
    cancel() {
      this.showForm = false;
      this.isEditMode = false;
      this.newEmployee = {
        firstName: '',
        lastName: '',
        email: '',
        password: ''
      };
      this.valid = true;
      this.submitted = false; // Reset submitted flag
    },
  },
};
</script>
  
  <style scoped>
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
  
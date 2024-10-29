<template>
    <v-container style="max-width: 1280px;">
        <div class="text-right"><v-btn color="primary" @click="CreateEmployees">Create a new Employee</v-btn></div>
    <div class="flex-container" style="display: flex; flex-direction: column;">
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
              <v-icon small @click="editEmployee(item)" color="grey">
                mdi-pencil
              </v-icon>
              <v-icon small @click="deleteEmployee(item)" color="grey"> 
                mdi-delete
              </v-icon>
            </td>
          </tr>
        </template>
      </v-data-table>

      <v-dialog v-model="showForm" max-width="600px">
        <v-card>
          <v-card-title>
            <span>{{ isEditMode ? 'Edit Employee' : 'Add Employee' }}</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="form" v-model="valid">
              <v-text-field
                v-model="newEmployee.firstName"
                :rules="[v => !!v || 'First Name is required']"
                label="First Name"
                required
              ></v-text-field>
              <v-text-field
                v-model="newEmployee.lastName"
                :rules="[v => !!v || 'Last Name is required']"
                label="Last Name"
                required
              ></v-text-field>
              <v-text-field
                v-model="newEmployee.email"
                :rules="emailRules"
                label="Email"
                required
              ></v-text-field>
              <v-text-field
                v-model="newEmployee.password"
                :rules="[v => (isEditMode || !!v) || 'Password is required']"
                label="Password"
                v-if="!isEditMode"
                type="password"
              ></v-text-field>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" @click="submitForm">{{ isEditMode ? 'Update' : 'Create' }}</v-btn>
            <v-btn color="primary" @click="cancel">Cancel</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

    </div>
  </v-container>

</template>

<script>
import axios from "axios";

export default {
  name: 'ManageEmployeePage',
  data() {
    return {
      role: null,
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
        v => !!v || 'Email is required',
        v => /.+@.+\..+/.test(v) || 'Email must be valid',
      ],
      submitted: false,
      isEditMode: false,
    };
  },
  created() {
    this.role = localStorage.getItem('userRole');
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
      this.isEditMode = false;
      this.submitted = false;
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

        console.log("Employees Data", response.data);

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
        await axios.delete(`https://localhost:7046/api/Admin/DeleteEmployee/${employee.email}`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        
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
      this.submitted = false;
    },
  },
};
</script>

<style>
.text-right{
    text-align: right;
}
</style>
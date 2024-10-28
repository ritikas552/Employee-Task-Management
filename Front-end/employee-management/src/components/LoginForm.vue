<template>
    <v-app>
      <v-content class="background-image">
        <v-container fluid fill-height class="custom-container">
          <v-layout align-center justify-center class="custom-layout">
            <v-card class="elevation-12">
              <v-toolbar dark color="primary">
                <v-toolbar-title>Login Form</v-toolbar-title>
              </v-toolbar>
              <v-card-text>
                <form ref="form" @submit.prevent="login">
                  <v-text-field
                    v-model="email"
                    name="email"
                    label="Email"
                    type="text"
                    placeholder="Email"
                    :rules="usernameRules"
                    required
                  ></v-text-field>
  
                  <v-text-field
                    v-model="password"
                    name="password"
                    label="Password"
                    type="password"
                    placeholder="Password"
                    :rules="passwordRules"
                    required
                  ></v-text-field>
  
                  <v-btn
                    type="submit"
                    class="mt-4"
                    color="primary"
                    :disabled="!isFormValid"
                    >Login</v-btn
                  >
                  <v-snackbar v-model="snackbar" :timeout="3000" color="error">
                    {{ errorMessage }}
                  </v-snackbar>
                </form>
              </v-card-text>
            </v-card>
          </v-layout>
        </v-container>
      </v-content>
    </v-app>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    name: "LoginForm",
    data() {
      return {
        email: "",
        password: "",
        snackbar: false,
        errorMessage: "",
      };
    },
    computed: {
      usernameRules() {
        return [
          (v) => !!v || "Username is required",
          (v) =>
            /.+@.+\..+|^[a-zA-Z0-9]+$/.test(v) ||
            "Must be a valid email or alphanumeric",
        ];
      },
      passwordRules() {
        return [
          (v) => !!v || "Password is required",
          (v) => (v && v.length >= 8) || "Minimum 8 characters",
          (v) =>
            (v && /[A-Z]/.test(v)) ||
            "Must contain at least one uppercase letter",
          (v) =>
            (v && /[a-z]/.test(v)) ||
            "Must contain at least one lowercase letter",
          (v) => (v && /[0-9]/.test(v)) || "Must contain at least one number",
          (v) =>
            (v && /[!@#$%^&*(),.?":{}|<>]/.test(v)) ||
            "Must contain at least one special character",
        ];
      },
      isFormValid() {
        const usernameValid = this.usernameRules.every(
          (rule) => rule(this.email) === true
        );
        const passwordValid = this.passwordRules.every(
          (rule) => rule(this.password) === true
        );
        return usernameValid && passwordValid;
      },
    },
    methods: {
      async login() {
        const API_URL = "https://localhost:7046/api/Authentication/login";
        try {
          const response = await axios.post(API_URL, {
            email: this.email,
            password: this.password,
          });
  
          console.log(response);
          console.log(response.data.user.UserName);
          const token = response.data.token;
          const userRole = response.data.userRole;
          const userName = response.data.user.userName;
          localStorage.setItem("authToken", token);
          localStorage.setItem("userRole", userRole);
          localStorage.setItem("userName", userName);
          this.$router.push('/home');
  
        } catch (error) {
          this.errorMessage = error.response?.data?.message || "Login failed";
          this.snackbar = true;
          console.error(error);
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .v-snackbar {
    position: absolute;
    bottom: 20px;
    left: 20px;
  }
  
  .custom-container {
    max-width: 22%;
  }
  
  .custom-layout {
    padding-left: 50rem;
    padding-top: 12rem;
    display: contents; /* Consider using flexbox styles instead */
  }
  
  .background-image {
    background-image: url("C:\Users\Admin\Downloads\16282276_rm222batch2-mind-03.jpg");
    background-size: cover;
    background-position: center;
  }
  </style>
  
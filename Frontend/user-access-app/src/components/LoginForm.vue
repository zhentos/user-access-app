<template>
  <div class="login-container">
    <p v-if="error" class="error-message">{{ error }}</p>
    <h1 class="login-title">Login</h1>
    <form @submit.prevent="login" class="login-form">
      <div class="form-group">
        <label for="email">Email</label>
        <input type="email" id="email" v-model="email" required />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input type="password" id="password" v-model="password" required />
      </div>
      <button type="submit" class="login-button">Login</button>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import type { LoginUserDto } from '@/services/authService'
import { authService } from '@/services/authService'

export default defineComponent({
  setup() {
    const email = ref('')
    const password = ref('')
    const error = ref('')
    const errorText = 'We could not log you in. Please check your username/password and try again.'

    const login = async () => {
      try {
        const user: LoginUserDto = { email: email.value, password: password.value }
        const success = await authService.login(user) // Call login method

        if (success) {
          const currentUser = authService.getCurrentUser()
          if (currentUser && currentUser.isActive) {
            // Check if user is active
            window.location.href = '/welcome' // Redirect to welcome page
          } else {
            console.log('Your account is inactive.')
            password.value = ''
            error.value = errorText
          }
        } else {
          console.log('Invalid email or password.')
          error.value = errorText
          password.value = ''
        }
      } catch (err) {
        console.log(err)
        password.value = ''
        error.value = errorText
      }
    }

    return { email, password, error, login }
  },
})
</script>

<style scoped>
.login-container {
  background-color: white; /* White background for the form */
  padding: 2rem; /* Padding around the form */
  border-radius: 8px; /* Rounded corners */
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1); /* Subtle shadow */

  width: 600px; /* Full width for smaller screens */
  max-width: 800px; /* Set max width for larger screens */
}

.login-title {
  text-align: center; /* Center title */
}

.form-group {
  margin-bottom: 1.5rem; /* Space between form groups */
}

label {
  display: block; /* Block display for labels */
}

input {
  width: 100%; /* Full width input */
  padding: 0.75rem; /* Padding inside input */
  border: 1px solid #ccc; /* Light border */
  border-radius: 4px; /* Rounded corners for input */
}

input:focus {
  border-color: #007bff; /* Change border color on focus */
}

.login-button {
  width: 100%; /* Full width button */
  padding: 0.75rem;
  background-color: #007bff; /* Primary button color */
  color: white; /* White text color */
  border: none; /* No border */
  border-radius: 4px; /* Rounded corners for button */
}

.login-button:hover {
  background-color: #0056b3; /* Darker shade on hover */
}

.error-message {
  color: red; /* Red color for error messages */
}
</style>

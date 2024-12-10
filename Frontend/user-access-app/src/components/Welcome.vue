<template>
  <div>
    <h1>Welcome, {{ username }}!</h1>
    <p>This page is for authenticated users only.</p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { authService } from '@/services/authService'

export default defineComponent({
  name: 'Welcome',
  computed: {
    username() {
      const user = authService.getCurrentUser()
      return user && user.userName ? user.userName : 'Guest'
    },
  },
})
</script>

<style scoped>
h1 {
  color: #4caf50;
}

p {
  font-size: 22px;
}
</style>

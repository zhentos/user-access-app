<template>
  <div class="container">
    <div v-if="userStore.changesQueue.size > 0" class="alert alert-warning">
      One or more users have been modified.
      <button @click="saveChanges" class="btn btn-secondary">Save</button>
    </div>

    <table class="table">
      <thead>
        <tr>
          <th>Username</th>
          <th>Active</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in userStore.users" :key="user.id">
          <td @click="openEditPanel(user)" class="clickable">{{ user.userName }}</td>
          <td>{{ user.isActive ? 'Yes' : 'No' }}</td>
        </tr>
      </tbody>
    </table>

    <!-- Edit User Popup -->
    <div v-if="isEditPanelOpen" class="edit-user-panel">
      <h4>Edit User</h4>
      <label>Username:</label>
      <input type="text" v-model="currentUser.userName" disabled />
      <label>Active:</label>
      <input type="checkbox" v-model="currentUser.isActive" />
      <div class="button-group">
        <button @click="confirmChanges" class="btn btn-success">Ok</button>
        <button @click="closeEditPanel" class="btn btn-secondary">Cancel</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import axios from 'axios'
import { defineComponent, ref, onMounted } from 'vue'
import { useUserStore } from '@/stores/userStore'

export default defineComponent({
  setup() {
    const userStore = useUserStore()
    const currentUser = ref(null) // Currently editing user
    const isEditPanelOpen = ref(false) // Control visibility of edit panel
    const originalStatus = ref(false) // Track original active status

    const fetchUsers = async () => {
      try {
        const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users`)

        if (response.status !== 200) {
          throw new Error('Failed to fetch users')
        }

        const users = response.data // Get the users from the response
        userStore.setUsers(users) // Set users in the store
      } catch (error) {
        console.error('Error fetching users:', error)
      }
    }

    onMounted(() => {
      fetchUsers()
    })

    const openEditPanel = (user) => {
      currentUser.value = { ...user } // Clone user data for editing
      originalStatus.value = user.isActive // Store original status
      isEditPanelOpen.value = true // Open edit panel
    }

    const closeEditPanel = () => {
      isEditPanelOpen.value = false
      currentUser.value = null // Clear current user
    }

    const confirmChanges = () => {
      if (currentUser.value.isActive !== originalStatus.value) {
        userStore.updateUserStatus(currentUser.value.id, currentUser.value.isActive) // Update status in store if changed
      }
      closeEditPanel() // Dismiss popup after confirming changes
    }

    const updateUserStatus = (userId, isActive) => {
      userStore.updateUserStatus(userId, isActive) // Update status in store immediately when checkbox is toggled
    }

    const saveChanges = async () => {
      await userStore.saveChanges() // Save changes via the store action
    }

    return {
      userStore,
      currentUser,
      isEditPanelOpen,
      openEditPanel,
      closeEditPanel,
      confirmChanges,
      updateUserStatus,
      saveChanges,
    }
  },
})
</script>

<style scoped>
.container {
  max-width: 800px; /* Set a maximum width for the container */
  width: 600px;
  margin: 0 auto; /* Center the container */
  padding: 20px; /* Add padding around the container */
  background-color: #ffffff; /* White background for the container */
  border-radius: 8px; /* Rounded corners */
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1); /* Subtle shadow */
}

.alert {
  padding: 1rem;
  margin-bottom: 1rem;
  border-radius: 5px;
  color: #007bff;
}

.title {
  margin-bottom: 1.5rem; /* Space below title */
}

.table {
  width: 100%; /* Full width table */
  border-collapse: collapse; /* Collapse borders */
}

.table th,
.table td {
  padding: 12px; /* Padding inside table cells */
  text-align: left; /* Align text to the left */
}

.table th {
  background-color: #007bff; /* Header background color */
  color: white; /* Header text color */
}

.table tr:nth-child(even) {
  background-color: #f2f2f2; /* Zebra striping for even rows */
}

.clickable {
  cursor: pointer; /* Pointer cursor for clickable rows */
}

.edit-user-panel {
  border: 1px solid #ccc;
  padding: 20px;
  margin-top: 10px;
  background-color: #f9f9f9; /* Light background for edit panel */
}

.button-group {
  display: flex;
  justify-content: space-between; /* Space between buttons */
}

.edit-user-panel {
  border: 1px solid #ccc; /* Light border */
  padding: 20px; /* Padding inside the panel */
  margin-top: 10px; /* Space above the panel */
  background-color: #f9f9f9; /* Light background for edit panel */
  border-radius: 8px; /* Rounded corners */
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* Subtle shadow */
}

.edit-user-panel h4 {
  margin-bottom: 1rem; /* Space below title */
  font-size: 1.5rem; /* Increase font size for better visibility */
  font-weight: bold; /* Make text bold */
  color: #007bff; /* Highlight color for the title */
}

.edit-user-panel label {
  display: block; /* Block display for labels */
  margin-bottom: 0.5rem; /* Space below label */
}

.edit-user-panel input[type='text'] {
  width: calc(100% - 12px); /* Full width minus padding */
  padding: 8px; /* Padding inside input */
  border: 1px solid #ccc; /* Light border */
  border-radius: 4px; /* Rounded corners for input */
}

.edit-user-panel input[type='checkbox'] {
  margin-top: 0.5rem; /* Space above checkbox */
  transform: scale(1.4); /* Increase checkbox size by 80% */
}

/* Button styles */
.btn {
  padding: 10px 15px; /* Padding inside buttons */
  border: none;
  border-radius: 4px; /* Rounded corners for buttons */
  cursor: pointer;
}

.btn-success {
  background-color: #28a745;
  color: white;
}

.btn-success:hover {
  background-color: #218838;
}

.btn-secondary {
  background-color: #6c757d; /* Gray background for secondary button */
  color: white;
}

.btn-secondary:hover {
  background-color: #5a6268; /* Darker gray on hover */
}

/* Button group styling to space buttons apart */
.button-group {
  display: flex;
  justify-content: space-between; /* Space between buttons */
  margin-top: 1rem; /* Space above button group */
}
.alert {
  padding: 1rem; /* Padding inside the alert */
  margin-bottom: 1rem; /* Space below the alert */
  border-radius: 5px; /* Rounded corners */
  background-color: #fff3cd; /* Light yellow background for warning */
  color: #856404; /* Darker text color for contrast */
}

.alert-warning {
  font-size: 1.2rem; /* Increase text size */
}

.alert button {
  margin-left: 10px; /* Space between text and button */
}
</style>

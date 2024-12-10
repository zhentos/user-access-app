import { defineStore } from 'pinia'
import axios from 'axios'
import { authService } from '@/services/authService'

export const useUserStore = defineStore('user', {
  state: () => ({
    users: [] as Array<{ id: number; username: string; isActive: boolean }>,
    changesQueue: new Map<number, boolean>(), // To track changes
  }),

  actions: {
    setUsers(users) {
      this.users = users // Set fetched users in state
    },

    updateUserStatus(userId, isActive) {
      const user = this.users.find((u) => u.id === userId)
      if (user) {
        user.isActive = isActive // Update local state immediately
        this.changesQueue.set(userId, isActive) // Track change
      }
    },

    async saveChanges() {
      if (this.changesQueue.size > 0) {
        const updates = Array.from(this.changesQueue.entries()).map(([id, isActive]) => ({
          UserId: id,
          IsActive: isActive,
        }))

        try {
          const response = await axios.post(
            `${import.meta.env.VITE_API_BASE_URL}/users/batch-update`,
            updates,
            {
              headers: { 'Content-Type': 'application/json' },
            },
          )

          if (response.status !== 200) {
            throw new Error('Failed to save changes')
          }
          // Check if the current user's status has changed
          const currentUser = authService.getCurrentUser()
          const updatedUser = updates.find((u) => u.UserId === currentUser.userId)

          if (updatedUser && updatedUser.IsActive === false) {
            authService.logout() // Clear session if IsActive is false

            console.log('You have been logged out.')
          }

          this.changesQueue.clear() // Clear the queue after successful save
        } catch (error) {
          console.error('Error saving changes:', error)
        }
      }
    },
  },
})

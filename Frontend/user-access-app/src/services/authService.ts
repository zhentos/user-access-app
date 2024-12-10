import axios from 'axios'

export interface LoginUserDto {
  email: string
  password: string
}

export const authService = {
  async login(user: LoginUserDto): Promise<boolean> {
    try {
      const response = await axios.post(`${import.meta.env.VITE_API_BASE_URL}/auth/login`, user)

      // Check for successful response
      if (response.status === 200 && response.data) {
        localStorage.setItem('user', JSON.stringify(response.data)) // Save user data in local storage
        return true // Login successful
      }
    } catch (error) {
      // Handle errors based on the response status
      if (axios.isAxiosError(error) && error.response) {
        switch (error.response.status) {
          case 400:
          case 401:
            console.error('Invalid credentials:', error.response.data)
            return false // Invalid credentials
          default:
            console.error('An unexpected error occurred:', error)
            throw error // Rethrow for further handling if needed
        }
      }
    }
    return false // Default to false if something went wrong
  },

  logout() {
    localStorage.removeItem('user') // Remove user data from local storage
  },

  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user') || '{}') // Get current user from local storage
  },
}

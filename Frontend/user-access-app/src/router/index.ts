import { createRouter, createWebHistory } from 'vue-router'
import { authService } from '@/services/authService'

import LoginForm from '@/components/LoginForm.vue'
import Welcome from '@/components/Welcome.vue'
import Users from '@/components/Users.vue'

const routes = [
  { path: '/', component: LoginForm },
  { path: '/login', component: LoginForm },
  { path: '/welcome', component: Welcome, meta: { requiresAuth: true } },
  { path: '/users', component: Users },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// Navigation guard to protect routes
router.beforeEach((to, from, next) => {
  const currentUser = authService.getCurrentUser()

  if (to.meta.requiresAuth && (!currentUser || !currentUser.isActive)) {
    next('/login') // Redirect to login page if not authenticated or inactive
  } else {
    next() // Proceed to route
  }
})

export default router

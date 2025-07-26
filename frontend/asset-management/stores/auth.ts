// stores/auth.ts
import { defineStore } from 'pinia'

interface User {
  id: number
  username: string
  email: string
  fullName: string
  role: string
  isActive: boolean
}

interface AuthState {
  user: User | null
  token: string | null
  isAuthenticated: boolean
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    user: null,
    token: null,
    isAuthenticated: false
  }),

  getters: {
    isAdmin: (state) => state.user?.role === 'Admin',
    currentUser: (state) => state.user
  },

  actions: {
    async login(username: string, password: string) {
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.post('/auth/login', {
          username,
          password
        })
        
        this.token = response.data.token
        this.user = response.data.user
        this.isAuthenticated = true
        
        // Store token in cookie
        const tokenCookie = useCookie('auth-token', {
          httpOnly: false,
          secure: false,
          sameSite: 'lax',
          maxAge: 60 * 60 * 24 * 7 // 7 days
        })
        tokenCookie.value = this.token
        
        // Set default auth header
        $axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
        
        return { success: true }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.response?.data?.message || 'Login failed' 
        }
      }
    },
    
    logout() {
      this.user = null
      this.token = null
      this.isAuthenticated = false
      
      const tokenCookie = useCookie('auth-token')
      tokenCookie.value = null
      
      const { $axios } = useNuxtApp()
      delete $axios.defaults.headers.common['Authorization']
      
      navigateTo('/login')
    },
    
    initializeAuth() {
      const tokenCookie = useCookie('auth-token')
      const token = tokenCookie.value
      
      if (token) {
        this.token = token
        this.isAuthenticated = true
        
        // Decode JWT to get user info
        try {
          const payload = JSON.parse(atob(token.split('.')[1]))
          this.user = {
            id: parseInt(payload.nameid),
            username: payload.unique_name,
            email: payload.email,
            fullName: payload.FullName,
            role: payload.role,
            isActive: true
          }
          
          const { $axios } = useNuxtApp()
          $axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
        } catch (error) {
          this.logout()
        }
      }
    }
  }
})
<!-- layouts/default.vue -->
<template>
    <div class="min-h-screen bg-gray-50">
      <!-- Navigation -->
      <nav class="bg-white shadow-sm">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex justify-between h-16">
            <div class="flex">
              <div class="flex-shrink-0 flex items-center">
                <h1 class="text-xl font-semibold text-gray-900">IT Asset Management</h1>
              </div>
              <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
                <NuxtLink
                  to="/dashboard"
                  class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                  :class="{ 'border-indigo-500 text-gray-900': $route.path === '/dashboard' }"
                >
                  Dashboard
                </NuxtLink>
                <NuxtLink
                  to="/assets"
                  class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
                  :class="{ 'border-indigo-500 text-gray-900': $route.path.startsWith('/assets') }"
                >
                  Assets
                </NuxtLink>
              </div>
            </div>
            <div class="flex items-center">
              <div class="ml-3 relative">
                <div class="flex items-center space-x-4">
                  <span class="text-sm text-gray-700">
                    {{ authStore.currentUser?.fullName }} ({{ authStore.currentUser?.role }})
                  </span>
                  <button
                    @click="logout"
                    class="text-sm text-gray-500 hover:text-gray-700"
                  >
                    Logout
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </nav>
  
      <!-- Main content -->
      <main>
        <slot />
      </main>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
  
  const authStore = useAuthStore()
  const router = useRouter()
  
  const logout = () => {
    authStore.logout()
  }
  </script>
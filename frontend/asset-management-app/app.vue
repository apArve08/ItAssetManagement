<!-- app.vue -->
<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'

// Initialize auth on app mount
const authStore = useAuthStore()
onMounted(() => {
  authStore.initializeAuth()
})
</script>
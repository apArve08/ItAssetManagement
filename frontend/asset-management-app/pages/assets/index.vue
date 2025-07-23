<template>
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="sm:flex sm:items-center">
        <div class="sm:flex-auto">
          <h1 class="text-2xl font-semibold text-gray-900">Assets</h1>
          <p class="mt-2 text-sm text-gray-700">
            A list of all assets in your company including their details and status.
          </p>
        </div>
        <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none space-x-3">
          <button
            @click="exportAssets"
            class="inline-flex items-center justify-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 sm:w-auto"
          >
            Export to Excel
          </button>
          <NuxtLink
            to="/assets/new"
            class="inline-flex items-center justify-center rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 sm:w-auto"
          >
            Add Asset
          </NuxtLink>
        </div>
      </div>
  
      <!-- Search -->
      <div class="mt-6">
        <input
          v-model="searchTerm"
          type="text"
          placeholder="Search assets by tag, name, serial number, or location..."
          class="block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
          @input="debouncedSearch"
        />
      </div>
  
      <!-- Table -->
      <div class="mt-8 flex flex-col">
        <div class="-my-2 -mx-4 overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div class="inline-block min-w-full py-2 align-middle md:px-6 lg:px-8">
            <div class="overflow-hidden shadow ring-1 ring-black ring-opacity-5 md:rounded-lg">
              <table class="min-w-full divide-y divide-gray-300">
                <thead class="bg-gray-50">
                  <tr>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Asset Tag</th>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Name</th>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Category</th>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Status</th>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Assigned To</th>
                    <th class="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">Location</th>
                    <th class="relative py-3.5 pl-3 pr-4 sm:pr-6">
                      <span class="sr-only">Actions</span>
                    </th>
                  </tr>
                </thead>
                <tbody class="divide-y divide-gray-200 bg-white">
                  <tr v-for="asset in assets" :key="asset.id">
                    <td class="whitespace-nowrap px-3 py-4 text-sm font-medium text-gray-900">
                      {{ asset.assetTag }}
                    </td>
                    <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                      {{ asset.name }}
                    </td>
                    <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                      {{ asset.category }}
                    </td>
                    <td class="whitespace-nowrap px-3 py-4 text-sm">
                      <span
                        class="inline-flex rounded-full px-2 text-xs font-semibold leading-5"
                        :class="{
                          'bg-green-100 text-green-800': asset.status === 'Available',
                          'bg-blue-100 text-blue-800': asset.status === 'Assigned',
                          'bg-yellow-100 text-yellow-800': asset.status === 'Maintenance',
                          'bg-gray-100 text-gray-800': asset.status === 'Retired'
                        }"
                      >
                        {{ asset.status }}
                      </span>
                    </td>
                    <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                      {{ asset.assignedToUserName || '-' }}
                    </td>
                    <td class="whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                      {{ asset.location || '-' }}
                    </td>
                    <td class="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-6">
                      <NuxtLink
                        :to="`/assets/${asset.id}`"
                        class="text-indigo-600 hover:text-indigo-900 mr-3"
                      >
                        View
                      </NuxtLink>
                      <NuxtLink
                        :to="`/assets/${asset.id}/edit`"
                        class="text-indigo-600 hover:text-indigo-900 mr-3"
                      >
                        Edit
                      </NuxtLink>
                      <button
                        v-if="authStore.isAdmin"
                        @click="deleteAsset(asset.id)"
                        class="text-red-600 hover:text-red-900"
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
              <div v-if="assets.length === 0" class="text-center py-6 text-gray-500">
                No assets found
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { debounce } from 'lodash-es'
  import { useAuthStore } from '~/stores/auth'
  import { useAssetsStore } from '~/stores/assets'
  
  definePageMeta({
    middleware: 'auth'
  })
  
  const authStore = useAuthStore()
  const assetsStore = useAssetsStore()
  const assets = computed(() => assetsStore.assets)
  const searchTerm = ref('')
  
  // Load assets on mount
  onMounted(() => {
    assetsStore.fetchAssets()
  })
  
  // Debounced search
  const debouncedSearch = debounce(async () => {
    await assetsStore.fetchAssets(searchTerm.value)
  }, 300)
  
  const exportAssets = async () => {
    const result = await assetsStore.exportToExcel()
    if (!result.success) {
      alert(result.error)
    }
  }
  
  const deleteAsset = async (id: number) => {
    if (confirm('Are you sure you want to delete this asset?')) {
      const result = await assetsStore.deleteAsset(id)
      if (!result.success) {
        alert(result.error)
      }
    }
  }
  </script>
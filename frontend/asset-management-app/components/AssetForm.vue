<template>
    <div class="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <form @submit.prevent="handleSubmit">
        <div class="space-y-8 divide-y divide-gray-200">
          <div>
            <div>
              <h3 class="text-lg leading-6 font-medium text-gray-900">
                {{ isEdit ? 'Edit Asset' : 'New Asset' }}
              </h3>
              <p class="mt-1 text-sm text-gray-500">
                {{ isEdit ? 'Update the asset information below.' : 'Fill in the information for the new asset.' }}
              </p>
            </div>
  
            <div class="mt-6 grid grid-cols-1 gap-y-6 gap-x-4 sm:grid-cols-6">
              <div class="sm:col-span-3">
                <label for="assetTag" class="block text-sm font-medium text-gray-700">
                  Asset Tag *
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.assetTag"
                    type="text"
                    name="assetTag"
                    id="assetTag"
                    required
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="name" class="block text-sm font-medium text-gray-700">
                  Name *
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.name"
                    type="text"
                    name="name"
                    id="name"
                    required
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-6">
                <label for="description" class="block text-sm font-medium text-gray-700">
                  Description
                </label>
                <div class="mt-1">
                  <textarea
                    v-model="form.description"
                    id="description"
                    name="description"
                    rows="3"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="category" class="block text-sm font-medium text-gray-700">
                  Category *
                </label>
                <div class="mt-1">
                  <select
                    v-model="form.category"
                    id="category"
                    name="category"
                    required
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  >
                    <option value="">Select a category</option>
                    <option value="Laptop">Laptop</option>
                    <option value="Desktop">Desktop</option>
                    <option value="Monitor">Monitor</option>
                    <option value="Printer">Printer</option>
                    <option value="Phone">Phone</option>
                    <option value="Tablet">Tablet</option>
                    <option value="Network">Network Equipment</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="status" class="block text-sm font-medium text-gray-700">
                  Status *
                </label>
                <div class="mt-1">
                  <select
                    v-model="form.status"
                    id="status"
                    name="status"
                    required
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  >
                    <option value="Available">Available</option>
                    <option value="Assigned">Assigned</option>
                    <option value="Maintenance">Maintenance</option>
                    <option value="Retired">Retired</option>
                  </select>
                </div>
              </div>
  
              <div v-if="isEdit" class="sm:col-span-3">
                <label for="assignedToUserId" class="block text-sm font-medium text-gray-700">
                  Assigned To
                </label>
                <div class="mt-1">
                  <select
                    v-model="form.assignedToUserId"
                    id="assignedToUserId"
                    name="assignedToUserId"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  >
                    <option :value="null">Unassigned</option>
                    <option :value="1">System Administrator</option>
                    <option :value="2">John Doe</option>
                  </select>
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="location" class="block text-sm font-medium text-gray-700">
                  Location
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.location"
                    type="text"
                    name="location"
                    id="location"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="serialNumber" class="block text-sm font-medium text-gray-700">
                  Serial Number
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.serialNumber"
                    type="text"
                    name="serialNumber"
                    id="serialNumber"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="model" class="block text-sm font-medium text-gray-700">
                  Model
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.model"
                    type="text"
                    name="model"
                    id="model"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="manufacturer" class="block text-sm font-medium text-gray-700">
                  Manufacturer
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.manufacturer"
                    type="text"
                    name="manufacturer"
                    id="manufacturer"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="purchaseDate" class="block text-sm font-medium text-gray-700">
                  Purchase Date
                </label>
                <div class="mt-1">
                  <input
                    v-model="form.purchaseDate"
                    type="date"
                    name="purchaseDate"
                    id="purchaseDate"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
  
              <div class="sm:col-span-3">
                <label for="purchaseCost" class="block text-sm font-medium text-gray-700">
                  Purchase Cost
                </label>
                <div class="mt-1">
                  <input
                    v-model.number="form.purchaseCost"
                    type="number"
                    step="0.01"
                    name="purchaseCost"
                    id="purchaseCost"
                    class="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
  
        <div v-if="error" class="mt-6 bg-red-50 border border-red-400 text-red-700 px-4 py-3 rounded">
          {{ error }}
        </div>
  
        <div class="mt-6 flex items-center justify-end gap-x-3">
          <NuxtLink
            to="/assets"
            class="text-sm font-semibold leading-6 text-gray-900"
          >
            Cancel
          </NuxtLink>
          <button
            type="submit"
            :disabled="loading"
            class="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:opacity-50"
          >
            {{ loading ? 'Saving...' : (isEdit ? 'Update' : 'Create') }}
          </button>
        </div>
      </form>
    </div>
  </template>
  
  <script setup lang="ts">
  interface Props {
    assetId?: number
  }
  
  const props = defineProps<Props>()
  const router = useRouter()
  const assetsStore = useAssetsStore()
  
  const isEdit = computed(() => !!props.assetId)
  const loading = ref(false)
  const error = ref('')
  
  const form = ref({
    assetTag: '',
    name: '',
    description: '',
    category: '',
    status: 'Available',
    assignedToUserId: null as number | null,
    location: '',
    serialNumber: '',
    model: '',
    manufacturer: '',
    purchaseDate: '',
    purchaseCost: null as number | null
  })
  
  // Load asset data if editing
  onMounted(async () => {
    if (props.assetId) {
      const asset = await assetsStore.fetchAsset(props.assetId)
      if (asset) {
        form.value = {
          assetTag: asset.assetTag,
          name: asset.name,
          description: asset.description || '',
          category: asset.category,
          status: asset.status,
          assignedToUserId: asset.assignedToUserId || null,
          location: asset.location || '',
          serialNumber: asset.serialNumber || '',
          model: asset.model || '',
          manufacturer: asset.manufacturer || '',
          purchaseDate: asset.purchaseDate ? asset.purchaseDate.split('T')[0] : '',
          purchaseCost: asset.purchaseCost || null
        }
      }
    }
  })
  
  const handleSubmit = async () => {
    error.value = ''
    loading.value = true
  
    try {
      let result
      if (isEdit.value) {
        result = await assetsStore.updateAsset(props.assetId!, form.value)
      } else {
        result = await assetsStore.createAsset(form.value)
      }
  
      if (result.success) {
        router.push('/assets')
      } else {
        error.value = result.error
      }
    } catch (e) {
      error.value = 'An unexpected error occurred'
    } finally {
      loading.value = false
    }
  }
  </script>
  
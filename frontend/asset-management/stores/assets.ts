import { useRuntimeConfig } from 'nuxt/app'
import { defineStore } from 'pinia'
import { useAuthStore } from './auth'

interface Asset {
  id: number
  assetTag: string
  name: string
  description?: string
  category: string
  status: string
  purchaseDate?: string
  purchaseCost?: number
  assignedToUserId?: number
  assignedToUserName?: string
  location?: string
  serialNumber?: string
  model?: string
  manufacturer?: string
  createdAt: string
  updatedAt: string
}

interface DashboardData {
  totalAssets: number
  assignedAssets: number
  unassignedAssets: number
  maintenanceAssets: number
  assetsByCategory: Record<string, number>
  assetsByStatus: Record<string, number>
}

export const useAssetsStore = defineStore('assets', {
  state: () => ({
    assets: [] as Asset[],
    currentAsset: null as Asset | null,
    loading: false,
    dashboardData: null as DashboardData | null
  }),

  actions: {
    async fetchAssets(searchTerm?: string) {
      this.loading = true
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const params = searchTerm ? { search: searchTerm } : {}
        const response = await $fetch(`${config.public.apiBase}/assets`, {
          params,
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        this.assets = response as Asset[]
      } catch (error) {
        console.error('Failed to fetch assets:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    
    async fetchAsset(id: number) {
      this.loading = true
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const response = await $fetch(`${config.public.apiBase}/assets/${id}`, {
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        this.currentAsset = response as Asset
        return response
      } catch (error) {
        console.error('Failed to fetch asset:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    
    async createAsset(assetData: Partial<Asset>) {
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const response = await $fetch(`${config.public.apiBase}/assets`, {
          method: 'POST',
          body: assetData,
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        this.assets.unshift(response as Asset)
        return { success: true, data: response }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.data?.message || 'Failed to create asset' 
        }
      }
    },
    
    async updateAsset(id: number, assetData: Partial<Asset>) {
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const response = await $fetch(`${config.public.apiBase}/assets/${id}`, {
          method: 'PUT',
          body: assetData,
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        const index = this.assets.findIndex(a => a.id === id)
        if (index !== -1) {
          this.assets[index] = response as Asset
        }
        return { success: true, data: response }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.data?.message || 'Failed to update asset' 
        }
      }
    },
    
    async deleteAsset(id: number) {
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        await $fetch(`${config.public.apiBase}/assets/${id}`, {
          method: 'DELETE',
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        this.assets = this.assets.filter(a => a.id !== id)
        return { success: true }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.data?.message || 'Failed to delete asset' 
        }
      }
    },
    
    async exportToExcel() {
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const response = await fetch(`${config.public.apiBase}/assets/export`, {
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        
        if (!response.ok) throw new Error('Export failed')
        
        const blob = await response.blob()
        const url = window.URL.createObjectURL(blob)
        const a = document.createElement('a')
        a.href = url
        a.download = `Assets_${new Date().toISOString().split('T')[0]}.xlsx`
        document.body.appendChild(a)
        a.click()
        window.URL.revokeObjectURL(url)
        document.body.removeChild(a)
        
        return { success: true }
      } catch (error: any) {
        return { 
          success: false, 
          error: 'Failed to export assets' 
        }
      }
    },
    
    async fetchDashboardData() {
      const config = useRuntimeConfig()
      const authStore = useAuthStore()
      
      try {
        const response = await $fetch(`${config.public.apiBase}/dashboard`, {
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        })
        this.dashboardData = response as DashboardData
        return response
      } catch (error) {
        console.error('Failed to fetch dashboard data:', error)
        throw error
      }
    }
  }
})
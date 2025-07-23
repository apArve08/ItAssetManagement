
import { defineStore } from 'pinia'
import * as XLSX from 'xlsx'
import { saveAs } from 'file-saver'

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
      const { $axios } = useNuxtApp()
      
      try {
        const params = searchTerm ? { search: searchTerm } : {}
        const response = await $axios.get('/assets', { params })
        this.assets = response.data
      } catch (error) {
        console.error('Failed to fetch assets:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    
    async fetchAsset(id: number) {
      this.loading = true
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.get(`/assets/${id}`)
        this.currentAsset = response.data
        return response.data
      } catch (error) {
        console.error('Failed to fetch asset:', error)
        throw error
      } finally {
        this.loading = false
      }
    },
    
    async createAsset(assetData: Partial<Asset>) {
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.post('/assets', assetData)
        this.assets.unshift(response.data)
        return { success: true, data: response.data }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.response?.data?.message || 'Failed to create asset' 
        }
      }
    },
    
    async updateAsset(id: number, assetData: Partial<Asset>) {
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.put(`/assets/${id}`, assetData)
        const index = this.assets.findIndex(a => a.id === id)
        if (index !== -1) {
          this.assets[index] = response.data
        }
        return { success: true, data: response.data }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.response?.data?.message || 'Failed to update asset' 
        }
      }
    },
    
    async deleteAsset(id: number) {
      const { $axios } = useNuxtApp()
      
      try {
        await $axios.delete(`/assets/${id}`)
        this.assets = this.assets.filter(a => a.id !== id)
        return { success: true }
      } catch (error: any) {
        return { 
          success: false, 
          error: error.response?.data?.message || 'Failed to delete asset' 
        }
      }
    },
    
    async exportToExcel() {
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.get('/assets/export', {
          responseType: 'blob'
        })
        
        const blob = new Blob([response.data], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        })
        
        saveAs(blob, `Assets_${new Date().toISOString().split('T')[0]}.xlsx`)
        return { success: true }
      } catch (error: any) {
        return { 
          success: false, 
          error: 'Failed to export assets' 
        }
      }
    },
    
    async fetchDashboardData() {
      const { $axios } = useNuxtApp()
      
      try {
        const response = await $axios.get('/dashboard')
        this.dashboardData = response.data
        return response.data
      } catch (error) {
        console.error('Failed to fetch dashboard data:', error)
        throw error
      }
    }
  }
})
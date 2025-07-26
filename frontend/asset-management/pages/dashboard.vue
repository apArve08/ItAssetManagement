<template>
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 class="text-2xl font-semibold text-gray-900 mb-8">Dashboard</h1>
      
      <!-- Stats -->
      <div class="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <dt class="text-sm font-medium text-gray-500 truncate">
              Total Assets
            </dt>
            <dd class="mt-1 text-3xl font-semibold text-gray-900">
              {{ dashboardData?.totalAssets || 0 }}
            </dd>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <dt class="text-sm font-medium text-gray-500 truncate">
              Assigned Assets
            </dt>
            <dd class="mt-1 text-3xl font-semibold text-gray-900">
              {{ dashboardData?.assignedAssets || 0 }}
            </dd>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <dt class="text-sm font-medium text-gray-500 truncate">
              Unassigned Assets
            </dt>
            <dd class="mt-1 text-3xl font-semibold text-gray-900">
              {{ dashboardData?.unassignedAssets || 0 }}
            </dd>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <dt class="text-sm font-medium text-gray-500 truncate">
              In Maintenance
            </dt>
            <dd class="mt-1 text-3xl font-semibold text-gray-900">
              {{ dashboardData?.maintenanceAssets || 0 }}
            </dd>
          </div>
        </div>
      </div>
      
      <!-- Charts -->
      <div class="mt-8 grid grid-cols-1 gap-5 lg:grid-cols-2">
        <div class="bg-white shadow rounded-lg p-6">
          <h3 class="text-lg font-medium text-gray-900 mb-4">Assets by Status</h3>
          <canvas ref="statusChart"></canvas>
        </div>
        
        <div class="bg-white shadow rounded-lg p-6">
          <h3 class="text-lg font-medium text-gray-900 mb-4">Assets by Category</h3>
          <canvas ref="categoryChart"></canvas>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { Chart, registerables } from 'chart.js'
import { onMounted, onUnmounted, ref } from 'vue'
  import { useAssetsStore } from '/Users/ap/ItAssetManagement/frontend/asset-management/stores/assets'
  
  
  Chart.register(...registerables)
  
  definePageMeta({
    middleware: 'auth'
  })
  
  const assetsStore = useAssetsStore()
  const statusChart = ref<HTMLCanvasElement>()
  const categoryChart = ref<HTMLCanvasElement>()
  const dashboardData = ref()
  
  let statusChartInstance: Chart | null = null
  let categoryChartInstance: Chart | null = null
  
  onMounted(async () => {
    dashboardData.value = await assetsStore.fetchDashboardData()
    
    // Create status chart
    if (statusChart.value && dashboardData.value?.assetsByStatus) {
      const ctx = statusChart.value.getContext('2d')
      if (ctx) {
        statusChartInstance = new Chart(ctx, {
          type: 'doughnut',
          data: {
            labels: Object.keys(dashboardData.value.assetsByStatus),
            datasets: [{
              data: Object.values(dashboardData.value.assetsByStatus),
              backgroundColor: [
                '#10b981',
                '#3b82f6',
                '#f59e0b',
                '#ef4444'
              ]
            }]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false
          }
        })
      }
    }
    
    // Create category chart
    if (categoryChart.value && dashboardData.value?.assetsByCategory) {
      const ctx = categoryChart.value.getContext('2d')
      if (ctx) {
        categoryChartInstance = new Chart(ctx, {
          type: 'bar',
          data: {
            labels: Object.keys(dashboardData.value.assetsByCategory),
            datasets: [{
              label: 'Number of Assets',
              data: Object.values(dashboardData.value.assetsByCategory),
              backgroundColor: '#3b82f6'
            }]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              y: {
                beginAtZero: true,
                ticks: {
                  stepSize: 1
                }
              }
            }
          }
        })
      }
    }
  })
  
  onUnmounted(() => {
    statusChartInstance?.destroy()
    categoryChartInstance?.destroy()
  })
  

function definePageMeta(arg0: { middleware: string }) {
  throw new Error('Function not implemented.')
}
</script>
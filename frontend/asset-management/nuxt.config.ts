import { defineNuxtConfig } from "nuxt/config";
import tsconfigPaths from 'vite-tsconfig-paths'


export default defineNuxtConfig({
  css: ['~/assets/css/main.css'],
  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {},
    },
  },
  vite: {
    plugins: [
      tsconfigPaths()
    ]
  },
  devtools: { enabled: true },
  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@vueuse/nuxt'
  ],
  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:5000/api'
    }
  },
  ssr: true,
  nitro: {
    preset: 'node-server'
  }
})

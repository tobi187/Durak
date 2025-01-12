// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  modules: [
    '@nuxt/devtools',
    '@nuxt/ui',
  ],
  runtimeConfig: {
    url: 'https://localhost:7175'
  }
})
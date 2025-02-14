// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-11-01",
  devtools: { enabled: true },
  modules: ["@nuxt/devtools", "@nuxt/ui"],
  runtimeConfig: {
    basicAuth: process.env.basic_auth,
    url: process.env.backend_url ?? "https://localhost:7175",
    public: {
      // TODO: change this back
      url: process.env.backend_url ?? "https://localhost:7175",
    },
  },
})

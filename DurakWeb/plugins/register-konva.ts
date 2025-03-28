import VueKonva from "vue-konva"

export default defineNuxtPlugin((nuxtApp) => {
  // Doing something with nuxtApp
  nuxtApp.vueApp.use(VueKonva)
})

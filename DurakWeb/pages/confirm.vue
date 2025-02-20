<script setup lang="ts">
definePageMeta({
  layout: "empty",
})

const user = useSupabaseUser()

// Get redirect path from cookies
const cookieName = useRuntimeConfig().public.supabase.cookieName
const redirectPath = useCookie(`${cookieName}-redirect-path`).value

watch(
  user,
  () => {
    if (user.value) {
      // Clear cookie
      useCookie(`${cookieName}-redirect-path`).value = null
      // Redirect to path
      return navigateTo(redirectPath || "/")
    }
  },
  { immediate: true },
)
</script>
<template>
  <div class="h-screen w-screen flex justify-center align-middle">
    <div class="font-bold text-xl">Waiting for login...</div>
  </div>
</template>

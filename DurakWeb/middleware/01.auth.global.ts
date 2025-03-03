export default defineNuxtRouteMiddleware(async (to) => {
  const auth = useAuth()

  // maybe ignore server
  if (import.meta.server) return

  if (to.path === "/login") {
    return
  }

  if (!auth.userState.user) {
    await auth.fetchMe()
    if (!auth.userState.user) {
      return navigateTo("/login")
    }
  }
})

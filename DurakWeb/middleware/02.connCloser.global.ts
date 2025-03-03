export default defineNuxtRouteMiddleware(async (to) => {
  const game = useGame()

  // maybe ignore server
  if (import.meta.server) return

  if (to.path === "/game" || to.path === "/room") {
    return
  }

  // TODO: maybe (probably) do this differently
  await game.cutConnection()
})

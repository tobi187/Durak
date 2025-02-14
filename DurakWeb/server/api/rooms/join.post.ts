export default defineEventHandler(async (event) => {
  const cfg = useRuntimeConfig()
  const query = getQuery(event)

  try {
    const result = await $fetch<string>(`${cfg.url}/api/room/join`, {
      method: "POST",
      headers: addHeader(),
      query: {
        roomId: query.roomId,
      },
      body: await readBody(event),
    })

    return result
  } catch (ex) {
    console.log(ex)
    return undefined
  }
})

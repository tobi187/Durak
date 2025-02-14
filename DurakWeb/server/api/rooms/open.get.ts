import { Room } from "~/types/api"

export default defineEventHandler(async (event) => {
  const cfg = useRuntimeConfig()

  let rooms = null
  try {
    rooms = await $fetch<Room[]>(`${cfg.url}/api/room/getall`, {
      headers: addHeader(),
    })
  } catch (ex) {
    console.log(ex)
  }

  return rooms ?? []
})

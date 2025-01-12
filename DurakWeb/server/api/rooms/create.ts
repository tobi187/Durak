import { useStore } from "~/composables/useStore"

export default defineEventHandler(async (event) => {
  const cfg = useRuntimeConfig()
  const { userState } = useStore()

  const query = getQuery(event)
  let roomName = query.roomName
  if (roomName === '') {
    roomName = undefined
  }
  
  const result = await $fetch(`${cfg.url}/api/room/create`, {
    query: {
      roomName: roomName
    },
    body: {
      id: userState.userId,
      username: userState.userName
    }
  })
  
  console.log('res', result)

  return result
})

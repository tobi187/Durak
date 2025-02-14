import { User } from "~/types/api"

export default defineEventHandler(async (event) => {
  const cfg = useRuntimeConfig()

  const query = getQuery(event)
  let userName = query.userName?.toString()
  if (userName === "") {
    userName = undefined
  }

  let result = null

  try {
    result = await $fetch<User>(`${cfg.url}/api/user/create`, {
      headers: addHeader(),
      query: {
        userName: userName,
      },
    })
  } catch (ex) {
    console.log("rawr", ex)
  }

  return result
})

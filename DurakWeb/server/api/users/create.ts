export default defineEventHandler(async (event) => {
  const cfg = useRuntimeConfig()

  const query = getQuery(event)
  let userName = query.userName?.toString()
  if (userName === '') {
    userName = undefined
  }
  
  const result = await $fetch(`${cfg.url}/api/user/create`, {
    query: {
      userName: userName
    }
  })
  
  console.log('res', result)

  return result
})

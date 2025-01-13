export default defineEventHandler(async (event) => {
    const cfg = useRuntimeConfig()

    const query = getQuery(event)
    let userName = query.userName?.toString()
    if (userName === '') {
        userName = undefined
    }

    let result = null

    try {
        result = await $fetch(`${cfg.url}/api/user/create`, {
            query: {
                userName: userName,
            },
        })
        // result = await fetch(`${cfg.url}/api/user/create?userName=${userName}`)
    } catch (ex) {
        console.log(ex)
    }

    return result
})

export default defineEventHandler(async (event) => {
    const cfg = useRuntimeConfig()

    debugger

    const query = getQuery(event)
    let roomName = query.roomName
    if (roomName === '') {
        roomName = undefined
    }

    debugger

    let result = null

    try {
        result = await $fetch(`${cfg.url}/api/room/create`, {
            method: 'POST',
            query: {
                roomName: roomName,
            },
            body: await readBody(event),
        })
    } catch (ex) {
        console.log(ex)
    }

    console.log('res', result)

    return result
})

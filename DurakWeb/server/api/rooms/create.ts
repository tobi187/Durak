import { useStore } from '~/composables/useStore'

export default defineEventHandler(async (event) => {
    const cfg = useRuntimeConfig()
    const { userState } = useStore()

    const query = getQuery(event)
    let roomName = query.roomName
    if (roomName === '') {
        roomName = undefined
    }

    console.log({
        query: {
            roomName: roomName,
        },
        body: {
            id: userState.userId,
            username: userState.userName,
        },
    })

    let result = null

    try {
        result = await $fetch(`${cfg.url}/api/room/create`, {
            method: 'POST',
            query: {
                roomName: roomName,
            },
            body: {
                id: userState.userId,
                username: userState.userName,
            },
        })
    } catch (ex) {
        console.log(ex)
    }

    console.log('res', result)

    return result
})

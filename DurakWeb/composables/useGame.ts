import * as signalR from '@microsoft/signalr'
let game = reactive({
    started: false
})

export const useGame = () => {
    const cfg = useRuntimeConfig()
    const store = useStore()

    let connection : signalR.HubConnection | null = null

    const initConnection = async () => {
        connection = 
            new signalR.HubConnectionBuilder()
            .withUrl(`${cfg.url}/durak`)
            .build()
        try {
            await connection.start()

            // await connection.send("joinRoom", store.roomState.id)
        } catch (ex) {
            console.log("Game Hub Connection failed")
            console.log(ex)
        }
    }

    return {
        initConnection
    }
}
import * as signalR from '@microsoft/signalr'
import type { Card, GameState, Me } from '@/types/game'

let game = reactive<{
    state: GameState | undefined
    me: Me | undefined
}>({
    state: undefined,
    me: undefined
})

export const useGame = () => {
    const cfg = useRuntimeConfig()
    const store = useStore()

    let connection : signalR.HubConnection | null = null

    const setCard = async (card: Card) => {
        await connection?.send("AddCard", store.roomState.id, card)
    }

    const beatCard = async (card:Card, cardToBeat:Card) => {
        await connection?.send("SchlagCard", store.roomState.id, card, cardToBeat)
    }

    const pushCard = async (card:Card) => {
        await connection?.send("SchiebCard", store.roomState.id, card)
    }

    const takeCards = async () => {
        await connection?.send("TakeCards", store.roomState.id)
    }

    const initConnection = async () => {
        connection = 
            new signalR.HubConnectionBuilder()
            .withUrl(`${cfg.url}/durak`)
            .build()
        
        try {
            await connection.start()

            connection.on("GameStateChanged", (state: GameState | undefined) => {
                if (!state) {
                    return            
                }
                game.state = state
            })
        
            connection.on("Hand", (me: Me) => {
                if (!me) {
                    return
                }
                game.me = me
            })

            // await connection.send("joinRoom", store.roomState.id)
        } catch (ex) {
            console.log("Game Hub Connection failed")
            console.log(ex)
        }
    }

    const testGetRandomGameState = async () => {
        try {
            const result = await $fetch<GameState>(`${cfg.public.url}/api/testgame/GetRandomGameState`)
            if (result) {
                game.state = result
            } 
        } catch(ex) {
            console.log(ex)
        }
    } 

    const testGetRandomHand = async () => {
        try {
            const result = await $fetch<Me>(`${cfg.public.url}/api/testgame/getrandomplayerhand`)
            if (result) {
                game.me = result
            } 
        } catch(ex) {
            console.log('hand err', ex)
        }
    } 

    return {
        initConnection,
        setCard,
        pushCard,
        beatCard,
        takeCards,
        game,
        testGetRandomGameState,
        testGetRandomHand
    }
}
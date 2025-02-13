import * as signalR from "@microsoft/signalr"
import type { Card, GameState, Me, Player } from "@/types/game"
import { useMechanix } from "./useMechanix"

let game = reactive<{
  state: GameState | undefined
  me: Me | undefined
}>({
  state: undefined,
  me: undefined,
})

let connection: signalR.HubConnection | null = null

export const useGame = () => {
  const cfg = useRuntimeConfig()
  const store = useStore()
  const nuxtApp = useNuxtApp()
  const tempPlayerList = ref<Player[]>([])

  const setCard = async (card: Card) => {
    console.log("Set Card", card)
    await connection?.send("AddCard", {
      roomId: store.roomState.id,
      card: card,
    })
  }

  const beatCard = async (card: Card, cardToBeat: Card) => {
    await connection?.send("SchlagCard", {
      roomId: store.roomState.id,
      card: card,
      cardToBeat: cardToBeat,
    })
  }

  const pushCard = async (card: Card) => {
    await connection?.send("SchiebCard", {
      roomId: store.roomState.id,
      card: card,
    })
  }

  const takeCards = async () => {
    await connection?.send("OnTakeCardsRequested", {
      roomId: store.roomState.id,
    })
  }

  const startGame = async () => {
    console.log("Starrrting")
    await connection?.send("StartGame", {
      roomId: store.roomState.id,
    })
  }

  const createRoom = async () => {
    await connection?.send("CreateRoom", {
      roomId: store.roomState.id,
      userName: store.userState.username,
    })
  }

  const joinRoom = async () => {
    await connection?.send("JoinRoom", {
      roomId: store.roomState.id,
      userName: store.userState.username,
    })
  }

  const endRequested = async () => {
    await connection?.send("OnEndRequested", {
      roomId: store.roomState.id,
    })
  }

  const initConnection = async () => {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${cfg.public.url}/durak`)
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
        console.log(game.me)
      })

      connection.on("TakeRequested", () => {
        if (game.state) {
          game.state!.board.takeRequested = true
        }
      })

      connection.on("UserJoined", (players: Player[]) => {
        tempPlayerList.value = players
      })

      connection.on("StartGame", async (state: GameState | undefined) => {
        if (!state) {
          return
        }
        game.state = state
        await nuxtApp.runWithContext(() => {
          navigateTo("/game")
        })
      })
      // await connection.send("joinRoom", store.roomState.id)
    } catch (ex) {
      console.log("Game Hub Connection failed")
      console.log(ex)
    }
  }

  const testGetRandomGameState = async () => {
    try {
      const result = await $fetch<GameState>(
        `${cfg.public.url}/api/testGame/GetRandomGameState`,
      )
      if (result) {
        game.state = result
      }
    } catch (ex) {
      console.log(ex)
    }
  }

  const testGetRandomHand = async () => {
    try {
      const result = await $fetch<Me>(
        `${cfg.public.url}/api/testGame/getRandomPlayerHand`,
      )
      if (result) {
        game.me = result
      }
    } catch (ex) {
      console.log("hand err", ex)
    }
  }

  return {
    initConnection,
    setCard,
    pushCard,
    beatCard,
    takeCards,
    startGame,
    createRoom,
    joinRoom,
    endRequested,
    game: readonly(game),
    testGetRandomGameState,
    testGetRandomHand,
    tempPlayerList,
  }
}

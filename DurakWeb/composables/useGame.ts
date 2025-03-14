import * as signalR from "@microsoft/signalr"
import type { Card, GameState, Me, Player } from "@/types/game"

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
  const { userState } = useAuth()
  const nuxtApp = useNuxtApp()
  const tempPlayerList = ref<Player[]>([])

  const setCard = async (card: Card) => {
    console.log("Set Card", card)
    await connection?.send("AddCard", {
      roomId: userState.user?.roomId,
      card: card,
    })
  }

  const beatCard = async (card: Card, cardToBeat: Card) => {
    await connection?.send("SchlagCard", {
      roomId: userState.user?.roomId,
      card: card,
      cardToBeat: cardToBeat,
    })
  }

  const pushCard = async (card: Card) => {
    await connection?.send("SchiebCard", {
      roomId: userState.user?.roomId,
      card: card,
    })
  }

  const takeCards = async () => {
    await connection?.send("OnTakeCardsRequested", {
      roomId: userState.user?.roomId,
    })
  }

  const startGame = async () => {
    await connection?.send("StartGame", {
      roomId: userState.user?.roomId,
    })
  }

  const createRoom = async () => {
    await connection?.send("CreateRoom", {
      roomId: userState.user?.roomId,
      userName: userState?.user?.username,
    })
  }

  const joinRoom = async () => {
    await connection?.send("JoinRoom", {
      roomId: userState.user?.roomId,
      userName: userState?.user?.username,
    })
  }

  const endRequested = async () => {
    await connection?.send("OnEndRequested", {
      roomId: userState.user?.roomId,
    })
  }

  const initConnection = async () => {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${cfg.public.url}/durak`)
      .withAutomaticReconnect()
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
    } catch (ex) {
      console.log("Game Hub Connection failed")
      console.log(ex)
    }
  }

  const cutConnection = async () => {
    if (connection !== null) {
      try {
        await connection.stop()
      } catch (ex) {
        // don't care probably
        console.log(ex)
      }
      connection = null
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
    cutConnection,
    game: readonly(game),
    tempPlayerList,
  }
}

import type { Room, User } from "~/types/api"

const userState: User = reactive({
  id: undefined,
  username: undefined,
})

const roomState: Room = reactive({
  id: undefined,
  name: undefined,
  isPlaying: false,
  users: [],
})

export const useStore = () => {
  const updateUser = async () => {
    let userName = undefined
    if (userState.username) {
      userName = userState.username
    }
    try {
      const data = (await $fetch("/api/users/create", {
        query: {
          userName: userName,
        },
      })) as User

      userState.id = data.id
      userState.username = data.username
      return true
    } catch (ex) {
      console.log(ex)
      return false
    }
  }

  const createRoom = async (roomName?: string) => {
    if (!userState.id) {
      const userIsAdded = await updateUser()
      if (!userIsAdded) {
        return false
      }
    }
    try {
      const roomId = await $fetch<string>("/api/rooms/create", {
        method: "POST",
        query: {
          roomName: roomName,
        },
        body: userState,
      })
      roomState.id = roomId
      return true
    } catch (ex) {
      console.log(ex)
      return false
    }
  }

  const joinRoom = async (selRoomId: string) => {
    if (!userState.id) {
      const userIsAdded = await updateUser()
      if (!userIsAdded) {
        return false
      }
    }

    try {
      const roomId = await $fetch<string>("/api/rooms/join", {
        method: "POST",
        query: {
          roomId: selRoomId,
        },
        body: userState,
      })
      roomState.id = roomId
      return true
    } catch (ex) {
      console.log(ex)
      return false
    }
  }

  const setUserName = (userName?: string) => {
    console.log("Setting username", userName)
    if (!userName) {
      return
    }
    userState.username = userName
  }

  return {
    userState,
    roomState: readonly(roomState),
    setUserName,
    updateUser,
    joinRoom,
    createRoom,
  }
}

import type { Room, User } from '~/types/api'

const userState: User = reactive({
    id: undefined,
    username: undefined,
})

const roomState: Room = reactive({
    id: undefined,
    name: undefined,
    isPlaying: false,
    users: []
})

export const useStore = () => {
    const updateUserState = (userId?: string, userName?: string) => {
        userState.id = userId
        userState.username = userName
    }

    const updateRoom = (roomId: string, roomName: string, users: User[] = []) => {
        roomState.id = roomId
        roomState.name = roomName
        roomState.users = users
    }

    return {
        userState,
        roomState,
        updateUserState,
        updateRoom
    }
}

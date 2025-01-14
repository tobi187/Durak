import type { User } from '~/types/api'

const userState: User = reactive({
    id: undefined,
    username: undefined,
})

export const useStore = () => {
    const updateUserState = (userId?: string, userName?: string) => {
        userState.id = userId
        userState.username = userName
    }

    return {
        userState,
        updateUserState,
    }
}

import type { User } from '~/types/api'

export const useStore = () => {
    const userState: User = reactive({
        userId: undefined,
        userName: undefined,
    })

    const updateUserState = (userId?: string, userName?: string) => {
        userState.userId = userId
        userState.userName = userName
    }

    return {
        userState,
        updateUserState,
    }
}

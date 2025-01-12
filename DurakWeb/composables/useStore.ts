import type { User } from "~/types/api"

const userState: User = reactive({
  userId: undefined,
  userName: undefined
})

export const useStore = () => {
  const updateUserState = (userId?: string, userName?: string) => {
    userState.userId = userId
    userState.userName = userName
  } 
  
  return {
    userState,
    updateUserState
  }
}

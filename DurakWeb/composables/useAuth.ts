import type { User } from "~/types/api"

const userState = reactive<{
  user?: User
  isGetting: boolean
}>({
  user: undefined,
  isGetting: false,
})

export const useAuth = () => {
  const { get, post } = useApi()

  const fetchMe = async () => {
    userState.isGetting = true
    const res = await get<User>({
      url: "/api/user/me",
    })

    if (res.isOk()) {
      userState.user = res.value
    }
    userState.isGetting = false
  }

  const login = async (mail: string, pw: string) => {
    const res = await post({
      url: "/login",
      query: { useCookies: true },
      body: {
        email: mail,
        password: pw,
      },
    })

    await fetchMe()

    return res
  }

  const register = async (mail: string, pw: string) => {
    const res = await post({
      url: "/register",
      body: {
        email: mail,
        password: pw,
      },
    })

    return res
  }

  const loginAnonymous = async () => {
    const res = await post({
      url: "/anon",
      query: { useCookies: true },
    })

    await fetchMe()

    return res
  }

  return {
    login,
    loginAnonymous,
    register,
    fetchMe,
    userState: readonly(userState),
  }
}

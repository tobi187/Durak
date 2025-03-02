const user = reactive<{
  name?: string
  isGetting: boolean
}>({
  name: undefined,
  isGetting: false,
})

export const useAuth = () => {
  const { get, post } = useApi()

  const fetchMe = async () => {
    user.isGetting = true
    // change this
    const res = await get<string>({
      url: "/api/users/me",
    })

    if (res.isOk()) {
      user.name = res.value
    }
    user.isGetting = false
  }

  const login = async (mail: string, pw: string) => {
    const res = await post({
      url: "/login",
      body: {
        email: mail,
        password: pw,
      },
    })

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
      url: "/loginAnonymous",
    })

    return res
  }

  onMounted(async () => {
    if (!user.name && !user.isGetting) {
      await fetchMe()
    }
  })

  return {
    login,
    loginAnonymous,
    register,
    userState: readonly(user),
  }
}

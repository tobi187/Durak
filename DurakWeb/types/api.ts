export interface User {
  id?: string
  username?: string
}

export interface Room {
  id?: string
  name?: string
  isPlaying: boolean
  users: User[]
}

export interface IFetchOptions {
  url: string
  query?: Record<string, any>
  body?: Record<string, any>
}

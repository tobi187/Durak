export interface User {
  id: string
  username: string
  isTempUser: boolean
  roomId?: string
}

export interface Room {
  id: string
  name: string
  playerCount: number
}

export default interface Rules {
  playerLimit: number
  minCard: number
  pushAllowed: boolean
  maxBoardCardAmount: number
}

export interface RoomWithRules {
  id: string
  name: string
  isPlaying: boolean
  rules: Rules
}

export interface IFetchOptions {
  url: string
  query?: Record<string, any>
  body?: Record<string, any>
}

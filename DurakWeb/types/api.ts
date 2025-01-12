export interface User {
    userId?: string,
    userName?: string
}

export interface Room {
    id: string,
    name: string,
    users: User[]
}
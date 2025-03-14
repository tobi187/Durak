enum CardValue {
  Six = 6,
  Seven = 7,
  Eight = 8,
  Nine = 9,
  Ten = 10,
  Bube = 11,
  Queen = 12,
  King = 13,
  Ace = 1,
}

enum CardSign {
  Herz = 1,
  Shippe = 2,
  Karo = 3,
  Kreuz = 4,
}

interface Card {
  readonly sign: CardSign
  readonly value: CardValue
}

type CardToBeat = {
  card: Card
  from: string
  beaten?: {
    card: Card
    from: string
  }
}

interface Player {
  id: string
  userName: string
  place?: number
}

interface PlayerWithHandAmount extends Player {
  handAmount: number
}

type GameState = {
  board: {
    locked: boolean
    takeRequested: boolean
    deckCount: number
    trumpf: Card
    cards: CardToBeat[]
  }
  players: {
    turnPlayer: Player
    players: PlayerWithHandAmount[]
  }
}

type Me = {
  info: Player
  hand: Card[]
}

export {
  type Card,
  type GameState,
  type Me,
  type CardToBeat,
  type Player,
  type PlayerWithHandAmount,
  CardSign,
  CardValue,
}

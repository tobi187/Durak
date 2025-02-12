import type { Card, CardToBeat } from "~/types/game"

export const isCardEqual = (firstCard?: Card, secondCard?: Card) => {
  if (!firstCard || !secondCard) {
    return false
  }
  return (
    firstCard?.sign == secondCard?.sign && firstCard?.value == secondCard?.value
  )
}

export const cardToBeatKey = (card: CardToBeat) => {
  const dc = `${card.card.sign}_${card.card.value}`
  if (!card.beaten) {
    return dc
  }

  return `${dc}_${card.beaten.card.sign}_${card.beaten.card.value}`
}

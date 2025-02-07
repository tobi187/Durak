import type { Card } from "~/types/game"

export const isCardEqual = (firstCard?: Card, secondCard?: Card) => {
  if (!firstCard || !secondCard) {
    return false
  }
  return (
    firstCard?.sign == secondCard?.sign && firstCard?.value == secondCard?.value
  )
}

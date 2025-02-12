import type { Card } from "~/types/game"

interface ICardState {
  highlighted?: Card
}

const state: ICardState = reactive({
  highlighted: undefined,
})

export const useMechanix = () => {
  const { beatCard, pushCard, setCard, game } = useGame()

  const setHighlighted = (card: Card) => {
    if (isCardEqual(state.highlighted, card)) {
      state.highlighted = undefined
    } else {
      state.highlighted = card
    }
  }

  const tryBeatCard = async (cardToBeat: Card) => {
    if (!state.highlighted) {
      return
    }
    await beatCard(state.highlighted, cardToBeat)
  }

  const tryPlayCard = async () => {
    if (!state.highlighted) {
      return
    }
    console.log("heiiii")
    if (game.state?.players.turnPlayer.id === game.me?.info.id) {
      await pushCard(state.highlighted)
    } else {
      await setCard(state.highlighted)
    }
  }

  return {
    setHighlighted,
    state: readonly(state),
    tryPlayCard,
    tryBeatCard,
  }
}

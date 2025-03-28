<template>
  <div class="relative">
    <span class="text-9xl">
      <img :src="cardPath" class="scale-75" width="100" height="100" />
    </span>
    <span class="absolute top-12 left-12" v-if="secondPath">
      <img :src="secondPath" width="100" height="100" />
    </span>
    <div
      class="absolute top-6 left-6 w-full h-full"
      v-else-if="state.highlighted"
    >
      <div
        class="w-20 rounded-md h-28 opacity-50 bg-yellow-400 cursor-pointer"
        @click="onDrop"
      ></div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { CardSign, type Card, type CardToBeat } from "~/types/game"
const { state, tryBeatCard } = useMechanix()

const props = defineProps<CardToBeat>()

const onDrop = async () => {
  if (!state.highlighted) {
    return
  }
  await tryBeatCard(props.card)
}

const getCardName = (card?: Card) => {
  if (!card) {
    return undefined
  }
  const cardName =
    `${CardSign[card.sign]}_${card.value}.svg`.toLocaleLowerCase()

  return `/cards/${cardName}`
}

const cardPath = getCardName(props.card)
const secondPath = getCardName(props.beaten?.card)
</script>

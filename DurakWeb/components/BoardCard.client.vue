<template>
  <div class="relative" ref="dropi">
    <span class="text-9xl">
      <img :src="cardPath" class="scale-75" />
    </span>
    <span class="absolute top-12 left-12" v-if="secondPath">
      <img :src="secondPath" />
    </span>
    <span class="absolute top-12 left-12" v-else-if="state.highlighted">
      <span class="w-3/4 h-3/4 opacity-50 bg-yellow-400" @click="onDrop">
      </span>
    </span>
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
    return ""
  }
  const cardName =
    `${CardSign[card.sign]}_${card.value}.svg`.toLocaleLowerCase()

  return `/cards/${cardName}`
}

const cardPath = getCardName(props.card)
const secondPath = getCardName(props.beaten?.card)
</script>

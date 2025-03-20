<template>
  <div class="float-left -m-4" :class="cardClass" @click="onCardClick">
    <img
      height="300"
      width="150"
      :src="cardPath"
      class="hover:scale-125 game-card"
      :class="{ 'scale-125': meHighlighted }"
    />
  </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from "~/types/game"

const { setHighlighted, state } = useMechanix()
const props = defineProps<{
  card: Card
  index: number
  len: number
}>()

const cardClass = computed(() => {
  const centerIndex = Math.round((props.len - 1) / 2)
  const rotation = (props.index - centerIndex) * 7 // Adjust the spread factor (7°)

  let rotate = `rotate-[${rotation}deg] `
  let pad = `pt-[${Math.floor(10 + Math.abs(rotation * 1.4))}px] ` // Base 10px + slight scaling

  return rotate + pad + `z-[${props.index + 1}]`
})

// const cardClass = computed(() => {
//   const centerIndex = Math.round((props.len - 1) / 2)
//   const rotation = (props.index - centerIndex) * 7 // Rotation adjustment

//   // Apply padding only to central cards
//   const distanceFromCenter = Math.abs(props.index - centerIndex)
//   const padAmount = Math.max(0, distanceFromCenter * 10) // Less lift on edges
//   let pad = `pt-[${Math.round(padAmount)}px] `

//   return `rotate-[${rotation}deg] ${pad} z-[${props.index + 1}] -m-4`
// })

const onCardClick = () => {
  setHighlighted(props.card)
}

const meHighlighted = computed(() => isCardEqual(state.highlighted, props.card))

const cardName =
  `${CardSign[props.card.sign]}_${props.card.value}.svg`.toLocaleLowerCase()

const cardPath = `/cards/${cardName}`
</script>

<template>
  <div>
    <span class="text-9xl" @click="onCardClick">
      <img
        :src="cardPath"
        class="hover:scale-125 game-card w-40"
        :class="{ 'scale-125': meHighlighted }"
      />
    </span>
  </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from "~/types/game"

const { setHighlighted, state } = useMechanix()
const props = defineProps<Card>()

const onCardClick = () => {
  setHighlighted(props)
}

const meHighlighted = computed(() => isCardEqual(state.highlighted, props))

const cardName =
  `${CardSign[props.sign]}_${props.value}.svg`.toLocaleLowerCase()

const cardPath = `/cards/${cardName}`
</script>

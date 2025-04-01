<template>
  <div class="p-2">
    <!-- <div class="relative h-48 w-36">
      <img height="150" width="150" class="absolute" :src="cardPath" />
      <img
        height="150"
        width="150"
        class="absolute ml-7 mt-7"
        src="/card_back/blue2.svg"
      />
      <div class="absolute top-1/2 left-1/2 -translate-y-1/2 -translate-x-1/2">
        <span class="border p-1 bg-white text-black">25 Karten</span>
      </div>
    </div> -->
    <div class="relative p-5 m-5">
      <img
        height="100"
        width="100"
        class="absolute"
        :class="{ 'opacity-50': deckCount <= 0 }"
        :src="cardPath"
      />
      <img
        v-if="deckCount > 0"
        height="100"
        width="100"
        class="ml-7 mt-7"
        src="/card_back/blue2.svg"
      />
      <div class="">
        <span class="border p-1 bg-white text-black"
          >{{ deckCount }} Karten</span
        >
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from "~/types/game"
const { game } = useGame()

const props = defineProps<Card>()

const deckCount = computed(() => game?.state?.board.deckCount ?? 52 - 4 * 6)

const cardName =
  `${CardSign[props.sign]}_${props.value}.svg`.toLocaleLowerCase()

const cardPath = `/cards/${cardName}`
</script>

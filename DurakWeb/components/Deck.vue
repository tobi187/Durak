<template>
  <div class="">
    <div class="relative h-48 w-36">
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
    </div>
  </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from "~/types/game"

const props = defineProps<Card>()

const cardName =
  `${CardSign[props.sign]}_${props.value}.svg`.toLocaleLowerCase()

const cardPath = `/cards/${cardName}`
</script>

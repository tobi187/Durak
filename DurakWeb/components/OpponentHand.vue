<template>
  <div>
    <div v-if="flipIt" class="flex flex-col justify-center h-full">
      <div class="p-3">
        <span>{{ player?.userName }}</span>
      </div>
      <div class="flex-col flex">
        <div v-for="index in Math.min(player?.handAmount || 0, 6)" :key="index">
          <img
            class="w-[4vw] rotate-90 float-left -m-6"
            :src="image"
            height="300"
            width="150"
          />
        </div>
      </div>
    </div>
    <div v-else>
      <div class="p-3">
        <span>{{ player?.userName }}</span>
      </div>
      <div class="flex">
        <div v-for="index in Math.min(player?.handAmount ?? 0, 6)" :key="index">
          <img :src="image" height="300" width="150" class="float-left -m-8" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const { game } = useGame()

const props = defineProps({
  playerId: {
    type: String,
    required: true,
  },
  flipIt: {
    type: Boolean,
    required: false,
  },
})

const player = computed(() => {
  return game.state?.players.players.find((el) => el.id === props.playerId)
})

const image = `/card_back/blue.svg`
</script>

<style></style>

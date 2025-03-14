<template>
  <div class="flex">
    <div class="flex align-middle p-3 h-1/2">
      <div v-if="game.me?.info.id === game.state?.players.turnPlayer.id">
        <UButton @click="onTakeCardClick">Nehmen</UButton>
      </div>
      <div v-else>
        <UButton @click="onEndClick">Ende</UButton>
      </div>
    </div>
    <div class="pt-1"></div>
    <div class="pt-2"></div>
    <div class="pt-3"></div>
    <div class="pt-4"></div>
    <div class="pt-5"></div>
    <div class="pt-6"></div>
    <div class="pt-7"></div>
    <div class="pt-8"></div>
    <div class="pt-9"></div>
    <div class="w-1/2">
      <div v-if="game?.me?.hand" class="flex px-20 w-full">
        <!-- 6, -3+0*2 -->
        <Card
          class="float-left -m-4"
          v-for="(card, ind) in game?.me?.hand"
          v-bind="card"
          :class="`rotate-[${(-(game.me.hand.length / 2) + ind) * 2}deg] pt-${Math.abs(game.me.hand.length / 2 - ind)} z-${ind + 1}`"
          :key="`${card.sign}_${card.value}`"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const { game, takeCards, endRequested } = useGame()

const onTakeCardClick = async () => {
  await takeCards()
}

const onEndClick = async () => {
  await endRequested()
}
</script>

<style></style>

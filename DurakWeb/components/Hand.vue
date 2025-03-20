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
    <div class="w-1/2">
      <div v-if="game?.me?.hand" class="flex px-20 w-full">
        <Card
          v-for="(card, ind) in game?.me?.hand"
          :card="card"
          :index="ind"
          :len="game?.me?.hand.length"
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

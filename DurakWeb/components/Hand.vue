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
    <div v-if="game?.me?.hand" class="flex relative gap-6">
      <Card
        v-for="card in game?.me?.hand"
        v-bind="card"
        :key="`${card.sign}_${card.value}`"
      />
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

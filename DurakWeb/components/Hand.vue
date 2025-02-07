<template>
  <div v-if="game?.me?.hand" class="flex gap-x-8">
    <Card
      v-for="card in game?.me?.hand"
      v-bind="card"
      :key="`${card.sign}_${card.value}`"
    />
  </div>
  <button @click="loggi">bluthund</button>
</template>

<script lang="ts" setup>
const { game } = useGame()

const loggi = () => {
  console.log(game.me)
}
</script>

<style></style>

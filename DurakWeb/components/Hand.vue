<template>
  <div v-if="game?.me?.hand" class="flex gap-x-8">
    <Card v-for="(card, key) in game?.me?.hand" v-bind="card" :key="key" />
  </div>
</template>

<script lang="ts" setup>
const { game } = useGame()

</script>

<style></style>
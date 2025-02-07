<template>
  <div class="flex">
    <div class="w-3/4 grid grid-cols-3 grid-rows-2 gap-3">
      <div v-for="card in game.state?.board.cards">
        <BoardCard v-bind="card" />
      </div>
      <div v-if="game.state?.board.cards ?? 7 < 6">
        <div
          class="w-full h-full bg-yellow-400 opacity-50"
          @v-if="state.highlighted"
          @click="onClick"
        ></div>
      </div>
    </div>
    <div class="w-1/4">
      <span class="border">deck: {{ game.state?.board.deckCount }}</span>
      <Card v-if="game.state?.board.trumpf" v-bind="game.state?.board.trumpf" />
      <div class="dropper" v-if="!game.state?.board.locked">
        <div class="w-10 h-28"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const { game } = useGame()
const { state, tryPlayCard } = useMechanix()

const onClick = async () => {
  if (!state.highlighted) {
    return
  }
  await tryPlayCard()
}
</script>

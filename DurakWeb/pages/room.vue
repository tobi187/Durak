<template>
  <div>
    <div>RoomId: {{ roomState.id }}</div>
    <div>UserId: {{ userState.id }}</div>
    <div>RoomId: {{ userState.username }}</div>
  </div>
</template>

<script lang="ts" setup>
const { roomState, userState } = useStore()
const { initConnection } = useGame()
if (!roomState.id) {
  // if no game id go back to home
  navigateTo('/')
}

await initConnection()
</script>

<style></style>
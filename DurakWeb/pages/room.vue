<template>
  <div>
    <div>RoomId: {{ roomState.id }}</div>
    <div>UserId: {{ userState.id }}</div>
    <div>Username: {{ userState.username }}</div>
  </div>
  <ul v-for="player in tempPlayerList">
    <li>{{ player.userName }}</li>
  </ul>
  <div>
    <UButton @click="onStartGame">Starten</UButton>
  </div>
</template>

<script lang="ts" setup>
const { roomState, userState } = useStore()
const { initConnection, startGame, joinRoom, createRoom, tempPlayerList } =
  useGame()
const route = useRoute()

const onStartGame = async () => {
  console.log("asd")
  if (tempPlayerList.value.length < 2) {
    return
  }
  console.log(tempPlayerList.value.length)
  await startGame()
}

;(async () => {
  if (!roomState.id) {
    // if no game id go back to home
    await navigateTo("/")
  }
  
  await initConnection()
  if (route.query.creator === "true") {
    await createRoom()
  } else {
    await joinRoom()
  }
})()
</script>

<style></style>

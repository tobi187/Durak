<template>
  <div class="text-center">
    <div class="p-3">
      <h1 class="text-xl" v-if="loading">Loading...</h1>
      <h1 class="text-xl" v-else>Room {{ room?.name }}</h1>
    </div>
    <div class="p-3">
      <ul v-for="player in tempPlayerList">
        <li class="p-1 text-lg text-gray-600">{{ player.userName }}</li>
      </ul>
    </div>
    <div v-if="route.query.creator === 'true'">
      <UButton @click="onStartGame">Starten</UButton>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { type RoomWithRules } from "~/types/api"

const { fetchMe, userState } = useAuth()
const { get } = useApi()
const { initConnection, startGame, joinRoom, createRoom, tempPlayerList } =
  useGame()

const route = useRoute()
const loading = ref(false)
const room = ref<RoomWithRules>()

const onStartGame = async () => {
  if (tempPlayerList.value.length < 2) {
    return
  }

  await startGame()
}

const onStart = async () => {
  loading.value = true
  await fetchMe()
  if (!userState.user?.roomId) {
    // if no game id go back to home
    await navigateTo("/")
  }

  await initConnection()
  if (route.query.creator === "true") {
    await createRoom()
  } else {
    await joinRoom()
  }

  const res = await get<RoomWithRules>({
    url: "/api/room/getRoom",
    body: { roomId: userState.user?.roomId },
  })

  if (res.isErr()) {
    return
  }

  room.value = res.value

  loading.value = false
}

onMounted(async () => {
  await onStart()
})
</script>

<style></style>

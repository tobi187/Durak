<template>
  <div>
    <div
      class="flex px-3 justify-between py-3.5 border-b border-gray-200 dark:border-gray-700"
    >
      <UInput v-model="q" placeholder="Filter people..." />
      <UButton icon="i-heroicons-arrow-path" @click="updateRooms" />
    </div>
    <UTable :rows="filteredRows" :columns="columns">
      <template #actions-data="{ row }">
        <UButton @click="() => onTryJoinRoom(row.id)">Join</UButton>
      </template>
    </UTable>
  </div>
</template>

<script lang="ts" setup>
import type { Room } from "~/types/api"

const { joinRoom } = useStore()
const { get } = useApi()

interface RoomInfo {
  id?: number
  name?: string
  amount?: string
  status?: string
}

const q = ref("")
const timer = ref()

const columns = ref([
  {
    key: "name",
    label: "Name",
  },
  {
    key: "amount",
    label: "Players",
  },
  {
    key: "status",
    label: "Status",
  },
  {
    key: "actions",
    class: "text-end",
    rowClass: "text-end",
  },
])

const vals = ref<RoomInfo[]>([])

const updateRooms = async () => {
  const res = await get<Room[]>({
    url: "/api/room/getall",
  })
  if (res.isErr()) {
    return
  }
  vals.value = res.value.map((el) => ({
    id: el.id,
    name: el.name,
    amount: `??? / 4`,
    status: el.isPlaying ? "Already playing" : "Waiting for you :D",
  })) as RoomInfo[]
}

const onTryJoinRoom = async (id?: string) => {
  if (!id) {
    return
  }
  const result = await joinRoom(id)
  if (!result) {
    // TODO: Show Error maybe
    console.log("join room failed", result)
    return
  }
  return await navigateTo("/room")
}

const filteredRows = computed(() => {
  if (!q.value) {
    return vals.value
  }

  const lowerSearch = q.value.toLowerCase()

  return vals.value.filter((row) => {
    return row.name?.toLowerCase().includes(lowerSearch)
  })
})

onMounted(async () => {
  if (timer.value) {
    return
  }
  await updateRooms()
  // timer.value = setInterval(async () => {
  //   await updateRooms()
  // }, 1000 * 5);
})

onBeforeUnmount(() => {
  clearInterval(timer.value)
  timer.value = null
})
</script>

<style></style>

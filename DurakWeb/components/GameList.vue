<template>
  <div>
    <div class="flex px-3 justify-between py-3.5 border-b border-gray-200 dark:border-gray-700">
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
const { joinRoom } = useStore()

interface RoomInfo {
  id?: string,
  name?: string,
  amount?: string
  status?: string
}

const q = ref('')
const timer = ref()

const columns = ref([{
  key: 'name',
  label: 'Name'
}, {
  key: 'amount',
  label: 'Players'
}, {
  key: 'status',
  label: 'Status'
}, {
  key: 'actions',
  class: 'text-end',
  rowClass: 'text-end'
}])

const vals = ref<RoomInfo[]>([])

const updateRooms = async () => {
  try {
    const data = await $fetch('/api/rooms/open')
    if (!data) {
      return
    }

    vals.value = data.map(el => ({
      id: el.id,
      name: el.name,
      status: el.isPlaying ? 'Already playing' : 'Waiting for you :D',
      amount: `${el.users.length} / 4`
    }))
  } catch (ex) {
    console.log(ex)
  }
}

const onTryJoinRoom = async (id?: string) => {
  if (!id) {
    return
  }
  const result = await joinRoom(id, undefined)
  if (!result) {
    // TODO: Show Error maybe
    console.log('join room failed', result)
    return
  }
  await navigateTo("/game")
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
  timer.value = setInterval(async () => {
    await updateRooms()
  }, 1000 * 5);
})

onBeforeUnmount(() => {
  clearInterval(timer.value)
  timer.value = null
})
</script>

<style></style>
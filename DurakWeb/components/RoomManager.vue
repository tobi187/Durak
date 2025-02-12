<template>
  <div>
    <div class="flex justify-center py-8">
      <div class="w-1/4">
        <UFormGroup size="xl" label="Username">
          <UInput icon="i-heroicons-user" v-model="userState.username" />
        </UFormGroup>
      </div>
    </div>
    <div class="flex justify-center">
      <div class="py-4 flex gap-4">
        <UInput v-model="roomName" />
        <UButton @click="onTryCreateRoom">Create Room</UButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const { userState, createRoom } = useStore()

const roomName = ref("")

const onTryCreateRoom = async () => {
  const result = await createRoom(roomName.value)
  if (!result) {
    // TODO: show some Error probably
    return
  }
  await navigateTo({
    path: "/room",
    query: {
      creator: "true",
    },
  })
}
</script>

<style></style>

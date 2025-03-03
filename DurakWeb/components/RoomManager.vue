<template>
  <div>
    <div class="flex justify-center py-8">
      <div class="w-1/4">
        <UFormGroup size="xl" label="Username">
          <UInput
            :loading="isLoadingNameChange"
            icon="i-heroicons-user"
            v-model="userName"
          />
        </UFormGroup>
        <div class="p-1 text-center">
          <UButton @click="onChangeUsername">Change</UButton>
        </div>
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
const { userState, fetchMe } = useAuth()
const { post } = useApi()
const toast = useToast()

const roomName = ref("")
const userName = ref(userState.user?.username)
const isLoadingNameChange = ref(false)

watch(
  () => userState.user?.username,
  (nv) => (userName.value = nv ?? ""),
)

const onChangeUsername = async () => {
  isLoadingNameChange.value = true
  const res = await post({
    url: "/api/user/rename",
    body: { username: userName.value },
  })
  if (res.isOk()) {
    await fetchMe()
    toast.add({
      title: "Name changed successfully",
      icon: "i-heroicons-check-circle",
    })
  }
  isLoadingNameChange.value = false
}

const onTryCreateRoom = async () => {
  const res = await post<string>({
    url: "/api/room/create",
    body: { roomName: roomName.value },
  })

  if (res.isErr()) {
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

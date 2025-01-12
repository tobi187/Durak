<template>
  <div class="p-3">
    <div class="text-center">
      <h1 class="text-3xl font-bold">Durak spielen</h1>
    </div>
    <div class="flex justify-center py-8">
      <div class="w-1/4">
        <UFormGroup size="xl" label="Username">
          <UInput icon="i-heroicons-user" v-model="userName" />
        </UFormGroup>
      </div>
    </div>
    <div class="flex justify-center">
      <div class="py-4 flex gap-4">
        <UInput v-model="roomName" />
        <UButton @click="addUser">Create Room</UButton>
      </div>
    </div>
    <div>
      <div class="">
        <GameList />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { User } from '~/types/api'
const { updateUserState, userState } = useStore()

const userName = ref('')
const roomName = ref('')


const addUser = async () => {
  const user = await useFetch('/api/users/create', {
      query: {
        userName: userName.value
      }
    }
  )
  if (user.error) {
    return false
  }
  const userData = user.data.value as User
  updateUserState(userData.userId, userData.userName)
  return true
}

const createRoom = async () => {
  if (!userState.userId) {
    if (!await addUser()) {
      return
    }
  }
  const room = useFetch('/api/rooms/create', {
    
  })
}

</script>

<style></style>
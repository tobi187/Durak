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
        <UButton @click="createRoom">Create Room</UButton>
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
  const { data, error } = await useFetch('/api/users/create', {
    query: {
      userName: userName.value
    }
  }
  )
  if (error.value) {
    console.log('usererr', error)
    return false
  }
  console.log('userval', data)
  const userData = data.value as User
  console.log(userData.id)
  updateUserState(userData.id, userData.username)
  return true
}

const createRoom = async () => {
  if (!userState.id) {
    const userIsAdded = await addUser()
    if (!userIsAdded) {
      return
    }
  }

  console.log('user', userState)

  const { data, error } = useFetch('/api/rooms/create', {
    query: {
      roomName: roomName.value
    },
    body: userState
  })

  if (error.value) {
    console.log('roomerr', error)
    return
  }
  console.log('roomval', data.value)
  alert(data.value)
}

</script>

<style></style>
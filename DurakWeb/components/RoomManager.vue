<template>
  <div>
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
  </div>
</template>

<script lang="ts" setup>
import type { User } from '~/types/api'
const { updateUserState, userState } = useStore()

const userName = ref('')
const roomName = ref('')
watch(userState, () => userName.value = userState.username ?? '')

const addUser = async () => {
  try {
    const data = await $fetch('/api/users/create', {
      query: {
        userName: userName.value
      }
    }) as User
  
    updateUserState(data.id, data.username)
    return true
  } catch (ex) {
    console.log(ex)
    return false
  }
}

const createRoom = async () => {
  if (!userState.id) {
    const userIsAdded = await addUser()
    if (!userIsAdded) {
      return
    }
  }

  console.log('user', userState)

  const { data, error } = await useFetch('/api/rooms/create', {
    method: 'POST',
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

<style>

</style>
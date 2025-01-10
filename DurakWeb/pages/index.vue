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
    <div>
      <div class="">
        <GameList />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const userName = ref('')

</script>

<style></style>
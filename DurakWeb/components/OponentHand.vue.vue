<template>
  <div>
    <div v-for="index in props.handCardAmount" :key="index">
      <span class="text-9xl p-3">&#127136</span>
    </div>
  </div>
</template>

<script lang="ts" setup>
const props = defineProps<{
  handCardAmount: number
}>()
</script>

<style>

</style>
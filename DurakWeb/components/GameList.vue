<template>
  <div>
    <div class="flex px-3 py-3.5 border-b border-gray-200 dark:border-gray-700">
      <UInput v-model="q" placeholder="Filter people..." />
    </div>
    <UTable :rows="vals" :columns="columns">
      <template #actions-data="{ row }">
        <UButton>Join</UButton>
      </template>
    </UTable>
  </div>
</template>

<script lang="ts" setup>
const q = ref('')

const columns = [{
  key: 'name',
  label: 'Name'
}, {
  key: 'amount',
  label: 'Amount'
}, {
  key: 'actions'
}]

const vals = [
  {
    name: 'Loosi',
    amount: '1/4',
  }
]
</script>

<style></style>
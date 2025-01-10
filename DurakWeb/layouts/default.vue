<template>
  <div>
    <div class="p-1 px-8 border-b border-gray-200 dark:border-gray-800">
      <UHorizontalNavigation :links="links" />
    </div>
    <div class="container mx-auto px-20">
      <slot />
    </div>
  </div>
</template>

<script lang="ts" setup>

const links = [{
  label: 'Durak',
  labelClass: 'text-lg',
  icon: 'i-heroicons-home',
  to: '/'
},
{
  label: 'Android App',
  labelClass: 'text-lg',
  icon: 'i-heroicons-device-phone-mobile',
  to: '/not-implemented'
}
]
</script>

<style></style>
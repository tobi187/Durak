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
const colorMode = useColorMode();

const isDark = computed({
  get() {
    return colorMode.value === "dark";
  },
  set() {
    colorMode.preference = colorMode.value === "dark" ? "light" : "dark";
  },
});

const links = computed(() => [
  [
    {
      label: "Durak",
      labelClass: "text-lg",
      icon: "i-heroicons-home",
      to: "/",
    },
    {
      label: "Android App",
      labelClass: "text-lg",
      icon: "i-heroicons-device-phone-mobile",
      to: "/not-implemented",
    },
    {
      label: "Gaaaaming Maan",
      labelClass: "text-lg",
      icon: "i-heroicons-beaker",
      to: "/game",
    },
  ],
  [
    {
      label: "",
      icon: isDark.value
        ? "i-heroicons-sun-20-solid"
        : "i-heroicons-moon-20-solid",
      click: () => (isDark.value = !isDark.value),
    },
  ],
]);
</script>

<style></style>

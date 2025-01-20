<template>
    <div>
        <span class="text-9xl" v-html="combined"></span>
    </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from '~/types/game';

const props = defineProps<Card>()

const startNums: { [key in CardSign]: number } = {
    [CardSign.Shippe]: 127136,
    [CardSign.Herz]: 127152,
    [CardSign.Karo]: 127168,
    [CardSign.Kreuz]: 127184
}

const sign = startNums[props.sign]

const combined = ref(`&#${sign + props.value};`)
</script>
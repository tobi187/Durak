<template>
    <div>
        <span class="text-9xl hover:scale-125">
            <img :src="cardPath" />
        </span>
    </div>
</template>

<script setup lang="ts">
import { CardSign, type Card } from '~/types/game';

const props = defineProps<Card>()

const cardName = `${CardSign[props.sign]}_${props.value}.svg`.toLocaleLowerCase()

const cardPath = `/cards/${cardName}`
</script>
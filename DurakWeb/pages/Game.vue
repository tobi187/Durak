<template>
    <div class="h-[85vdh]">
        <div class="w-full flex p-3 px-20 border justify-evenly">
            <UButton @click="testGetRandomGameState">Gen State</UButton>
            <UButton @click="testGetRandomHand">Gen Hand</UButton>
        </div>
        <div class="w-full flex justify-center">
            <div>
                Turnplayer: {{ game?.state?.players?.turnPlayer?.userName ?? 'Loading...' }}
            </div>
        </div>
        <div class="grid grid-cols-4 grid-rows-4">
            <div class="col-start-2 col-span-2">
                <Hand />
            </div>
            <div class="col-start-1 row-start-1 row-span-4 flex align-middle">
                <OpponentHand v-if="isOppThere(1)" :playerId="getOpp(1)" :flipIt="true" />
            </div>
            <div class="col-span-2 row-span-2">
                <Board />
            </div>
            <div class="col-start-4 row-span-4 row-start-1 align-middle flex justify-end">
                <OpponentHand v-if="isOppThere(2)" :playerId="getOpp(2)" :flipIt="true" />
            </div>
            <div class="col-start-2 col-span-2">
                <OpponentHand v-if="isOppThere(0)" :playerId="getOpp(0)" />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
const { testGetRandomGameState, testGetRandomHand, game } = useGame()

const ops = computed(() => {
    const myId = game?.me?.info?.id
    if (!myId) {
        return undefined
    }
    return game?.state?.players?.players?.filter(pl => pl.id !== myId)
})

const isOppThere = (i: number) => {
    return ops.value && ops.value[i]
}

const getOpp = (i: number) => {
    if (!ops.value) return ''
    if (!ops.value[i]) return ''
    if (!ops.value[i].id) return ''
    return ops.value[i].id
}

</script>
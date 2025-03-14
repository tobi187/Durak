<template>
  <div class="h-[85vdh]">
    <div class="w-full flex justify-center">
      <div class="font-bold text-lg">
        {{ statusMessage }}
      </div>
    </div>
    <div class="grid grid-cols-4 grid-rows-4">
      <div class="col-span-4 flex justify-center">
        <OpponentHand v-if="isOppThere(0)" :playerId="getOpp(0)" />
      </div>
      <div class="col-start-1 row-start-1 row-span-4 flex align-middle">
        <OpponentHand
          v-if="isOppThere(1)"
          :playerId="getOpp(1)"
          :flipIt="true"
        />
      </div>
      <div class="col-span-2 row-span-2">
        <Board />
      </div>
      <div
        class="col-start-4 row-span-4 row-start-1 align-middle flex justify-end"
      >
        <OpponentHand
          v-if="isOppThere(2)"
          :playerId="getOpp(2)"
          :flipIt="true"
        />
      </div>
      <div class="col-span-4">
        <Hand />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const { game } = useGame()

const timeLeft = ref(11)
const statusMessage = ref("Loading...")

watch(
  () => game.state?.board.takeRequested,
  (isTakeRequested) => {
    if (isTakeRequested && timeLeft.value === 11) {
      const timer = setInterval(() => {
        if (timeLeft.value <= 0) {
          timeLeft.value = 11
          clearInterval(timer)
        }
        timeLeft.value -= 1
      }, 1000)
    }
  },
)

const ops = computed(() => {
  const myId = game?.me?.info?.id
  if (!myId) {
    return undefined
  }
  return game?.state?.players?.players?.filter((pl) => pl.id !== myId)
})

const isOppThere = (i: number) => {
  return ops.value && ops.value[i]
}

const getOpp = (i: number) => {
  if (!ops.value) return ""
  if (!ops.value[i]) return ""
  if (!ops.value[i].id) return ""
  return ops.value[i].id
}

watchEffect(() => {
  const turnPlayer = game.state?.players.turnPlayer.userName
  const isLocked = game.state?.board.locked
  const takeReq = game.state?.board.takeRequested
  let message = `Current Player: ${turnPlayer}`
  if (isLocked) {
    message += ` is am Schlagen`
  } else if (takeReq) {
    message += ` will nehmen. Du hast ${timeLeft.value} Sekunden restliche Karten zu legen`
  }
  statusMessage.value = message
})

// const statusMessage = computed(() => {
// if (!game.state) {
//   return "Loading..."
// }
// const turnPlayer = game.state?.players.turnPlayer.userName
// const isLocked = game.state?.board.locked
// const takeReq = game.state?.board.takeRequested
// const message = `Current Player: ${turnPlayer}`
// if (isLocked) {
//   return message + ` is am Schlagen`
// }
// if (takeReq) {
//   return (
//     message +
//     ` will nehmen. Du hast ${timeLeft.value} Sekunden restliche Karten zu legen`
//   )
// }
// return message
// })
</script>

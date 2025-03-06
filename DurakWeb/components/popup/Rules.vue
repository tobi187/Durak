<template>
  <UModal>
    <UCard>
      <template #header>
        <h3>{{ room?.name }}</h3>
      </template>

      <div>
        <div>
          <UFormGroup label="Spieler Limit">
            <UInput readonly :model-value="room?.rules.playerLimit" />
          </UFormGroup>
        </div>
        <div>
          <UFormGroup label="Schieben">
            <UInput
              readonly
              :model-value="room?.rules.pushAllowed.toString()"
            />
          </UFormGroup>
        </div>
        <div>
          <UFormGroup label="Niedrigste Karte">
            <UInput readonly :model-value="room?.rules.minCard" />
          </UFormGroup>
        </div>
        <div>
          <UFormGroup label="Max. Menge an Bordkarten">
            <UInput readonly :model-value="room?.rules.maxBoardCardAmount" />
          </UFormGroup>
        </div>
      </div>

      <!-- <template #footer>
        <Placeholder class="h-8" />
      </template> -->
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import type { RoomWithRules } from "~/types/api"

const { get } = useApi()

const props = defineProps<{
  roomId: string
}>()

const room = ref<RoomWithRules>()

onMounted(async () => {
  const res = await get<RoomWithRules>({
    url: "/api/room/getRoom",
    query: { roomId: props.roomId },
  })
  if (res.isOk()) {
    room.value = res.value
  }
})
</script>

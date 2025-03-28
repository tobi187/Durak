<template>
  <v-stage :config="conf">
    <v-layer ref="gameLayer">
      <!-- left player -->
      <v-group
        :config="{
          width: conf.width / 10,
          height: conf.height / 2,
          y: conf.height / 4,
          x: 20,
        }"
      >
      </v-group>
      <!-- right player -->
      <v-group
        :config="{
          width: conf.width / 10,
          height: conf.height / 2,
          y: conf.height / 4,
          x: conf.width - (conf.width / 10 + 10),
        }"
      ></v-group>
      <!-- up player -->
      <v-group
        :config="{
          width: conf.width / 2,
          height: conf.height / 8,
          y: 10,
          x: conf.width / 4,
        }"
      ></v-group>
      <!-- down (me) player -->
      <v-group
        ref="myhand"
        :config="{
          width: conf.width / 2,
          height: conf.height / 8,
          y: conf.height - conf.height / 4,
          x: conf.width / 4,
        }"
      >
      </v-group>
      <!-- Board -->
      <v-group
        :config="{
          width: conf.width / 2,
          height: conf.height / 8 - 10,
          y: conf.height / 4 + 10,
          x: conf.width / 4,
        }"
      >
      </v-group>
    </v-layer>
  </v-stage>
</template>

<script setup lang="ts">
import Konva from "konva"
import type { Group } from "konva/lib/Group"
import type { Layer } from "konva/lib/Layer"
import type { VueKonvaProxy } from "~/types/konvaTypes"

definePageMeta({
  layout: "full",
})

const conf = ref({ width: window.innerWidth, height: window.innerHeight - 100 })

type P = { sx: number; sy: number }

const gameLayer = useTemplateRef<VueKonvaProxy<Group>>("myhand")
let layer: Group | undefined = undefined
const imagePos = ref<P[]>([])

const cardDefaultsSize = 150

onMounted(() => {
  if (gameLayer !== null) {
    layer = gameLayer.value?.getNode()
  }

  for (let i = 0; i < 6; i++) {
    const img = new Image()
    img.onload = function () {
      // aspect ratio 0.7:1 w:h
      const myimg = new Konva.Image({
        image: img,
        x: i * (cardDefaultsSize / 2),
        y: 0,
        width: cardDefaultsSize * 0.7,
        height: cardDefaultsSize,
        draggable: !false,
      })
      layer?.add(myimg)
      imagePos.value.push({ sx: i * cardDefaultsSize, sy: cardDefaultsSize })
      myimg.zIndex(i)
      myimg.on("dragend", () => {
        myimg.x(i * (cardDefaultsSize / 2))
        myimg.y(0)
      })
    }
    img.src = "/cards/herz_" + (i + 1) + ".svg"
  }
})
</script>

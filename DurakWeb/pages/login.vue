<script setup lang="ts">
import type { Result } from "neverthrow"

const auth = useAuth()

definePageMeta({
  layout: "empty",
})

const email = ref("")
const pw = ref("")
const pwWdh = ref("")
const errorMessage = ref("")

const checkEmilPw = () => {
  if (!/^[^@]+@[^@]+\.[^@]+$/.test(email.value)) {
    errorMessage.value = "Check Email dikker"
    return false
  }
  if (pw.value.length < 1) {
    errorMessage.value = "Mach Pw besser"
    return false
  }
  return true
}

const signIn = async () => {
  if (!checkEmilPw()) {
    return
  }
  const res = await auth.login(email.value, pw.value)

  await handleAuthResult(res)
}

const signInAnonymously = async () => {
  const res = await auth.loginAnonymous()

  await handleAuthResult(res)
}

const signUpWithMail = async () => {
  if (!checkEmilPw()) {
    return
  }
  if (pw.value !== pwWdh.value) {
    errorMessage.value = "Pls check your Passwords are same"
    return
  }
  const res = await auth.register(email.value, pw.value)

  if (res.isErr()) {
    return
  }
  const lgRes = await auth.login(email.value, pw.value)

  await handleAuthResult(lgRes)
}

const handleAuthResult = async (result: Result<unknown, any>) => {
  if (result.isErr()) {
    errorMessage.value = "Something went wrong. Please try again"
    return
  }
  await navigateTo("/")
}

const passwordReset = () => {
  if (!email.value) {
    errorMessage.value = "Bitte geben sie ihne Email ein"
    return
  }
  errorMessage.value = "Haha verarscht. Ist noch nicht eingebaut"
}
</script>

<template>
  <div class="lg:p-10 lg:px-20 px-5 pb-5">
    <div class="text-center p-8">
      <h1 class="text-3xl font-bold">Welcome to the World of Durak</h1>
    </div>
    <div>
      <div class="flex flex-col lg:flex-row">
        <div class="text-center lg:w-1/2 lg:p-10">
          <UFormGroup class="p-3" label="Email" size="lg">
            <UInput v-model="email" type="email" icon="i-heroicons-envelope" />
          </UFormGroup>
          <UFormGroup class="p-3" label="Password" size="lg">
            <UInput
              v-model="pw"
              type="password"
              icon="i-heroicons-lock-closed"
            />
          </UFormGroup>
          <ULink class="p-3" as="button" @click="passwordReset"
            >Passwort zurücksetzen</ULink
          >
        </div>
        <div
          class="text-center lg:w-1/2 lg:p-10 lg:border-l-2 lg:border-t-0 border-white border-t-2"
        >
          <UFormGroup class="p-3" label="Email" size="lg">
            <UInput v-model="email" type="email" icon="i-heroicons-envelope" />
          </UFormGroup>
          <UFormGroup class="p-3" label="Password" size="lg">
            <UInput
              v-model="pw"
              type="password"
              icon="i-heroicons-lock-closed"
            />
          </UFormGroup>
          <UFormGroup class="p-3" label="Password wiederholen" size="lg">
            <UInput
              v-model="pwWdh"
              type="password"
              icon="i-heroicons-lock-closed"
            />
          </UFormGroup>
        </div>
      </div>
    </div>
    <div class="p-6 text-center">
      <p class="text-red-600">&nbsp;{{ errorMessage }}</p>
    </div>
    <div class="lg:px-20 lg:p-6 flex justify-evenly">
      <UButton @click="signIn">Sign In</UButton>
      <UButton @click="signInAnonymously">Sign In anonymously</UButton>
      <UButton @click="signUpWithMail">Sign Up</UButton>
    </div>
  </div>
</template>

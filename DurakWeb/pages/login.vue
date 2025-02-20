<script setup lang="ts">
definePageMeta({
  layout: "empty",
})

const errorMessage = ref("")

const supabase = useSupabaseClient()

const email = ref("")
const pw = ref("")
const pwWdh = ref("")

const checkEmilPw = () => {
  if (!/^[^@]+@[^@]+\.[^@]+$/.test(email.value)) {
    errorMessage.value = "Check Email dikker"
    return false
  }
  if (pw.value.length < 6) {
    errorMessage.value = "Mach Pw besser"
    return false
  }
  return true
}

const signIn = async () => {
  if (!checkEmilPw()) {
    return
  }
  const res = await supabase.auth.signInWithPassword({
    email: email.value,
    password: pw.value,
  })

  await handleAuthResult(res)
}

const signInAnonymously = async () => {
  const res = await supabase.auth.signInAnonymously()

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
  const res = await supabase.auth.signUp({
    email: email.value,
    password: pw.value,
  })

  await handleAuthResult(res)
}

const handleAuthResult = async (result: any) => {
  if (result.error || !result.data) {
    errorMessage.value = "Something went wrong. Please try again"
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
  <div class="w-full h-full p-10">
    <div class="text-center p-8">
      <h1 class="text-3xl font-bold">Welcome to the World of Durak</h1>
    </div>
    <div>
      <div class="flex">
        <div class="text-center w-1/2 p-10">
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
        <div class="text-center w-1/2 p-10 border-l-white h-full border-l-2">
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
    <div class="p-6 px-20 flex justify-evenly">
      <UButton @click="signIn">Sign In</UButton>
      <UButton @click="signInAnonymously">Sign In anonymously</UButton>
      <UButton @click="signUpWithMail">Sign Up</UButton>
    </div>
  </div>
</template>

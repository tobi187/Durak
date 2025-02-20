export const addHeader = () => {
  const cfg = useRuntimeConfig()

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
  }

  const auth = cfg.basicAuth

  if (auth) {
    const val = Buffer.from(auth).toString("base64")
    headers["Authorization"] = `Basic ${val}`
  }

  return headers
}

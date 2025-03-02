import { ok, err, Result, ResultAsync } from "neverthrow"
import type { FetchError, FetchOptions } from "ofetch"
import type { IFetchOptions } from "~/types/api"

export const useApi = () => {
  const cfg = useRuntimeConfig()
  const url = cfg.public.url

  const post = async <T>(opts: IFetchOptions): Promise<Result<T, void>> => {
    try {
      const result = (await $fetch<T>(`${url}${opts.url}`, {
        method: "post",
        query: opts.query,
        body: opts.body,
      })) as T

      return ok(result)
    } catch (ex) {
      const error = ex as FetchError
      if (error.status === 401 || error.status === 403) {
        await navigateTo("/login")
      }

      return err()
    }
  }

  const get = async <T>(opts: IFetchOptions): Promise<Result<T, void>> => {
    try {
      const result = (await $fetch<T>(`${url}${opts.url}`, {
        method: "get",
        query: opts.query,
        body: opts.body,
      })) as T

      return ok(result)
    } catch (ex) {
      const error = ex as FetchError
      if (error.status === 401 || error.status === 403) {
        await navigateTo("/login")
      }

      return err()
    }
  }

  return {
    get,
    post,
  }
}

import { ok, err, Result } from "neverthrow"
import type { FetchError } from "ofetch"
import type { IFetchOptions } from "~/types/api"

export const useApi = () => {
  const cfg = useRuntimeConfig()
  const url = cfg.public.url

  const post = async <T>(
    opts: IFetchOptions,
  ): Promise<Result<T, FetchError>> => {
    try {
      const result = (await $fetch<T>(`${url}${opts.url}`, {
        method: "post",
        credentials: "include",
        query: opts.query,
        body: opts.body,
      })) as T
      console.log("Fucking T", result)
      return ok(result)
    } catch (ex) {
      console.log(ex)
      const error = ex as FetchError

      return err(error)
    }
  }

  const get = async <T>(
    opts: IFetchOptions,
  ): Promise<Result<T, number | FetchError>> => {
    try {
      const result = (await $fetch<T>(`${url}${opts.url}`, {
        method: "get",
        credentials: "include",
        query: opts.query,
        body: opts.body,
      })) as T

      return ok(result)
    } catch (ex) {
      console.log(ex)
      const error = ex as FetchError

      return err(error)
    }
  }

  return {
    get,
    post,
  }
}

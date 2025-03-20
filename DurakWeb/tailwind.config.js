/** @type {import('tailwindcss').Config} */
export default {
  content: [],
  safelist: [
    ...[...Array(200).keys()].flatMap((i) => [
      `rotate-[${i + 1}deg]`,
      `rotate-[-${i - 1}deg]`,
    ]),
    ...[...Array(200).keys()].map((i) => `pt-[${i + 1}px]`),
    ...[...Array(20).keys()].map((i) => `z-[${i + 1}]`),
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

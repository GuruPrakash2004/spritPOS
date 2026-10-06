// Vitest config: reuses vite.config.ts (React plugin, "@" alias) and adds the test settings.
// Tests live next to the code as *.test.ts(x) and run in jsdom; src/test/setup.ts loads the
// jest-dom matchers. Run with `npm test` (watch) or `npm run test:run` (once, for CI).
import { defineConfig, mergeConfig } from 'vitest/config'
import viteConfig from './vite.config.ts'

export default defineConfig(env => mergeConfig(viteConfig(env), {
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
    include: ['src/**/*.test.{ts,tsx}'],
    css: false,
    // The first test that imports the whole app (App.test.tsx) needs more than the default 5 s on a slow PC.
    testTimeout: 20000,
    // The tests expect the default address (/api/...), whatever frontend/.env says.
    env: { VITE_API_URL: '', VITE_CENTRAL_API_URL: '' },
  },
}))

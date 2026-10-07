// Vitest config: reuses vite.config.js (React plugin, "@" alias) and adds the test settings.
// Tests live next to the code as *.test.js(x) and run in jsdom; src/test/setup.js loads the
// jest-dom matchers. Run with `npm test` (watch) or `npm run test:run` (once, for CI).
import { defineConfig, mergeConfig } from 'vitest/config'
import viteConfig from './vite.config.js'

export default defineConfig(env => mergeConfig(viteConfig(env), {
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.js'],
    include: ['src/**/*.test.{js,jsx}'],
    css: false,
    // The first test that imports the whole app (App.test.jsx) needs more than the default 5 s on a slow PC.
    testTimeout: 60000,
    // The tests expect the default address (/api/...), whatever frontend/.env says.
    env: { VITE_API_URL: '', VITE_CENTRAL_API_URL: '' },
  },
}))

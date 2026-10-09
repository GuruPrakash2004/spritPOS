import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'
import tailwindcss from '@tailwindcss/vite'
import path from 'path'

// Two pages: index.html (the shop app) and central.html (the hosted central site, src/central/).
// /api/* goes to the FastAPI backend with the prefix stripped: the shop backend (uvicorn app.main:app on
// :8001), or with `--mode central` (npm run dev:central) the central site (uvicorn app.central.main:app on :8002).
// https://vite.dev/config/
export default defineConfig(({ mode }) => ({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(import.meta.dirname, './src'),
    },
  },
  build: {
    rolldownOptions: {
      input: {
        main: path.resolve(import.meta.dirname, 'index.html'),
        central: path.resolve(import.meta.dirname, 'central.html'),
      },
      output: {
        // The React runtime and the icon set rarely change: their own files stay cached across app updates. The
        // charts library is only reached through the Dashboard's lazy pie chart, so it loads on its own, later.
        codeSplitting: {
          groups: [
            { name: 'vendor-react', test: /node_modules[/\\](react|react-dom|scheduler)[/\\]/, priority: 20 },
            { name: 'vendor-icons', test: /node_modules[/\\]lucide-react[/\\]/, priority: 10 },
          ],
        },
      },
    },
  },
  server: {
    proxy: {
      '/api': { target: mode === 'central' ? 'http://localhost:8002' : 'http://localhost:8001', rewrite: path => path.replace(/^\/api/, '') },
    },
  },
}))

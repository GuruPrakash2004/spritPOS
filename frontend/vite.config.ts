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
    rollupOptions: {
      input: {
        main: path.resolve(import.meta.dirname, 'index.html'),
        central: path.resolve(import.meta.dirname, 'central.html'),
      },
    },
  },
  server: {
    proxy: {
      '/api': { target: mode === 'central' ? 'http://localhost:8002' : 'http://localhost:8001', rewrite: path => path.replace(/^\/api/, '') },
    },
  },
}))

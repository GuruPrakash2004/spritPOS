import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'
import tailwindcss from '@tailwindcss/vite'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(import.meta.dirname, './src'),
    },
  },
  // /api/* goes to the FastAPI backend (backend/, uvicorn on :8001).
  server: {
    proxy: {
      '/api': { target: 'http://localhost:8001', rewrite: path => path.replace(/^\/api/, '') },
    },
  },
})

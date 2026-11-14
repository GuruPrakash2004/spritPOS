// Browser entry point of the central site (central.html): mounts <CentralApp /> into #root.

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '../index.css'
import CentralApp from './CentralApp'

// A number box must never change its value when the mouse wheel is turned over it.
document.addEventListener('wheel', () => {
  const el = document.activeElement
  if (el instanceof HTMLInputElement && el.type === 'number') el.blur()
}, { passive: true })

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <CentralApp />
  </StrictMode>,
)

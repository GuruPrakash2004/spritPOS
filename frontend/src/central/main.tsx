// Browser entry point of the central site (central.html): mounts <CentralApp /> into #root.

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '../index.css'
import CentralApp from './CentralApp'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <CentralApp />
  </StrictMode>,
)

// cn(): merge Tailwind class names (clsx + tailwind-merge). Used by the components in components/ui/.

import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

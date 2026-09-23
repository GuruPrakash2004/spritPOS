---
paths:
  - "frontend/models/**"
---

# Vanilla design prototypes (`frontend/models/`)

## The five versions
`Version1-5.html` are single-file, dependency-free design explorations (vanilla JS + inline `<style>`, no build step), each ~25KB. They share the same module list, sample data and render loop, and differ only in visual theme:
- `Version1`: baseline.
- `Version2`: Aurora Glass.
- `Version3`: Editorial Paper.
- `Version4`: Terminal Grid.
- `Version5`: Midnight Sunset, the theme the React app uses.

Each file is one `<script>` containing:
- a `modules` object (nav groups → module names);
- `stock` / `reportCategories` fixtures;
- module-level `let` state (`active`, `dark`, `purchase`, `selectedReport`, `filters`);
- a `render()` that rebuilds `#main.innerHTML` from a template-string function per screen (`dashboard()`, `reportsPage()`, `purchasePage()`, `generic()`, …).

Handlers are inline `onclick="..."` calls to globals, so every mutation ends in `render()`.

In Version2-5, the theme is a base stylesheet followed by override blocks (e.g. `/* Version 5: Midnight Sunset */`, `/* Midnight Sunset navigation refinement */`). Change the overrides, not the shared base.

**Keep the five versions in sync when changing behaviour.** They intentionally share the same data and module list.

## Pixel-matching workflow
`Design.md` defines how to match a reference image:
1. Screenshot, compare and fix.
2. Repeat at least twice, until within ~2–3px.

It forbids adding content or "improving" the design, and requires given style tokens to be used verbatim. Its "Technical Defaults" (Tailwind CDN, a single `version3.html`) are stale: none of the prototypes load Tailwind or any other script.

---
description: Analyze the full picture of a legacy system from a bird's-eye view. Progressively create system correlation diagrams, DFD, and I/O interface diagrams.
argument-hint: "<target system or subsystem name>"
---

# /deep-dive -- In-depth System Analysis

## Overview

A workflow for gaining a bird's-eye view of a legacy system.
Sequentially execute current-legacy skill Phases 0-3 to create system correlation diagrams, DFD, and I/O interface diagrams.

## Workflow

### Step 1: Phase 0 -- Foundation Building

Apply **current-legacy** skill (Phase 0):

- Auto-collect from README, docker-compose.yml, dependency management files (.env.example)
- Extract repository roles, technology stack, and dependencies
- Ask 9 clarifying questions about business domain and context
- Generate initial system overview diagram and "Known/Unknown" list

$ARGUMENTS

> **Checkpoint**: "System overview analysis complete. Ready to proceed to the next phase (correlation diagrams)?"

### Step 2: Phase 1 -- Dig from the Concrete

Apply **current-legacy** skill (Phase 1 [change theme]):

- Select a specific change theme (e.g., "add a new product category")
- Trace impact across repositories using /change-impact
- Document cross-repository connections, hidden outputs, hardcoded values
- Update system overview diagram and DFD fragments
- Refine "Known/Unknown" list

> **Checkpoint**: "Correlation diagram complete. Ready to proceed to the next phase (DFD)?"

### Step 3: Phase 2 -- Audit to Eliminate Falsehoods

Apply **current-legacy** skill (Phase 2):

- metsuke (Inspector) audits Phase 1 deliverables for accuracy
- Verify all method names, class names, file paths exist
- Cross-check code snippets against actual implementation
- Validate application of project-specific rules (e.g., soft-delete conditions)
- Correct findings and deliver audited artifacts

> **Checkpoint**: "DFD complete. Ready to proceed to the next phase (I/O diagrams)?"

### Step 4: Phase 3 -- Return to the Abstract

Apply **current-legacy** skill (Phase 3):

- Integrate Phase 1-2 fragments into three consolidated maps
- Update system overview diagram with new connections
- Add data flows to DFD
- Document I/O interfaces and external connections
- Persist domain knowledge to knowledge/domain/
- Update "Known/Unknown" list

### Step 5: Suggest Next Actions

- "To investigate a specific feature in detail, use `/understand`"

---
description: Track down bugs — trace symptoms to root cause, identify the exact file and line, and plan the fix. Execute project-guide -> current-spec skill chain.
argument-hint: "<bug symptoms / error description>"
---

# /bug-hunt -- Bug Investigation Flow

## Overview

A workflow for systematically executing from cause identification to fix strategy formulation for bug fix tasks.

## Workflow

### Step 1: Get Reference Guide

Apply **project-guide** skill:

- Identify the document reference order needed for bug fixing

$ARGUMENTS

> **Checkpoint**: "Reference guide prepared. Ready to proceed to the next phase (cause investigation)?"

### Step 2-3: Cause Investigation + Specification Verification

Apply **current-spec** skill:

- Trace error symptoms through the code flow (trace error → identify cause location (file:line))
- Identify the cause location with file path:line number
- Record discovered risks
- Organize the specification of the cause location's Service and verify correct behavior
- Provide judgment material for the fix strategy

> **Checkpoint**: "Have you pinpointed the root cause with a specific file path and line number, and verified the correct specification? Ready to proceed with the implementation?"

### Step 4: Suggest Next Actions

- "If you'd like to proceed with the fix, use `/implement`"
- "If the impact scope is wide, proceed with impact analysis using `/investigate-flow`"

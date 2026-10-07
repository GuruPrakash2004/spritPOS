---
description: Map unfamiliar code — understand how a feature works, its dependencies, and data flow before making changes. Execute the project-guide -> current-spec skill chain.
argument-hint: "<feature or process name to understand>"
---

# /understand -- Code Comprehension Flow

## Overview

A workflow for systematically executing specification understanding and code reading of legacy code.

## Workflow

### Step 1: Get Reference Guide

Apply **project-guide** skill:

- Identify the document reference order needed for investigation and analysis

$ARGUMENTS

> **Checkpoint**: "Reference guide prepared. Ready to proceed to the next phase (code investigation)?"

### Step 2-3: Code Investigation + Specification Organization

Apply **current-spec** skill:

- Explore the code flow and dependency relationships of the target feature
- Identify key classes, methods, and tables
- Systematically document the specification of discovered key Services
- Organize method lists, dependencies, and business rules

> **Checkpoint**: "Do you have a clear picture of the overall structure, key dependencies, and data flow? Ready to proceed?"

### Step 4: Suggest Next Actions

- "If planning changes, proceed with impact analysis using `/investigate-flow`"

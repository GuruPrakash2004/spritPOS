---
description: Systematically execute impact investigation for legacy systems. Sequentially execute the project-guide -> current-spec -> change-impact skill chain.
argument-hint: "<description of the change>"
---

# /investigate-flow -- Impact Investigation Flow

## Overview

A workflow for systematically investigating the impact scope when adding new features or changing specifications.
Sequentially execute 4 skills and output the final ADR-format impact analysis report.

## Workflow

### Step 1: Get Reference Guide

Apply **project-guide** skill:

- Analyze the task overview and identify the order of documents to reference
- Present the optimal reference order from knowledge/system/ and knowledge/domain/

$ARGUMENTS

> **Checkpoint**: "Reference guide prepared. Ready to proceed to the next phase (investigation)?"

### Step 2-3: Codebase Investigation + Specification Organization

Apply **current-spec** skill:

- Pre-read documents identified in Step 1
- Explore target classes/features using Serena's symbolic search
- Document method lists, dependencies, and business rules
- Create investigation report with file path:line numbers
- Organize specifications of high-impact Services in detail

> **Checkpoint**: "Investigation and specification organization complete. Ready to proceed to the next phase (impact analysis)?"

### Step 4: Impact Analysis

Apply **change-impact** skill:

- Create an ADR-format impact analysis report based on investigation and specification results
- Includes risk assessment and phased implementation plan

### Step 5: Suggest Next Actions

- "If you'd like to proceed with the fix, use `/implement`"
- "If deeper investigation is needed, use `/deep-dive`"

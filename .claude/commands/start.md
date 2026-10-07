---
name: start
description: Interactive guide to help you get started with CLysis. Works for both users with a clear goal and those exploring what's possible.
allowed-tools: Read, Grep, Glob, Bash, Agent
---

# /start -- CLysis Getting Started Guide

## Overview

This command is the single entry point for CLysis. It guides users to the right skill chain based on their current situation.

## Step 0: Project Initialization (auto, idempotent)

Run the following to set up the CLysis directory structure. Safe to run multiple times — existing directories are preserved.

```bash
mkdir -p knowledge/{domain,system,adr,archive}
mkdir -p workspace/{in_progress,pending_merge,planned}
mkdir -p reports
```

If all directories already exist, skip to the next step.

## Step 1: Determine the User's Intent

Ask the user:

**"How would you like to get started?"**

Present two paths:

### Path A: "I have a specific goal"

If the user has a clear objective, present the lineup:

| Goal | Command | What You'll Get |
|------|---------|----------------|
| **Understand the entire system** | `/deep-dive` | Architecture overview, DFD, I/O interface maps |
| **Understand a specific feature** | `/understand [feature]` | Service spec summary with code-level details |
| **Investigate a bug or incident** | `/bug-hunt [symptom]` | Root cause analysis with fix recommendations |
| **Plan a new feature or change** | `/investigate-flow [change]` | Impact analysis across repositories |
| **Review a pull request** | `/review [PR-URL]` | Code quality and business logic review |

Ask: "Which is closest to what you need?"

### Path B: "What can CLysis do?"

If the user is exploring, present the full capability overview:

**CLysis helps you conquer legacy systems through 4 phases:**

```
Phase 1: INVESTIGATE — Understand what exists
  /deep-dive    → System-wide overview (architecture, DFD, I/O maps)
  /understand   → Feature-level specification
  /current-spec → Code-level investigation

Phase 2: ANALYZE — Assess risks and impact
  /current-distortion → Detect structural code problems
  /change-impact      → Trace change impact across repos

Phase 3: EXECUTE — Make changes safely
  /bug-hunt    → Find and fix bugs
  /implement   → Propose changes → Create PR

Phase 4: CAPTURE — Preserve knowledge
  /current-prd → Reverse-engineer specs from code
  /doc-update  → Update docs for target audience
```

Ask: "Which phase interests you? Or describe your situation and I'll recommend where to start."

## Step 2: Guide to First Action

Based on the user's selection, provide:

1. **What the command does** (1-2 sentences)
2. **What input is needed** (repository path, feature name, etc.)
3. **What output to expect** (report location, document type)
4. **What comes next** (the natural follow-up command)

### Recommended Journeys

**Journey 1: System Onboarding (new to the codebase)**
```
/deep-dive [repo] → /understand [key-feature] → /current-prd [repo]
```
"Start with the big picture, then zoom into key features, then generate specs."

**Journey 2: Bug Investigation**
```
/bug-hunt [symptom] → /current-distortion [area] → /implement
```
"Find the bug, check for structural problems nearby, then fix."

**Journey 3: Feature Development**
```
/investigate-flow [change] → /implement
```
"Understand the impact first, then make the change safely."

**Journey 4: Knowledge Building**
```
/current-prd [repo] → /doc-update [audience]
```
"Extract specs from code, adapt for your audience."

**Journey 5: Code Review**
```
/review [PR-URL]
```
"Review with business logic awareness."

## Step 3: Execute

Launch the selected command. After completion, suggest the natural next step from the journey.

## Important Notes

- This command is the **entry point only**. It delegates to existing commands.
- If the user's goal doesn't match any pattern, help them decompose it into smaller goals that do match.
- Always ask before executing a command — never auto-launch without confirmation.
- Journeys are recommendations, not requirements. Users can skip steps or combine freely.

---
name: grill-me
description: Act purely as a "questioner" against tasks, plans, and designs. Surface undecided decisions, unnoticed assumptions, and avoided trade-offs through the power of questions alone. No code is written.
argument-hint: "<task summary or file path>"
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# Grill Me Skill

## Overview

AI acts strictly as a **questioner** (not a reviewer). When the user presents a new task, requirement, or design, this skill generates 7-8 questions to force articulation of:

- Undecided decisions
- Unnoticed assumptions
- Avoided trade-offs
- Scope/priority/risk gaps

No code is written. No fixes are proposed. Questions only.

See config/terminology.md for term customization

## Roles and Responsibilities

| Role | Responsibility |
|------|----------------|
| **Shogun (Leader)** | Trigger skill, receive report, bridge to subsequent skills |
| **Ashigaru (Worker)** | Execute this skill: context collection, question generation, result recording |
| **Metsuke (Inspector)** | Validate question quality, detect hallucination, assess fitness for downstream skills |

> **Mandatory**: Output must always be saved to file (F006 compliance). stdout-only reporting is prohibited.

## Key Principles

- **Question only**: Never provide answers, suggestions, or recommendations
- **Respectful tone**: "Let me confirm..." style, not aggressive or leading
- **Subject-explicit**: Always include "whose/what" context in questions
- **Forced exit**: User can say "enough" or "done" to immediately end
- **7-8 questions max**: Present all at once (not one-by-one)
- **No speculation**: Do not guess answers or fill in blanks
- **Context-aware exclusion**: Skip questions whose answers are already evident from context; mark them as "already confirmed"
- **Maximum 8 questions**: No more than 8 total (7-8 is the typical range)
- **Batch presentation only**: Generate all questions at once, never in Q&A exchange format

## Trigger Conditions

| Condition | Timing |
|-----------|--------|
| New task presented | User says "add X", "change Y", or presents a new task |
| Large-scale change | Cross-repository changes, API design, data model changes |
| Requirements validation | User says "validate requirements" or "check assumptions" |
| Plan review | Before complex multi-step tasks |

## Usage

```
/grill-me "<task summary>"
/grill-me "reports/workspace/in_progress/xxx.md"
```

### Execution Examples

| Use Case | Command |
|----------|---------|
| New task start | `/grill-me "Add pricing feature for teacher plan"` |
| Requirements validation | `/grill-me "reports/workspace/planned/feature-x.md"` |
| Before large-scale change | `/grill-me "<change summary>"` |

## Execution Steps

### Step 1: Context Collection

**Purpose**: Understand the task/file the user presented; scan related domain knowledge.

**Actions**:
- Read the task summary or referenced files
- Scan related `knowledge/` files (reuse `/project-guide` logic)
- Check related GitHub Issues if available
- Review session memory for prior discussions on this topic

**Notes**:
- If the file is not found, state "File not found. Treating as text input." and proceed
- Do not speculatively supplement background information. Use only confirmed file content

### Step 2: Generate Questions (7-8 questions)

**Purpose**: Generate 7-8 questions and present them all at once.

One question per category. Skip categories already clearly answered by context.

| # | Category | Question Intent | Example |
|----|----------|----------------|---------|
| 1 | Scope | Clarify what is included/excluded | "What is included/excluded from the scope?" |
| 2 | Assumptions | Verify underlying premises | "Is this assumption correct: ...?" |
| 3 | Priority / Trade-offs | Clarify what to prioritize when choices conflict | "Between A and B, which takes priority?" |
| 4 | Risk / Fallback | Identify worst-case scenarios and contingencies | "What is the worst case? What is the fallback?" |
| 5 | Dependencies | Identify prerequisites and blockers | "What must be completed before this?" |
| 6 | Completion criteria | Define "done" | "What defines 'done'?" |
| 7 | Stakeholders | Identify affected parties | "Who is affected by this change?" |
| 8 | Technical constraints | Surface non-functional requirements | "Are there performance/compatibility/security constraints?" |

**Important**:
- Present all questions **at once** (no one-by-one Q&A)
- Skip questions already answered by context; mark them as "Omitted: already confirmed from context"
- Do not speculate answers or complete responses for the user
- Identify obvious answers from context and list them in the "Skipped Questions" section with the reason
- Generate 7-8 questions, excluding context-confirmed items

### Step 3: Await User Responses

**Execution**:
- Present questions in the standard report format
- Wait for the user to respond
- Record responses as Q&A pairs
- If user says "enough" or "done" at any point, immediately stop and proceed to Step 4

**Optional: User interruption handling**:
- If user says "enough" or "done" before all questions are answered
  - Cease question collection immediately
  - Save collected responses
  - Step 4: Record unanswered questions in "Unresolved Items" section
  - Propose subsequent skills

### Step 4: Save Results

**Purpose**: Organize Q&A pairs, create handoff section for downstream skills.

Output format:
```markdown
# Grill Results -- {task summary}

> Date: {YYYY-MM-DD}
> Target: {task summary}

## Confirmed Items (Q&A pairs)

### 1. Scope
**Q**: {question}
**A**: {user's answer}

### 2. Assumptions
**Q**: {question}
**A**: {user's answer}

### 3. Priority / Trade-offs
**Q**: {question}
**A**: {user's answer}

### 4. Risk / Fallback
**Q**: {question}
**A**: {user's answer}

### 5. Dependencies
**Q**: {question}
**A**: {user's answer}

### 6. Completion Criteria
**Q**: {question}
**A**: {user's answer}

### 7. Stakeholders
**Q**: {question}
**A**: {user's answer}

### 8. Technical Constraints
**Q**: {question}
**A**: {user's answer}

## Skipped Questions
- {question}: {reason}

**If none were skipped, omit this section.**

## Handoff to Subsequent Skills

| Item | Confirmed Content |
|------|-------------------|
| Scope | {confirmed} |
| Assumptions | {confirmed} |
| Priority | {confirmed} |
| Trade-offs | {confirmed} |
| Risk / Fallback | {confirmed} |
| Dependencies | {confirmed} |
| Completion criteria | {confirmed} |
| Stakeholders | {confirmed} |
| Technical constraints | {confirmed} |

## Unresolved Items (if any)

- {question number}: {reason}
```

Save to: `reports/grill/{YYYYMMDD}-{topic}.md`

---

## Handoff to Subsequent Skills

After grill-me questions are answered, the confirmed decisions form the foundation for downstream skills:

| Subsequent Skill | When to Use | Input |
|------------------|------------|--------|
| `/project-guide` | Task scope confirmed | Grill results summary |
| `/change-impact` | Modification task identified | Grill results + change scope |
| `/current-spec` | Implementation details needed | Grill results + target component |
| `/create-pr --plan` | Solution approach determined | Grill results + design decisions |

## Execution Environment

| Item | Value |
|------|-------|
| Output directory | `reports/grill/` |
| Naming convention | `{YYYYMMDD}-{topic}.md` (e.g., `20260415-pricing-feature.md`) |
| File format | Markdown |

## Differentiation from Other Skills

| Skill | Timing | Role |
|-------|--------|------|
| `/grill-me` | Before task start | Surface undecided decisions, assumptions, and trade-offs (questions only) |
| `/project-guide` | After task overview confirmed | Guide to relevant domain knowledge and reference implementations |
| `/change-impact` | Before implementation | Investigate and visualize impact scope on related systems |
| `/current-spec` | When specification details are unclear | Reverse-engineer current implementation from actual code |
| `/create-pr --plan` | When solution approach is determined | Draft modification proposal with testing strategy and risk assessment |

## Prohibited Actions

- Writing or editing code (this skill's sole purpose is asking questions)
- Providing solutions, improvements, or recommendations
- Denying or evaluating user's answers ("that's wrong", etc.)
- Asking more than 8 questions (7-8 is the limit)
- One-by-one Q&A format (must present all at once)
- Leading questions ("Shouldn't we really do X?")
- Speculating or completing user's answers

## Tone and Style

- "Let me confirm..." base tone
- Respectful: "Would it be...?", "Could you clarify...?"
- Never interrogative or pressuring
- Subject-explicit: "From the perspective of X...", "Regarding impact on Y..."

## Subsequent Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/create-pr --plan` | Modification task | Report to leader with grill results file path, obtain approval |
| `/change-impact` | Impact analysis needed | Report to leader with grill results file path, obtain approval |
| `/project-guide` | Task type undetermined | Propose to leader, await judgment |
| `/current-spec` | Specification investigation needed | Report to leader, obtain approval |

## Fallback Handling

**File not found**:
- State "File not found. Treating input text as the target." and proceed with question generation

**Context extremely limited** (no related knowledge/ files):
- Generate questions, but note "Background information is limited; please provide more specific premises."
- Proceed normally

**User says "enough" or "done"**:
- Immediately stop collecting responses
- Save responses collected so far
- Record unanswered questions in "Unresolved Items" section
- Propose subsequent skills

## Quality Checkpoints

- [ ] Question count is within 7-8 range
- [ ] Questions contain only "questions" -- no answers, proposals, or suggestions
- [ ] Tone is "Let me confirm..." style, not aggressive
- [ ] Questions already answered by context are skipped (with reason noted)
- [ ] Results are saved to file (F006 compliance)
- [ ] Handoff section for subsequent skills is included
- [ ] No speculation or answer completion
- [ ] Context from knowledge/ files is only factual content (no inference)
- [ ] Responses are confirmed directly from the user (not guessed)
- [ ] Unanswered items section exists (if applicable)

## Operating Principles (Inspector-Reviewed)

### A-001: Hallucination Prevention

- Record only facts confirmed by actual file scans
- Do not speculate or complete the user's answers
- If a referenced file is not found, explicitly state "File not found"
- Do not record uncertain information

### A-002: F006 Strict (File Save Mandatory)

- Output must always be saved to `reports/grill/{YYYYMMDD}-{topic}.md`
- A stdout report is also required, but the file save itself is mandatory
- An execution that does not produce a file is treated as a failure

### A-003: Stay in the Questioner Role

- Do not propose fixes or improvements (that is the leader's role)
- Do not deny or evaluate the user's answers ("that's wrong", etc.)
- Do not lead with speculative phrasing such as "isn't it really X...?"
- Restrict output to questions and response collection only
- Present all questions as a batch; do not engage in iterative dialogue
- When questions cannot be answered due to information gaps, state "Background information insufficient" and pause for clarification

## I/O Specification

### INPUT
| Type | Format | Required/Optional | Example |
|------|--------|-------------------|---------|
| Task summary | Text or file path | Required | `"Add pricing feature for teacher plan"` |

### OUTPUT
| Type | Format | Destination | Required/Optional |
|------|--------|-------------|-------------------|
| Grill results | Markdown | `reports/grill/{YYYYMMDD}-{topic}.md` | Required (F006) |
| Leader report | Standard report format | stdout | Required |

### Prerequisites
- User has presented a task summary
- `reports/grill/` directory exists (create if missing)
- `knowledge/` files accessible for context scan when available

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/create-pr --plan` | Modification task | Report to leader with grill results file path, obtain approval |
| `/change-impact` | Impact analysis needed | Report to leader with grill results file path, obtain approval |
| `/project-guide` | Task type undetermined | Propose to leader, await judgment |
| `/current-spec` | Specification investigation needed | Report to leader, obtain approval |

> **Fallback**: If prerequisites are not met, report to leader and await further instructions

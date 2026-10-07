---
name: current-distortion
description: Distortion Analysis - Detect code distortion patterns and organize findings in Part A/B/C framework
argument-hint: "[repository-name] [target-area]"
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# /current-distortion -- Distortion Analysis Skill

## Overview

Systematically detect "distortions" (validation gaps, implicit dependencies, type comparison traps, race conditions, etc.) lurking in legacy code and organize findings from three perspectives (business process-driven, root cause-driven, and remediation-impact-driven). This skill produces structured, actionable reports with risk classification via a 2-axis framework: **5 Root Cause Axes (A-E)** and **6 Symptom Patterns (DR1-DR6)**.

**Design Philosophy**: "Detect risks" then "Organize from 3 perspectives" (output-centric 2-Phase design). Distortion investigation does not produce standalone artifacts; instead, risks are naturally embedded within the Part A/B/C output.

See `config/terminology.md` for term customization.

## Scope

### Target
- Real-world problems existing in codebase across multiple repositories
- Cross-repository problems spanning service boundaries (P7-P11)
- Structural and logical distortions affecting data integrity, control flow, and system boundaries
- Root causes spanning validation gaps (A), type/value mismatches (B), logic gaps (C), architectural flaws (D), and security issues (E)
- All 6 symptom patterns (DR1-DR6): data corruption, crashes, missing checks, inconsistencies, data loss, cross-system divergence

### Out of Scope
- Hypothetical/generic anti-patterns (comprehensive SOLID violations, etc.)
- Security vulnerability audits (handled by `/review-code`)
- Performance optimization (separate investigation)
- Problems requiring business specification changes (requires human decision first)
- Operational knowledge loss (addressed via documentation and knowledge management)

---

## Target

$ARGUMENTS

## Usage

### Command Syntax

```
/current-distortion [repository-name] [target-area]
/current-distortion Phase 1 [repository-name] [target-area]
/current-distortion Phase 2
```

### Examples

```
# Default (all-in-one: Phase 1 -> Phase 2 in sequence)
/current-distortion your-ec-repo checkout-flow
/current-distortion your-backend-repo order-delivery

# Step-by-step execution (explicit Phase)
/current-distortion Phase 1 your-backend-repo order-delivery
/current-distortion Phase 2

# Discussion integration
/current-distortion your-ec-repo checkout-flow --discussion 42
```

When Phase is omitted, it runs as all-in-one (All). Step-by-step execution requires explicit Phase specification.

---

## Execution Flow

```
Phase omitted (default): Phase 1 -> Leader integration -> Phase 2 (all-in-one)
Phase 1 only: Risk collection -> Leader verbally reports count & severity distribution to User
Phase 2 only: Takes Phase 1 integration results as input -> Creates Part A/B/C
```

### Overall Flow

```mermaid
graph TD
    subgraph Phase1["Phase 1: Risk Collection (parallel)"]
        A1["Worker A: Integrate existing investigation results"]
        A2["Worker B: Flag consistency check gaps"]
        A3["Worker C: Cross-table consistency"]
        A4["Worker D: Constant definition vs DB value drift"]
        A5["Worker E: Incomplete conditional branching"]
        A6["Worker E2: Concurrency, async, external dependencies"]
    end

    subgraph Merge["Leader: Result integration & deduplication"]
        M1["Integrated risk list (no file output)"]
    end

    subgraph Phase2["Phase 2: Part A/B/C Creation (parallel)"]
        B1["Worker F: Part A (Process-driven)"]
        B2["Worker G: Part B (Root cause-driven)"]
        B3["Worker H: Part C (Remediation impact + visualization)"]
    end

    subgraph Output["Final Deliverables"]
        O1["Integrated report: Part A/B/C"]
    end

    A1 --> M1
    A2 --> M1
    A3 --> M1
    A4 --> M1
    A5 --> M1
    A6 --> M1

    M1 --> B1
    M1 --> B2
    M1 --> B3

    B1 --> O1
    B2 --> O1
    B3 --> O1
```

---

## Phase 1: Risk Collection

### Purpose

Detect all risks that serve as material for Part A/B/C. Runs integration of existing investigation results and code distortion investigation (4 perspectives) in parallel.

### Worker Formation

Launch 6 workers in parallel.

| Worker | Agent Type | Assignment | Investigation Content | Pattern Coverage |
|--------|-----------|------------|----------------------|-----------------|
| Worker A | investigator | Integrate existing investigation results | Extract and merge known risks from past reports in reports/; deduplicate; preserve source tracking | All patterns |
| Worker B | investigator | Flag consistency & soft-delete gaps | Implicit shared-flag dependencies, missing logical deletion filters, JOIN condition validation | P1, P4, P11 |
| Worker C | investigator | Cross-table consistency & conversions | Foreign key constraints, data transformation gaps, format/type divergence between tables | P2, P5, P11 |
| Worker D | investigator | Constant/Enum definition drift | Mismatch between code constant defs and DB values; hardcoded values; multi-repo divergence | P2, P3 |
| Worker E | investigator | Type comparison & branching gaps | Type coercion pitfalls, insufficient case handling, missing else/default branches, validation omissions | P3, P6 |
| Worker E2 | investigator | Concurrency, async, external deps | Idempotency gaps, unreplayable transitions, async state sync issues, external service SPoFs | P7, P8, P9, P10 |

### Detection Framework: Problem Patterns (P1-P11)

Each worker uses these 11 problem patterns as detection criteria:

| ID | Pattern Name | Description | Detection Focus |
|----|-------------|-------------|----------------|
| P1 | Implicit shared flag dependency | Multiple processes reference the same flag column/variable with different assumptions | Cross-search references to flag columns/variables. Record "whose/what" flag using Subject-First Rule |
| P2 | Constant/Enum mismatch | Code constant definitions diverge from DB stored values or differ across repositories | Compare Enum/const declarations with DB initial data and schema; cross-repo comparisons |
| P3 | Type comparison traps | Language-specific loose comparison pitfalls (PHP `==` vs `===`, missing strict flags in searches) | Loose equality operators, missing strict parameters, switch-case type coercion |
| P4 | Soft-delete/JOIN condition gaps | Missing logical deletion filters (e.g., soft-delete flags) or incomplete JOIN conditions | SQL queries, ORM scopes, raw queries; search for deleted-record filters |
| P5 | Implicit value conversion | Unintended type/value conversions during processing affect correctness | Casts, type conversion functions, date/time parsing, encoding/decoding |
| P6 | Insufficient branching | Missing cases, if-else without else, switch without default, unhandled exceptions | Branch coverage analysis, explicit error handling validation |
| P7 | Non-idempotent concurrent operations | Concurrent mutations on same resource lack idempotency; collisions cause data inconsistency | Database/file mutation without idempotency guarantees; concurrent request handling without locks |
| P8 | Unreplayable state transitions | Mid-way processing failure cannot safely retry due to irreversible side effects | External API calls followed by DB updates; SFTP sends before DB updates; message publishing before persistence |
| P9 | Async boundary gaps | Integrity between sync and async processing not guaranteed; state stuck in intermediate states | Sync request completes before async worker starts; flag updates delayed or not propagated |
| P10 | External dependency single points of failure | External service dependencies lack redundancy, timeouts, or recovery; messages can be lost | No retry limits, no fallback paths, message loss on service unavailability |
| P11 | Data context loss | Data loses intent/metadata as it transforms through integration points | Identity/reference loss when external process writes; session context not persisted; type/semantic divergence across boundaries |

### Classification Framework: 2-Axis Risk Model

Risk classification uses **two orthogonal axes**: Root Cause (A-E) and Symptom Pattern (DR1-DR6).

#### Root Cause Axes (A-E)

**Classification Priority**: E → D → C → B → A (evaluate in this order; highest-ranked root cause is primary)

| ID | Root Cause | Description | Scope |
|----|-----------|-------------|-------|
| E | Security & Robustness defects | Missing authentication, authorization, input validation; insufficient error handling | Authentication gaps, injection vulnerabilities, unhandled exceptions |
| D | Architecture & Design flaws | Structural issues: responsibility separation failures, circular dependencies, inappropriate layering, session-dependent designs | Design patterns, system boundaries, component coupling |
| C | Algorithmic/Logic gaps | Incomplete branching, missing cases, unhandled edge cases; conditions without corresponding handlers | if-else without else, switch without default, null checks, case coverage |
| B | Value/Type conversion defects | Type mismatches, implicit conversions, encoding/format divergence, constant-definition drift | Type coercion, casting, data representation mismatches |
| A | Validation bypass | Insufficient validation allowing invalid data to pass downstream | Input validation, state preconditions, boundary checks |

#### Symptom Patterns (DR1-DR6)

Describes **how the risk manifests** in the system:

| ID | Symptom | Description | Typical Manifestation |
|----|---------|-------------|----------------------|
| DR1 | Invalid data passes | Invalid/expired values flow to downstream processing without rejection | Expired items in cart, out-of-range values in computations |
| DR2 | Stops/crashes midway | Processing succeeds initially, then fails downstream or crashes unexpectedly | Order created but fulfillment dispatch errors; NullPointerException in payment |
| DR3 | Check missing entirely | Required validation step does not exist in code | No expiration check at all, missing authorization check |
| DR4 | Inconsistent behavior | Same operation produces different results in different contexts or systems | Pricing differs between frontend validation and backend logic |
| DR5 | Silent data loss | Data modified, lost, or corrupted without explicit error | Concurrent updates overwrite each other; async transitions skip states |
| DR6 | Cross-system divergence | Data representation or meaning differs across service boundaries or repositories | ProductType stored as int in one service, string in another |

### Subject-First Rule

When documenting risks, always explicitly state "whose/what" as the subject for flags, variables, and columns.

```
Bad: "When soft-delete flag is 0..."
Good: "When event table's soft-delete flag (event logical deletion flag) is 0..."

Bad: "publication_end_date is not checked"
Good: "Event's publication end date (event.publication_end_date) is not checked in PaymentProcessor.php's payment processing"
```

### Legacy Distortion Pattern Mapping (A-F to Root Cause + Symptom)

This section maps traditional distortion patterns (A-F) to the 2-Axis model (Root Cause A-E + Symptom Pattern DR1-DR6) for reference:

| Legacy Pattern | Root Cause Axis | Symptom Pattern | Meaning |
|---|---|---|---|
| A | Validation bypass | Invalid data passes | Insufficient validation allows invalid data downstream |
| B | Type/Value conversion defects | Inconsistent behavior / Cross-system divergence | Type coercion, implicit conversion, or encoding mismatch |
| C | Logic/Branching gaps | Check missing entirely | Missing validation or incomplete case handling |
| D | Architecture & Design flaws | Inconsistent behavior / Cross-system divergence | Structural issues in responsibility, design, or layering |
| E | Security & Robustness defects | (varies) | Authentication, authorization, or robust error handling gaps |
| F | Integrity defects (cross-system) | Cross-system divergence | Value, type, or semantic mismatch across repository boundaries |

When classifying a risk in this skill, prefer the 2-Axis model (A-E root cause + DR1-DR6 symptom) for consistency and clarity. The legacy A-F patterns are provided for reference only.

### Phase 1 Output

**No file output by default.** The leader receives all workers' results, performs deduplication and integration, and holds them locally. Passes directly to Phase 2.

**When Phase 1 only is executed**: The leader verbally reports the integration summary (count, severity distribution) to the user. Only output to an intermediate file if the user explicitly requests "save to file."

### Phase 1 to Phase 2 Handoff Contract

After Phase 1 completion, processing follows one of these 3 patterns:

| Pattern | Condition | Leader Action |
|---------|-----------|---------------|
| **Immediate Phase 2** | Phase not specified (all-in-one mode) | Hold integrated results locally and immediately launch 3 Phase 2 workers |
| **Intermediate file output** | Phase 1 only executed AND user requests "save to file" | Output integrated risk list to `reports/distortion-phase1-[repo]-[area]-[date].md` |
| **Phase 1 only completion** | Phase 1 only executed AND no additional user instruction | Report summary (count, severity distribution) verbally and ask user whether to proceed to Phase 2 |

**When executing Phase 2 in a separate session**: An intermediate file is required. The leader must ask the user at Phase 1 completion: "If Phase 2 may be executed in a separate session, should an intermediate file be created?"

#### Phase 2 Worker Delegation Template (for leader use)

```
**Important: You are a worker (executor). You are not the leader.
Do not propose team formation or consult the planner.
Execute the analysis yourself and write results to a file.**

[Phase 2 Assignment]
- Assigned Part: Part {A/B/C}
- Target Repository: {repository name}
- Target Area: {area name}

[Input]
- Phase 1 Integrated Risk List: {local integration results or intermediate file path}
- Risk Count: {count} (High {X}, Medium {Y}, Low {Z})

[Output Rules]
- Output destination: {leader-specified temporary file path} (file persistence required)
- Template: Follow the Part {A/B/C} section of this skill's "Integrated Report Template"
- Visualization: Must include mermaid diagrams (Part C additionally requires remediation ordering constraint diagram)

[Completion Criteria]
- Assigned Part section is complete
- Risk count matches Phase 1 integration results (no omissions or duplicates)
```

---

## Phase 2: Part A/B/C Creation

### Purpose

Organize all risks collected in Phase 1 from 3 perspectives and create final deliverables.

### Worker Formation

Launch 3 workers in parallel.

| Worker | Agent Type | Assignment | Output File |
|--------|-----------|------------|-------------|
| Worker F | general-purpose | Part A: Business Process-driven | Part A section of `reports/distortion-report-[repo]-[area]-[date].md` |
| Worker G | general-purpose | Part B: Root Cause-driven | Part B section of same file |
| Worker H | general-purpose | Part C: Remediation Impact-driven + Visualization | Part C section of same file |

**Note**: The integrated report is a single file. In Phase 2, the leader integrates the 3 workers' outputs into 1 file. Each worker creates only their respective section; the leader performs integration.

### Part A: Business Process-driven

Map all risks to business processes (e.g., PR1-PR5 or your project's process definitions).

- Place risks at each process sub-step
- List distortion-discovered risks and existing investigation risks without distinction
- Include source breakdown (existing investigation: Y items + distortion survey: Z items) in the overview section

### Part B: Root Cause-driven

Classify all risks by problem pattern (P1-P11) and map to root cause axes (A-E) and symptom patterns (DR1-DR6).

**Part B Responsibility**: Describe state and severity (why it happens + how critical). Explain root cause chain (which axis is primary + which problem pattern). Delegate remediation specifics to Part C.

- Analyze root causes for each pattern (P1-P11)
- Map to root cause axes (A-E) using priority order: E → D → C → B → A
- Map to symptom patterns (DR1-DR6)
- Present remediation approach and effort estimate
- **Remediation Priority Table** (the core of Part B): List remediation targets, resolved risks, and ROI

### Part C: Remediation Impact-driven (WHAT TO DO)

Organize remediation priority, remediation class, side effects, and ordering constraints for all risks. Provide a decision-maker-oriented view.

**Remediation Class Classification**:

| Class | Meaning | Criteria |
|-------|---------|----------|
| **Quick Fix** | Single file, few lines, no side effects | Low difficulty + high severity |
| **Planned Fix** | Multiple files, no design change needed | Medium difficulty + high severity |
| **Architecture Fix** | Includes design changes, cross-cutting | High difficulty OR P7-P10 pattern |
| **Deferred** | Currently no business impact, high fix cost | High difficulty + low severity |

- Remediation priority matrix (ROI: difficulty vs resolved count)
- Remediation side-effect map (when Fix A impacts System B)
- Remediation ordering constraints (dependencies between fixes)

---

## Output Format

### File Name

```
reports/distortion-report-[repository]-[area]-[date].md
```

**Examples**:
```
reports/distortion-report-ec-checkout-flow-2026-03-13.md
reports/distortion-report-backend-order-delivery-2026-03-13.md
```

### Integrated Report Template

```markdown
# [Area] Distortion Analysis Report: [Repository Name]

## Investigation Overview
- Date: YYYY-MM-DD
- Target Repository: [repository-name] (`[repository-path]`)
- Investigation Scope: [target area description]
- Distortions Found: X items (High Y, Medium Z, Low W)

---

## Distortion Investigation Results

### Summary Table

| ID | Risk Name | Severity | Symptom Pattern | Root Cause | Problem Pattern | Primary File |
|----|-----------|:--------:|:---------------:|:----------:|:---------------:|-------------|
| XX-01 | [name] | High/Med/Low | DR1-DR6 | A-E | P1-P11 | `[file path]` |

### Detail for Each Risk

#### XX-01: [Risk Name]

- **Severity**: High/Medium/Low
- **Symptom Pattern**: DR1 (Invalid data passes) / DR2 (Stops midway) / DR3 (Check missing) / DR4 (Inconsistent behavior) / DR5 (Silent data loss) / DR6 (Cross-system divergence)
- **Root Cause Axis**: A (Validation bypass) / B (Value/Type conversion) / C (Logic/Branching gaps) / D (Architecture/Design) / E (Security & Robustness)
- **Problem Pattern(s)**: P1-P11
- **Repository**: [repository-name]
- **Summary**: [Description with explicit "whose/what" subject]
- **How It Occurs**: [Conditions and processing flow that trigger the risk]
- **Where It Fails**: [Point of failure or missing validation]
- **File Paths**:
  - `[file path]` line N: [code description]
- **Affected Business Processes**: PR1-PR5

---

## Part A: Business Process-driven

### Overview
- Risk Count: X items (Existing investigation: Y + Distortion survey: Z)
- Target Processes: PR1-PR5
- Repository: [repository-name]
- Target Area: [area name]

### Risk Count by Process

| Process | High | Med | Low | Total |
|---------|:----:|:---:|:---:|:-----:|
| PR1 ... | ... | ... | ... | ... |
| PR2 ... | ... | ... | ... | ... |
| PR3 ... | ... | ... | ... | ... |
| PR4 ... | ... | ... | ... | ... |
| PR5 ... | ... | ... | ... | ... |
| **Total** | **X** | **Y** | **Z** | **N** |

### Risk Details by Process

#### PR1: [Process Name]
(Risk details. Distortion-discovered and existing investigation risks listed without distinction)

#### PR2: [Process Name]
...

---

## Part B: Root Cause-driven

### Overview
(Same risk count as Part A. Only the perspective differs)

### Risk Count by Problem Pattern

| Pattern | Description | High | Med | Low | Total |
|---------|------------|:----:|:---:|:---:|:-----:|
| P1 | Implicit shared flag dependency | ... | ... | ... | ... |
| P2 | Constant/Enum mismatch | ... | ... | ... | ... |
| P3 | Type comparison traps | ... | ... | ... | ... |
| P4 | Soft-delete/JOIN condition gaps | ... | ... | ... | ... |
| P5 | Implicit value conversion | ... | ... | ... | ... |
| P6 | Insufficient branching | ... | ... | ... | ... |
| P7 | Non-idempotent concurrent operations | ... | ... | ... | ... |
| P8 | Unreplayable state transitions | ... | ... | ... | ... |
| P9 | Async boundary gaps | ... | ... | ... | ... |
| P10 | External dependency single points of failure | ... | ... | ... | ... |
| P11 | Data context loss | ... | ... | ... | ... |
| **Total** | | **X** | **Y** | **Z** | **N** |

### Risk Count by Root Cause Axis

| Axis | Description | High | Med | Low | Total |
|------|------------|:----:|:---:|:---:|:-----:|
| E | Security & Robustness defects | ... | ... | ... | ... |
| D | Architecture & Design flaws | ... | ... | ... | ... |
| C | Logic/Branching gaps | ... | ... | ... | ... |
| B | Value/Type conversion defects | ... | ... | ... | ... |
| A | Validation bypass | ... | ... | ... | ... |
| **Total** | | **X** | **Y** | **Z** | **N** |

### Risk Count by Symptom Pattern

| Pattern | Description | High | Med | Low | Total |
|---------|------------|:----:|:---:|:---:|:-----:|
| DR1 | Invalid data passes | ... | ... | ... | ... |
| DR2 | Stops/crashes midway | ... | ... | ... | ... |
| DR3 | Check missing entirely | ... | ... | ... | ... |
| DR4 | Inconsistent behavior | ... | ... | ... | ... |
| DR5 | Silent data loss | ... | ... | ... | ... |
| DR6 | Cross-system divergence | ... | ... | ... | ... |
| **Total** | | **X** | **Y** | **Z** | **N** |

### Pattern Details & Remediation Approach

#### P1: Implicit Shared Flag Dependency
**Root Cause(s)**: [analysis of which axes A-E are involved]
**Symptom Pattern(s)**: [which DR1-DR6 patterns manifest]
**Affected Risks**:
- **[XX-01]** [risk name] [severity]
  - Root Cause Axis: [A-E]
  - Symptom Pattern: [DR1-DR6]
  - Repository: [repository-name]
  - File Path: `[file path:line number]`

**Remediation Approach**:
1. [specific remediation content]

...

### Remediation Priority

| Priority | Remediation | Resolved Risks | Effort | ROI |
|:--------:|------------|---------------|:------:|:---:|
| **1** | [content] | XX-01 (High) | S/M/L | Highest/High/Med/Low |
| **2** | [content] | XX-02 (High) | S/M/L | High |

---

## Part C: Remediation Impact-driven

### Remediation Class Mapping

| Remediation ID | Description | Class | Resolved Risks | Side Effects | Constraints |
|:------:|---------|:--------:|-----------|--------|---------|
| FIX-01 | [description] | Quick / Planned / Architecture / Deferred | XX-01 (High) | None / [Impact System] | None / After FIX-XX |

### Remediation Priority Matrix

| Priority | Remediation ID | Remediation Description | Resolved Count | Difficulty | ROI |
|:------:|:------:|---------|:--------:|:---------:|:---:|
| **1** | FIX-01 | [description] | N items | Low/Medium/High | Highest/High/Medium/Low |

### Remediation Ordering Constraints

[Draw remediation dependencies using mermaid flowchart]

## Visualization (Supplementing Part A/B/C)

### 1. Processing Flow Diagram (Risk Occurrence Point Mapping)

[Draw target area processing flow in mermaid flowchart. Color-code risk occurrence points by severity]
- High severity: Red (fill:#e74c3c)
- Medium severity: Orange (fill:#f39c12)
- Low severity: Gray (fill:#95a5a6)

### 2. Root Cause → Problem Pattern → Symptom Causality Diagram

[Draw multi-level causality diagram in mermaid flowchart]
- Root Cause Axes (A-E) → Problem Patterns (P1-P11) → Symptom Patterns (DR1-DR6) → User Impact

### 3. Remediation Impact Diagram (Fix Points and Resolved Risks)

[Draw relationship between each fix and directly/indirectly resolved risks in mermaid flowchart. Show dependencies between fixes]

---

## Remaining Investigation Items

1. [Items not fully investigated]
2. [Items requiring additional investigation]

---

## Next Actions

### Remediation Priority
1. **[Highest]** [remediation content] ([resolved risk IDs])
2. **[High]** [remediation content] ([resolved risk IDs])

### Items Requiring User Decision
1. [Items requiring business judgment]
```

---

## Input Parameters

| Parameter | Description | Required/Optional | Default | Example |
|-----------|------------|-------------------|---------|---------|
| Phase | `Phase 1`, `Phase 2` | Optional | All-in-one when omitted | `Phase 1` |
| Repository Name | Target repository | Required for Phase 1 | -- | `your-backend-repo`, `your-ec-repo` |
| Target Area | Target functional area | Optional | All areas when omitted | `order-delivery`, `checkout-flow`, `pricing` |
| --discussion | Discussion number | Optional | -- | `--discussion 42` |

### Target Area Specification Rules

- **When omitted**: The entire specified repository becomes the investigation target. If the repository is large, consulting the planner for scope partitioning is recommended
- **When specified**: Specify by business domain or functional unit
  - Format examples: `order-delivery`, `checkout-flow`, `pricing`, `print-integration`, `incentive-payments`, `authentication`
  - Direct specification of directories or classes within the repository is also acceptable (e.g., `app/Services/Order/`)
- **When ambiguous**: Request clarification from the leader: "The target area is ambiguous; please select from the following candidates." Do not determine scope by guessing

### --discussion Option

When a Discussion number is specified, a report summary is automatically posted to the GitHub Discussion upon completion.

```
/current-distortion your-ec-repo checkout-flow --discussion 42
```

Post content:
- Investigation overview (target, count, severity distribution)
- Remediation priority table (core of Part B)
- Link to integrated report file

---

## Cross-Cutting Risk View

When distortion analysis covers multiple repositories, risks that span repository boundaries require special attention. The **Cross-Cutting Risk View** provides a two-layer management approach:

### L1: Component-Level View
- Risks within a single repository/component
- Managed within each distortion report

### L2: Cross-Cutting View
- Risks that span multiple repositories
- Shared flags referenced differently across repos
- Data flow integrity across service boundaries
- Inconsistent validation between frontend and backend

**When to create L2 view**: After completing L1 analyses for 2+ repositories in related areas, synthesize cross-cutting risks into a separate section or dedicated report.

---

## Related Skills

| Skill | Relationship |
|-------|-------------|
| `/current-legacy` | Discovers risks during change-theme investigation -> `/current-distortion` organizes systematically |
| `/current-spec` | Individual code investigation -> `/current-distortion` Phase 1 for distortion-focused investigation |
| `/change-impact` | Change impact analysis -> Part B remediation priority judgment benefits from this |
| `/review-code` | **Complementary relationship**: `/review-code` performs per-PR differential review (dynamic, tactical). `/current-distortion` detects structural distortions across a domain (static, strategic). When distortions are discovered during code review, delegate systematic analysis to `/current-distortion` |

### Skill Chain Examples

```
# From legacy analysis to distortion analysis
/current-legacy Phase 1 order-delivery  ->  /current-distortion your-backend-repo order-delivery

# From individual investigation to distortion analysis
/current-spec ShoppingCartService  ->  /current-distortion your-ec-repo checkout-flow

# From distortion analysis to impact analysis
/current-distortion your-backend-repo pricing  ->  /change-impact price-pattern-fix
```

---

## Reference Files

| File | Purpose |
|------|---------|
| `knowledge/domain/business_processes.md` | Business process definitions. Mapping target for Part A |
| `knowledge/system/overview.md` | Project structure. Understanding cross-repository relationships |
| `knowledge/system/repositories.md` | Repository responsibilities. Judgment for investigation scope |
| `reports/` | Past investigation reports. Source for Phase 1 Worker A to extract existing risks |

---

## I/O Specification

### INPUT

| Type | Content | Required/Optional | Example |
|------|---------|-------------------|---------|
| Phase | Phase 1, Phase 2, or omitted (All) | Optional | `Phase 1`, omitted |
| Repository Name | Target repository | Required for Phase 1/All | `your-backend-repo`, `your-ec-repo` |
| Target Area | Target functional area | Optional | `checkout-flow`, `order-delivery` |
| --discussion | GitHub Discussion number | Optional | `--discussion 42` |

### OUTPUT

| Type | Format | Destination |
|------|--------|-------------|
| Integrated Report | Markdown (distortion results + Part A/B/C) | `reports/distortion-report-[repo]-[area]-[date].md` |

### Prerequisites

- Serena MCP is running
- Target repository is accessible
- For Phase 2 only: Leader holds Phase 1 integration results
- Business process definitions exist in knowledge/domain/ (required for Part A mapping)

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|------------|
| `/change-impact` | When Part B remediation priority 1 is confirmed | Execute after Part C completion; obtain leader approval before proceeding |
| `/create-pr --plan` | When Quick Fix distortions exist | Propose to leader; seek approval before execution |

> **User Approval Required**: Do not proceed to next phase for Architecture Fix distortions without explicit user decision

> **Fallback**: If prerequisites are not met, report to leader and await further instruction

### Quality Checklist

- [ ] All risks include file path:line number
- [ ] All risks have symptom pattern (DR1-DR6), root cause axis (A-E), and problem pattern (P1-P11) assigned
- [ ] Root cause axis classification follows priority: E → D → C → B → A
- [ ] Flag/variable descriptions explicitly state "whose/what" (Subject-First Rule)
- [ ] Part A: All risks mapped to business processes
- [ ] Part B: Root cause analyses completed for each pattern; remediation priority table with ROI created
- [ ] Part C: Remediation class mapping, priority matrix, ordering constraints, and side effect analysis included
- [ ] Visualization: 3 mermaid diagrams included (processing flow, causality chain, remediation impact)
- [ ] Risk counts match across Part A/B/C (same risk set, different perspectives)
- [ ] P7-P10 risks (concurrency, async, external dependencies) explicitly identified with root cause mapping
- [ ] Remaining investigation items and next actions documented
- [ ] Findings verified against actual code (no hallucinations)
- [ ] Part C "WHAT TO DO" view is decision-maker-oriented with clear remediation effort/impact tradeoffs
- [ ] All Phase 1 risks are deduplicated and source-tracked (existing investigation vs distortion survey)
- [ ] Each risk's file paths point to verified code locations (no assumptions)
- [ ] Negative assertions ("does not check") are supported by absence evidence (grep results showing 0 matches)

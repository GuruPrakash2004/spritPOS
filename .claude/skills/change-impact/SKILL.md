---
name: change-impact
description: Systematically investigate the scope of impact when adding new features or changing specifications, creating an impact analysis report. Use in combination with Service specification summary.
argument-hint: <Description of changes>
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# Impact Analysis Skill

## Overview

Systematically investigate the scope of impact when adding new features or changing specifications, and create an impact analysis report.

## Roles and Responsibilities

| Role | Responsibility |
|------|----------------|
| **Shogun (General)** | Determine analysis targets, approve analysis results, judge response strategy |
| **Karo (Chief Retainer / Planner)** | Task decomposition and selection of investigation scope for large-scale analyses |
| **Ashigaru (Worker)** | Execute code investigation, impact range identification, and report creation |
| **Metsuke (Inspector)** | Quality audit of analysis results (verification of impact range coverage, false positive detection) |

**Delegate-mode rule**: Shogun (General) must not conduct impact analysis directly. Consult with Karo (Planner) for large tasks, delegate execution to Ashigaru (Worker), and request quality audit from Metsuke (Inspector).

### Communication Style

Clear, business-appropriate reporting format. Provide explicit responses for OK/NG judgments and clarify areas of uncertainty.

## Investigation Target

$ARGUMENTS

## Investigation Flow

```
1. Summarize change overview
   |
2. Identify impact scope (files, tables list)
   |
3. Track static dependencies
   - Use Serena find_referencing_symbols to exhaustively trace callers and call sites
   - Verify Call Hierarchy in both upstream and downstream directions
   |
4. Confirm dynamic dependencies
   - Check DI binding (Service binding / container)
   - Identify runtime behavior branching from configuration files
   - Detect reflection and dynamic class resolution
   |
5. Trace data flow
   - Track "entry → processing → exit" for changed variables/columns
   - Identify final destination (DB column, external API, S3, email, etc.)
   - Record intermediate type conversions and value transformations
   |
6. Classify impact level (High/Medium/Low)
   |
7. Detailed investigation of high-impact areas (Service specification summary)
   |
8. Organize risks and concerns
   |
9. Present next action (handoff to /create-pr --plan)
```

## Impact Level Judgment Criteria

### A. Dependency Analysis

| Analysis Axis | Confirmation Item | Method |
|--------|---------|------|
| Static dependencies | Function/class/variable references (Call Hierarchy) | Serena find_referencing_symbols, Grep |
| Dynamic dependencies | DI binding, configuration file branching, reflection | config files, ServiceProvider, environment files |
| Data flow | How changed variables are processed and ultimately reach DB columns/external APIs | Variable tracking (entry -> processing -> exit) |

### B. Contract and Non-Functional Impacts

| Impact Axis | Verification Perspective | Example of High Impact |
|--------|---------|-----------|
| API contracts | Do endpoint/request/response types change? | Field deletion, type change, URL change |
| DB schema contracts | Do table definitions, column types, constraints change? | Adding NOT NULL, type change, column deletion |
| Common component side effects | Does modification break expected behavior of all dependent features? | Change in return value meaning, added parameters |
| Performance | Does N+1 query, unused index, or heavy query occur? | List-based API, batch processing, bulk data operations |
| Authorization / Security | Are permission checks omitted? | New endpoints, common middleware changes |

### C. Overall Impact Level (Final Judgment)

| Impact Level | Criteria | Action |
|--------|------|------|
| High | Code modifications are required. API/schema contracts change. Or affects non-functional requirements | Detailed investigation required. Plan modifications via `/create-pr --plan` |
| Medium | Confirmation needed. Dynamic dependencies exist. Common component side effect risk | Runtime verification required |
| Low | Auto-responsive. Impact is isolated, contracts unchanged | Confirmation only |

## Output Format

```markdown
# [Feature Name] Impact Analysis Report

## ADR (Architecture Decision Record)

### Decision

[Summarize the change in concise terms]

### Background

[Reason for the change, business requirements]

### Technical Decision

[Architecture judgment and rationale]

---

## Impact Scope Analysis

### Impact Level Summary

| Impact Level | Count | Percentage |
|--------|------|------|
| High (Modification Required) | X | XX% |
| Medium (Requires Verification) | X | XX% |
| Low (Auto-responsive) | X | XX% |

### Impact by Category

#### 1. Database

| Table Name | Change | Impact Level | Notes |
|-----------|---------|--------|------|
| xxx | Column addition | High | ... |

#### 2. Model/Enum

| File | Change | Impact Level | Notes |
|---------|---------|--------|------|
| xxx.php | Constant addition | High | ... |

#### 3. Service/UseCase

| File | Change | Impact Level | Notes |
|---------|---------|--------|------|
| xxxService.php | Method modification | High | ... |

#### 4. Batch Processing

| Batch Name | Change | Impact Level | Notes |
|---------|---------|--------|------|
| xxxBatch | ... | Medium | ... |

---

## High-Impact Areas: Detailed Analysis

### [Service Name 1]

※ Record in Service specification summary format (use `/current-spec` skill)

---

## Risks and Concerns

### High Risk (Must be resolved before implementation)

| Item | Details | Confirmation Point | Deadline |
|------|------|--------|------|
| xxx | ... | Related department | Before Phase X |

### Medium Risk (Confirm during implementation)

| Item | Details | Confirmation Point |
|------|------|--------|
| xxx | ... | ... |

### Low Risk (Can be addressed through post-implementation verification)

| Item | Details |
|------|------|
| xxx | ... |

---

## Next Steps

If modifications are necessary based on impact analysis results, create a concrete modification proposal via `/create-pr --plan`.

---

## Change History

| Version | Date | Content |
|-----------|------|------|
| 1.0 | YYYY-MM-DD | Initial version |
```

## Quality Check

### 1. Completeness
- [ ] All repositories investigated
- [ ] DB, Model, Service, batch coverage complete
- [ ] Hardcoded locations identified

### 2. Accuracy
- [ ] Actual code verified (not assumptions)
- [ ] File path:line numbers recorded
- [ ] Cross-repository differences verified

### 3. Practical Utility
- [ ] Impact level judgment criteria clear
- [ ] Modification content specific
- [ ] Confirmation points and deadlines specified

### 4. Service Specification Summary
- [ ] Created specification summary for high-impact Service/UseCase
- [ ] Includes all four items: role, features, dependencies, notable items

## Output Template

### Risk Registration Template

Record risks and concerns discovered during impact analysis in the following template for tracking management.

| ID | Risk Description | Discovery Date | Impact Level | Status | Mitigation Strategy |
|----|----------|--------|--------|---------|--------|
| R001 | [Risk description] | YYYY-MM-DD | High/Medium/Low | Unaddressed/In Progress/Resolved | [Mitigation strategy] |

**Example**:

| ID | Risk Description | Discovery Date | Impact Level | Status | Mitigation Strategy |
|----|----------|--------|--------|---------|--------|
| R001 | Migration script for existing data not updated (Enum structure change) | 2026-03-15 | High | In Progress | Create migration script (Phase 0 planned) |
| R002 | External integration partner specification change verification needed | 2026-03-15 | Medium | Unaddressed | Verification planned by 2026-03-20 |
| R003 | Potential test failure (price calculation test) | 2026-03-16 | Medium | In Progress | Test fix proposal created |

---

## I/O Specification

### INPUT
| Type | Content | Required/Optional | Example |
|------|------|-----------|-----|
| Changes | Description of features to add/modify | Required | `Add new category`, `Change price calculation logic` |
| Prior investigation | Output from `/current-spec` | Optional | Refer to investigation report |

### OUTPUT
| Type | Format | Destination |
|------|------|--------|
| Impact analysis report | ADR format Markdown (impact file list, risks, implementation plan) | stdout (report to Shogun (General)) |

### Prerequisites
- Change specification must be understood (pre-investigation with `/current-spec` recommended)
- knowledge/domain/ files must be accessible for domain context

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|--------|------|------|
| `/create-pr --plan` | When high-impact items exist (1 or more) | After saving impact analysis report, report to Shogun (General) and await approval before executing `/create-pr --plan` |
| `/doc-check` | When knowledge/ updates needed | Propose to Shogun (General) and await judgment |

> **Required**: Even when impact analysis result is "no modifications needed", report results to Shogun (General) and await judgment
>
> **Fallback**: If prerequisites are not met, report to Shogun (General) and await further instructions

### Quality Checkpoints
- [ ] Impact file includes path:line_number
- [ ] Impact level classified (High/Medium/Low)
- [ ] Risks and concerns organized
- [ ] Next action (handoff to `/create-pr --plan`) clearly marked
- [ ] Static callers (Call Hierarchy) traced
- [ ] Dynamic dependencies from DI binding / configuration files confirmed
- [ ] Final destination of changed variables identified (DB column, external API)
- [ ] REST API endpoint and type changes verified
- [ ] DB schema change impact across multiple repositories confirmed
- [ ] Performance and authorization impacts assessed

## Effort Setting

Impact scope investigation typically uses `/effort xhigh`.

Reason: Exhaustively tracing change propagation requires high reasoning capacity to avoid oversights, and such oversights directly translate to rework costs. Particularly effective in:
- DB schema change impact (cross-repository scope)
- Shared Service modification (exhaustive caller tracing)
- Flag/status value change (complete identification of reference locations)

For small-scale changes (single file, isolated modifications), `/effort high` is acceptable.

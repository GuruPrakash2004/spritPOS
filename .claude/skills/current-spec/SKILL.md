---
name: current-spec
description: Systematically investigate Service/UseCase/Model specifications and organize them in Service specification summary format. Combines investigative discovery with detailed specification documentation. Deploy scouts (Explore agents) for rapid initial investigation.
argument-hint: <Service/UseCase/Model name> [repository name]
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# Service Specification Summary Skill

## Overview

Investigate the specification of the specified Service/UseCase/Model and organize it in a unified format. This skill combines rapid investigative discovery (scout mode) with comprehensive detailed specification documentation.

## Roles and Responsibilities

| Role | Responsibility |
|------|----------------|
| **Shogun (General)** | Select investigation targets, delegate to Ashigaru (Worker), consolidate reports. Does not read code directly (delegate-mode rule) |
| **Karo (Chief Retainer / Planner)** | Task decomposition and strategy for large-scale investigations |
| **Ashigaru (Foot Soldier / Worker)** | Execute actual code investigation and create specification summaries |
| **Metsuke (Inspector)** | Quality audit of deliverables (hallucination detection, symbol existence verification) |

> **Delegate-mode rule**: Shogun (General) must not read code directly. All investigation and analysis must be delegated to Ashigaru (Foot Soldier / Worker).

### Communication Style
Use clear, business-appropriate language. Be explicit about what is confirmed vs. uncertain.

## Investigation Target

$ARGUMENTS

## Investigation Procedure

1. **Context check**: Review CLAUDE.md to understand project overview, repository structure, and tech stack
2. **Domain knowledge check**: Check domain knowledge files under knowledge/domain/ to understand knowledge related to the investigation target
   - Check the file list under knowledge/domain/
   - Read files related to the task keywords
   - Start investigation with understanding of known business rules and constraints
   
   #### Domain Terminology Pre-check (Optional / When terminology is ambiguous)
   
   If terminology interpretation is ambiguous before code investigation, perform the following pre-checks:
   - `knowledge/domain/domain_vocabulary.md` — Domain terminology glossary
   - If term meaning is unclear, consult the glossary before starting investigation instead of making assumptions

3. **Target file identification**: Identify file paths using Serena's find_symbol
4. **Symbol overview retrieval**: Get method list using get_symbols_overview
5. **Dependency investigation**: Identify callers/callees using find_referencing_symbols
6. **Survival check**: Verify the investigation target is still actively used

   | Check Item | Method | Criteria |
   |------------|--------|----------|
   | Caller existence | Serena find_referencing_symbols | At least 1 reference must exist. If 0, record as "dead code candidate" |
   | Last commit date | `git log -1 --format=%ci [file_path]` | If >1 year since last update, record as "low activity". Record fact only — do not propose deletion |
   | Runtime usage | Check route definitions, cron schedules, queue registrations | May be used at runtime even without static references |

   **Note**: Dead code candidate / low activity judgments are factual records only. Do not propose deletion or refactoring (state-comprehension phase rule).

7. **Detailed reading**: Read only important method bodies using find_symbol
8. **Diff check**: If the same class exists in multiple repositories, check differences

   | Check Item | Method | Criteria |
   |------------|--------|----------|
   | Caller existence | Serena find_referencing_symbols | At least 1 reference must exist. If 0, record as "dead code candidate" |
   | Last commit date | `git log -1 --format=%ci [file_path]` | If >1 year since last update, record as "low activity". Record fact only — do not propose deletion |
   | Runtime usage | Check route definitions, cron schedules, queue registrations | May be used at runtime even without static references |

   **Note**: Dead code candidate / low activity judgments are factual records only. Do not propose deletion or refactoring (state-comprehension phase rule).

8. **Diff check**: If the same class exists in multiple repositories, check differences

## Output Format

```markdown
# [Service/UseCase Name] Specification Summary

**File Path**: `app/Services/xxx/xxxService.php`
**Repository**: [Target repository (each repository if multiple)]
**Last Investigation Date**: YYYY-MM-DD

---

## 1. Role (Responsibility)

[Explain the responsibility of this Service/UseCase in 1-2 sentences]

**Callers**:
- [Controller/UseCase name]: [Purpose]
- [Batch name]: [Purpose]

---

## 2. Key Feature List

| Method Name | Description | Visibility | Line Numbers |
|-------------|-------------|------------|--------------|
| `methodA()` | ... | public | 45-67 |
| `methodB()` | ... | private | 70-85 |

> **Checkpoint**: After completing the key feature list, report progress to the leader and confirm the investigation scope before proceeding to dependency and side-effect analysis.

> **Checkpoint**: After completing the key feature list, report progress to the leader and confirm the investigation scope before proceeding to dependency and side-effect analysis.

### Key Method Details

#### methodA()
- **Description**: ...
- **Parameters**: `$param1` (type), `$param2` (type)
- **Return value**: type
- **Processing flow**:
  1. ...
  2. ...

---

## 3. Dependent Classes/Modules

### Internal Dependencies (same repository)

| Type | Class Name | Purpose | File Path |
|------|-----------|---------|-----------|
| Model | xxx | ... | app/xxx.php |
| Enum | xxx | ... | app/Enums/xxx.php |

### External Dependencies (other repositories / external services)

| Dependency | Type | Operation | Purpose |
|------------|------|-----------|---------|
| [Dependency name] | DB / API / S3 / Queue / Email etc. | **Read / Write / Both** | [Purpose] |

---

## 4. Notable Items (Legacy Code / Technical Debt)

### Hardcoded Locations

| Line Number | Content | Impact | Improvement Proposal |
|-------------|---------|--------|---------------------|
| 25-30 | Category array | Requires modification when adding new categories | Dynamic retrieval from DB/Enum |

### Technical Debt

| Item | Details | Priority |
|------|---------|----------|
| typo | Typo in `ClassName` | Medium |
| duplication | Processing duplication between xxx and yyy | Low |

### Cross-Repository Differences

| Item | Repository A | Repository B | Response Policy |
|------|-------------|-------------|-----------------|
| xxx | Present | Absent | No action needed |

---

### Cross-Repository Investigation (for features spanning multiple repositories)

When investigating features that do not complete within a single repository, record the following additional items:

| Item | Description |
|------|-------------|
| **Source of Truth** | Which repository's implementation is authoritative |
| **Sync Timing** | Is data synchronization between repositories real-time, batch, or event-driven |
| **Inconsistency Behavior** | Impact and recovery methods when synchronization is delayed or fails |
| **Data Flow** | Pathway: Repository A -> API/DB/Kinesis -> Repository B |

---

## Change History

| Date | Description |
|------|-------------|
| YYYY-MM-DD | Initial version |
```

## Investigation Status Template

When managing multiple investigation targets in parallel, track progress using this template:

| Target | Status | Date | Investigator | Notes |
|--------|--------|------|-------------|-------|
| [Class/Feature name] | Done/Pending/In Progress | YYYY-MM-DD | [Ashigaru (Worker) name] | [Notable findings] |

**Example**:

| Target | Status | Date | Investigator | Notes |
|--------|--------|------|-------------|-------|
| ValidationService | Done | 2026-03-15 | Ashigaru | 3 hardcoded locations found |
| PriceCalculationService | In Progress | 2026-03-16 | Ashigaru | Dependency verification in progress |
| InvoiceService | Pending | - | - | High priority Phase 1 task |

## Quality Checks

Upon investigation completion, verify the following:

- [ ] Confirmed with actual code (not assumptions)
- [ ] Covered all public methods
- [ ] Documented dependencies (DI)
- [ ] Extracted business rules and validations
- [ ] Included file path:line numbers
- [ ] Checked cross-repository differences
- [ ] Identified hardcoded locations
- [ ] (Cross-repo investigation) Documented Source of Truth, sync timing, and inconsistency behavior

## Reporting Guidelines

1. **Distinguish facts from speculation**: Use "X is Y" for confirmed findings; use "X appears to be Y (needs verification)" for unconfirmed items
2. **Include code references**: Always cite in `filename:line_number` format
3. **Be concise and accurate**: Avoid verbose explanations; emphasize key findings

---

## I/O Specification

### INPUT
| Type | Description | Required/Optional | Example |
|------|-------------|-------------------|---------|
| Target class | Service/UseCase/Model name | Required | `OrderService`, `CreateOrderUseCase` |
| Repository | Target repository specification | Optional | `backend`, `frontend` |

### OUTPUT
| Type | Format | Destination |
|------|--------|-------------|
| Service specification summary | Detailed Markdown (method list, dependencies, business rules) | reports/ + stdout (report to Shogun (General)) |

### Prerequisites
- Serena MCP is running
- Target file has been identified (prior investigation with /current-spec recommended)

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/change-impact` | When the task involves changes | After specification summary is complete, report to Shogun (General) and proceed to `/change-impact` upon approval |
| `/create-pr --plan` | For bug fixes or minor changes | If impact analysis is deemed unnecessary, propose to Shogun (General) for judgment |

> **Fallback**: If prerequisites are not met, report to the leader and await further instructions.

### Quality Checkpoints
- [ ] Confirmed with actual code (not assumptions)
- [ ] Covered all public methods
- [ ] Documented dependencies (DI)
- [ ] Extracted business rules and validations
- [ ] Included file path:line numbers
- [ ] Checked cross-repository differences
- [ ] Identified hardcoded locations
- [ ] (Cross-repo investigation) Documented Source of Truth, sync timing, and inconsistency behavior

## Effort Setting

For large-scale specification investigations (cross-service investigation, tracking 10+ file dependencies), using `/effort xhigh` is recommended.

Reason: High reasoning capacity is effective for exhaustive dependency tracking and hallucination prevention. Particularly improves accuracy in tracking DI bindings, configuration file branching, and dynamic dependencies via reflection.

For typical small-scale investigations (single Service, 5 files or fewer), `/effort high` is sufficient.

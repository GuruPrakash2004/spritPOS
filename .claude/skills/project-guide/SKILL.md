---
name: project-guide
description: Present a reference guide for project documentation tailored to the task. Used at mission start and during impact investigation.
argument-hint: <task overview>
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# /project-guide Skill

## Overview

A guide skill that analyzes the nature of the task and presents the optimal reference order from knowledge/system/ and knowledge/domain/.
Helps the team reach "the knowledge needed for the current mission" as quickly as possible.

See config/terminology.md for term customization

## Role Assignment

| Role | Responsibility |
|------|----------------|
| **Shogun (Leader)** | Triggers the skill and receives the report. Does not read code directly (F002 strictly enforced) |
| **Karo (Strategist)** | For large-scale tasks, evaluates the validity of the reference order before task decomposition |
| **Ashigaru (Worker)** | Executor of this skill. Determines task type and generates/reports the reference guide |
| **Metsuke (Inspector)** | After deliverables are produced, verifies that referenced files actually exist and detects hallucinations |

> **F002 strictly enforced**: Shogun does not execute this skill personally. Consult Karo for planning, delegate execution to Ashigaru, and request quality audit from Metsuke.

### Tone
Use a clear, business-appropriate reporting style. State OK/NG clearly, and report uncertainties explicitly as "unknown".

## Usage

/project-guide [task overview]

### Generic Usage Examples (applicable to any legacy system)

| Example | Task Type |
|---------|-----------|
| /project-guide Order processing bug investigation | Bug fix |
| /project-guide Adding a new API endpoint | New feature addition |
| /project-guide External API specification change response | Specification change |
| /project-guide Understanding batch processing specifications | Investigation & analysis |
| /project-guide Local development environment setup | Environment setup |
| /project-guide Consolidating duplicate code | Refactoring |

### Project-Specific Usage Examples

For project-specific usage examples, keyword mappings, and notes, see `PROJECT_EXAMPLES.md`.
Create this file per project to document domain-specific settings.

## Investigation Target

$ARGUMENTS

## Execution Steps

### Step 1: Determine Task Type

Classify the task overview into one of the following:

| Type | Determination Criteria |
|------|----------------------|
| New feature addition | Category addition, screen addition, API addition, etc. |
| Bug fix | Error correction, defect resolution |
| Specification change | Rule changes, external API changes |
| Investigation & analysis | Impact investigation, specification understanding, code reading |
| Environment setup | Local development environment, Docker, DB |
| Refactoring | Code improvement, technical debt resolution |

### Step 2: Generate Reference Guide

Output in the following format:

```
# Project Guide - [Task Overview]

## Recommended Reference Order

### Phase 1: System Understanding (required reading)
[Always presented regardless of task type]
1. knowledge/system/overview.md - Project overview
2. knowledge/system/repositories.md - Repository responsibility boundaries

### Phase 2: Task-Specific Knowledge
[Dynamically selected based on task type]

### Phase 3: Implementation Preparation
[Presented only when applicable]

### Phase 4: Impact Analysis
[Presented only when changes are involved]

## Related Domain Knowledge
[Present applicable files from knowledge/domain/]

## Notes
[Extract relevant notes from environment.md "common pitfalls"]
```

**Hallucination Prevention Check (before proceeding to Step 3)**:
1. Skill existence verification: Run `ls .claude/skills/` to confirm each skill folder actually exists
2. Reference document existence verification: Run `grep -n '[section name]' knowledge/README.md` to confirm referenced sections exist
3. Reference file existence verification: Run `ls [file path]` to confirm each referenced file actually exists
4. Do not include non-existent skills or files in recommendations

**Prohibitions**:
- Do not write skill names by assumption (do not present skills not found in `ls .claude/skills/` results)
- Do not include non-existent files as references (exclude paths that cannot be verified via `ls`)
- Do not conclude "does not exist" without final grep verification (try at least 2 keyword variations before giving up)

### Step 3: Skill Selection Guide by Task Type

| Task Type | Recommended Skills | Purpose |
|-----------|-------------------|---------|
| **New feature addition** | `/current-spec` -> `/change-impact` -> `/create-pr --plan` -> `/create-pr --exec` -> `/review-code` | Spec understanding -> impact analysis -> change proposal -> PR creation -> review |
| **Bug fix** | `/current-spec` -> `/create-pr --plan` -> `/create-pr --exec` -> `/review-code` | Root cause investigation -> change proposal -> PR creation -> review |
| **Specification change** | `/current-spec` -> `/change-impact` -> `/create-pr --plan` -> `/create-pr --exec` -> `/review-code` | Current spec -> impact analysis -> change proposal -> PR creation -> review |
| **Investigation & analysis** | `/current-spec` -> `/change-impact` | Spec investigation -> impact range identification |
| **Environment setup** | Reference knowledge/runbooks/ | Per-repository setup procedures |
| **Refactoring** | `/current-distortion` -> `/create-pr --plan` -> `/create-pr --exec` -> `/review-code` | Distortion analysis -> change proposal -> PR creation -> review |
| **Legacy code understanding** | `/current-legacy` | Stepwise understanding (growing 3 maps) |
| **PRD creation** | `/current-prd` | Reverse-generate spec from existing code |
| **Documentation maintenance** | `/doc-check` -> `/doc-organize` -> `/doc-update` | Diagnose -> organize -> maintain |

**Reference document details**: See `knowledge/README.md` "FAQ and References" and "Navigation Guide" sections.

#### Detailed Phase Mapping by Task Type

#### New Feature Addition
- **Phase 2**: environment.md (business rules) -> service_responsibilities.md (related Services) -> tech_stack.md (technical constraints)
- **Phase 3**: typical_change_patterns.md (check similar patterns) -> schema_database.md (DB verification)
- **Phase 4**: impact_analysis_template.md -> impact_analysis_example.md

#### Bug Fix
- **Phase 2**: service_responsibilities.md (identify cause location) -> environment.md (verify business rules)
- **Phase 3**: schema_database.md (data verification) -> non_functional_requirements.md (constraint verification)
- **Phase 4**: None (when fix scope is limited)

#### Specification Change
- **Phase 2**: environment.md (current specification) -> service_responsibilities.md (affected Services) -> diagrams/data_flow.md (data flow)
- **Phase 3**: typical_change_patterns.md -> schema_database.md
- **Phase 4**: impact_analysis_template.md -> impact_analysis_example.md

#### Investigation & Analysis
- **Phase 2**: service_responsibilities.md -> diagrams/system_overview.md -> diagrams/data_flow.md -> diagrams/io_interfaces.md
- **Phase 3**: schema_database.md -> environment.md
- **Phase 4**: None

#### Environment Setup
- **Phase 2**: local_dev.md -> tech_stack.md
- **Phase 3**: None
- **Phase 4**: None
- **Note**: Reference detailed knowledge under knowledge/local_dev/

#### Refactoring
- **Phase 2**: service_responsibilities.md (verify responsibilities) -> non_functional_requirements.md (constraints) -> todo_list.md (known issues)
- **Phase 3**: typical_change_patterns.md -> schema_database.md
- **Phase 4**: impact_analysis_template.md

### Step 4: Domain Knowledge Mapping

Present relevant files from knowledge/domain/ based on keywords in the task overview.

**Search Procedure (3-stage fallback)**:
1. Open knowledge/README.md and search for the task keyword
2. 1+ matches: Present the matching files as reference targets
3. 0 matches (1st fallback): Full-text search knowledge/domain/ (`grep -rn '[keyword]' knowledge/domain/`)
4. Still 0 matches (2nd fallback): Full-text search knowledge/system/ (`grep -rn '[keyword]' knowledge/system/`)
5. Still 0 matches: Report "No matching domain knowledge found. Searched domain/ and system/ with keyword '[keyword used]'"

**Prohibitions**:
- Do not conclude "does not exist" on 0 results immediately. Consider keyword variations (English/native language, abbreviations/full names) and search with at least 2 patterns
- Do not report "probably does not exist" by assumption without searching

**Edge Case Handling**:
| Case | Action |
|------|--------|
| Keyword is ambiguous (e.g., "price") | Try multiple related keywords (price, pricing, cost, wholesale, etc.) |
| Spans multiple domains | List all matching files across domains and present reference order |
| Not in knowledge/ but exists in reports/ | Annotate with "Related file found in reports/ (not yet promoted)" |

**Quality Check**:
- [ ] Are reference file paths fully specified?
- [ ] Were all 3 fallback stages executed for 0-result cases?
- [ ] Were keyword variations considered in searches?

**Project-specific mappings**: Define keyword-to-file mappings in `PROJECT_EXAMPLES.md`.

### Step 5: Extract Notes

Extract task-related notes from the "common pitfalls" section of environment.md.

**Common notes (always presented)**:
- Include project-specific soft-delete conditions (e.g., is_deleted = false) in all queries
- Verify project-specific type safety rules (e.g., PHP strict_types)
- Synchronize duplicate code across multiple repositories

**Project-specific notes**: Document in `PROJECT_EXAMPLES.md`.

## Document Quality Evaluation Integration

After task completion, it is recommended to evaluate documents output to reports/ against the criteria in assessment/legacy_document_quality.md.

**"Ready for next action" 5W Check**:
- **What**: Change target has been identified
- **Where**: File path:line number is known
- **How**: Implementation method has been determined
- **Risk**: Impact scope and side effects have been evaluated
- **Next**: Phase-divided plan exists

## Report Format

```
# Project Guide - [Task Overview]

> **Task Type**: [New feature addition / Bug fix / Specification change, etc.]
> **Created**: YYYY-MM-DD

---

## Recommended Skill Pipeline

1. `/current-spec` -- [purpose]
2. `/change-impact` -- [purpose]
...

### Phase Consumer Table

| Phase | Skill | Output | Consumer |
|-------|-------|--------|----------|
| 1 | /current-spec | spec.md | Input for Phase 2 |
| 2 | /change-impact | impact_analysis.md | Input for Phase 3 |
| 3 | /create-pr --plan | pr_plan.md | Leader's approval decision |
| 4 | /create-pr --exec | PR | Input for Phase 5 |
| 5 | /review-code | review_report.md | Merge decision |

## Recommended Reference Order

### Phase 1: System Understanding (required reading)
1. `knowledge/system/overview.md` - Project overview
2. `knowledge/system/repositories.md` - Repository responsibility boundaries

### Phase 2: Task-Specific Knowledge
1. `knowledge/system/environment.md` - [Reference reason]
2. `knowledge/system/service_responsibilities.md` - [Reference reason]
...

### Phase 3: Implementation Preparation
1. `knowledge/system/typical_change_patterns.md` - [Reference reason]
2. `knowledge/system/schema_database.md` - [Reference reason]
...

### Phase 4: Impact Analysis
1. `knowledge/system/impact_analysis_template.md` - [Reference reason]
2. `knowledge/system/impact_analysis_example.md` - [Reference reason]

---

## Related Domain Knowledge

- `knowledge/domain/xxx.md` - [Relevance reason]

---

## Notes

### Common (required)
- Include project-specific soft-delete conditions (e.g., is_deleted = false) in all queries
- Project-specific type safety rules (e.g., PHP strict_types) required
- Synchronize duplicate code across multiple repositories

### Task-Specific
- [Task-specific notes extracted from environment.md]
```

## Prohibited Actions

- Do not specify documents by assumption (present only after confirming knowledge/ contents)
- Do not modify files (guide presentation is the primary mission)

---

## I/O Specification

### INPUT
| Type | Description | Required/Optional | Example |
|------|-------------|-------------------|---------|
| Task overview | Description of the task to be performed | Required | `Order processing bug investigation`, `New API endpoint addition` |

### OUTPUT
| Type | Format | Destination |
|------|--------|-------------|
| Reference guide | Document reference order and related file list | stdout (report to leader) |

### Prerequisites
- Documents in knowledge/system/ exist
- Domain knowledge in knowledge/domain/ exists

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| First skill in recommended pipeline | When task type is determined | Report the recommended pipeline to Shogun, obtain approval, then execute the first skill |
| `/current-spec` | When detailed investigation is needed | Detailed investigation and specification of targets identified by the guide |
| `/change-impact` | When change is involved | Analyze impact scope identified by the guide |

> **Fallback**: If the recommended pipeline is unclear, report to Shogun and await further instructions.

### Quality Checkpoints
- [ ] Correctly classified task type
- [ ] Reference order has logical rationale
- [ ] Presented all related domain knowledge without omission

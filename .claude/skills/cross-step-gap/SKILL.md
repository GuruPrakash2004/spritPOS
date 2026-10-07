---
name: cross-step-gap
description: Detect inter-step dependency gaps from map documents. Systematically discover patterns that are correct within a single step but break down when viewed across the before-after relationship.
argument-hint: "[target-scope: step-range / flag-name / table-name]"
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# /cross-step-gap -- Inter-Step Gap Detection Skill

## Overview

Systematically detect inconsistencies where processing is correct within a single step but breaks down when crossing before-after relationships (preconditions and handoffs to downstream steps), using map documents as input.

## Distinction from /current-distortion

| Aspect | /current-distortion | /cross-step-gap |
|--------|---------------------|-----------------|
| Input | Actual code (PHP/SQL/config) | Map documents (business-flow/data-flow docs) |
| Investigation unit | File / function / class | Step (business process step) |
| Detection target | Local problems within code | Before-after dependency breakdowns across steps |
| Nature of output | Empirical facts | Document-grounded hypotheses (code verification recommended) |

### Role Clarification

- **Audit completable from documents alone**: Cross-checking preconditions and postconditions between steps. Detecting inconsistencies within documents.
- **Bridge to code**: Attach "code verification recommended" flags to detected gaps. Hand off to /current-distortion for code-level verification.

### Integration Pipeline

```
/cross-step-gap (document-driven, inter-step gap hypotheses)
  -> verify high-confidence gap candidates in code
/current-distortion (code-driven, implementation-level verification)
  -> once remediation direction is established
/create-pr --plan
```

## Input Materials

This skill requires the following three map documents as input. The user specifies the documents corresponding to the target project.

| Purpose | Required Document Characteristics | Check Items |
|---------|-----------------------------------|-------------|
| **Process definition** | Document describing all business process steps with preconditions and postconditions | Preconditions, postconditions, and dependencies for each step |
| **Data flow** | Document illustrating data inflow/outflow per step in diagram or table format | Table/column dependencies, read-to-write ordering |
| **Attribute/flag mapping** | Document describing which step sets each attribute/flag and which downstream steps reference it | Flag values and downstream branching behavior, mapping between setting steps and referencing steps |

**Example (for [your project])**
- Process definition: `[your-project-business-flow-doc]`
- Data flow: `[your-data-flow-doc]`
- Attribute/flag mapping: `[your-flags-doc]`

## Execution Steps

### Phase 0: Map Document Freshness Check

Check the Last Updated date of the three input documents.

| Criterion | Action |
|-----------|--------|
| Within 30 days | Use as-is |
| 30-90 days | Attach `[FRESHNESS WARNING]` tag to output. Treat results as reference values |
| 90+ days | Report to leader and ask whether to run /doc-update first |

### Phase 1: Expand Inter-Step Before-After Dependency Matrix

1. Extract "preconditions" and "postconditions (primary outputs)" for each step from the **process definition document**
2. Generate a mapping table of "setting step" to "referencing step" for each flag/attribute from the **attribute/flag mapping document**
3. Trace write-to-read dependencies between tables/columns from the **data flow document**

Output: Before-after dependency matrix (steps x flags/tables)

### Phase 2: Gap Detection

Cross-check the before-after dependency matrix against five detection patterns:

## Detection Patterns

### GP-01: Unsatisfied Precondition Handoff

**Definition**: A downstream step assumes "X has been set" as a precondition, but there are cases where the upstream step does not set X (unsetting in some conditional branch paths).

**Distinction from GP-03**: GP-01 is "a path exists where the upstream step does not satisfy the condition" (path coverage issue). GP-03 is "the upstream step checked the condition, but the downstream step does not re-check the same condition" (condition inheritance omission).

### GP-02: Flag Meaning Drift (Setting Step vs. Referencing Step)

**Definition**: The intent of the step that sets a flag differs from the interpretation of the step that references it. Tends to occur when COALESCE or indirect references are involved.

### GP-03: Valid Condition Completed Within Step / Not Inherited by Downstream

**Definition**: Step A filtered for "valid X" and processed accordingly, but when step B subsequently retrieves X, the same filtering condition is not applied.

**Distinction from GP-01**: GP-03 is "the filtering condition does not propagate to downstream steps" (condition inheritance omission). GP-01 is "a path exists where the upstream step does not satisfy the condition in the first place" (precondition failure).

### GP-04: Dependency Chain Cognitive Load Explosion

**Definition**: Multiple flags and tables are serially dependent, and if any one is missing, downstream steps malfunction silently. Detects serial dependencies of 3 or more layers.

### GP-05: Missing State Validation at Async Boundary

**Definition**: There is no integrity check between synchronous and asynchronous steps (batch, etc.), and the completion of the synchronous step does not guarantee the executability of the asynchronous step.

## Output Format

Save destination: `reports/YYYY.MM.DD_cross-step-gap_[scope].md`

```markdown
# Inter-Step Gap Detection Results -- [Scope]

> Date: YYYY-MM-DD
> Input document freshness: [OK / FRESHNESS WARNING / UPDATE REQUIRED]

## Detection Summary

| ID | Pattern | Related Steps | Related Attributes/Tables | Severity | Code Verification |
|----|---------|---------------|--------------------------|----------|------------------|
| GAP-01 | GP-XX | Step X -> Y | [attribute name] | High/Medium/Low | Recommended / Not required |

## Details

### GAP-01: [Title]

**Pattern**: GP-XX
**Related Steps**: Step X ([step name]) -> Step Y ([step name])
**Inconsistency Content**: [Cross-check result of preconditions and postconditions]
**Document Evidence**: [Map document filename:section name]
**Code Verification**: [If recommended, guidance on files/methods to verify]

## Before-After Dependency Matrix (mermaid)

(Visualize dependencies between steps)

## Cognitive Load Map

(Visualize dependency chains where GP-04 applies)
```

## Quality Checklist

- [ ] Was document freshness checked (Phase 0)?
- [ ] Does the before-after dependency matrix cover all steps?
- [ ] Is the distinction between GP-01 and GP-03 not confused (verify definition distinction)?
- [ ] Is output in "hypothesis" form rather than "fix instructions" (State observation phase compliance)?
- [ ] Were "code verification recommended" flags appropriately attached?
- [ ] Was the report saved to reports/ (F006)?

## Prohibited Actions

- Do not write fix suggestions (state observation phase). Use "... is the case" form only.
- Do not supplement with guesses for information not in map documents. If there is no document basis, state "no basis."
- Do not produce findings that duplicate detection patterns (P1-P11) of /current-distortion. Delegate code-level issues to /current-distortion.

## Fallback

- If input documents are 90+ days old -> Report to leader and ask whether to run /doc-update first.
- If the before-after dependency matrix cannot be generated (step preconditions undocumented) -> Report affected steps as "requires investigation." Do not fill in with guesses.
- If no gaps are detected -> Report "none detected." Do not force gap discovery.

## Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/current-distortion` | When gaps with "code verification recommended" flag exist | Specify target files/methods for code verification |
| `/doc-update` | When map document freshness is insufficient | Run document update first |

## I/O Specification

### INPUT

| Type | Content | Required/Optional |
|------|---------|-------------------|
| Three map documents | Documents specified in the Input Materials section (process definition / data flow / attribute/flag mapping) | Required |
| Investigation scope | Step range / flag name / table name | Optional (all steps when omitted) |

### OUTPUT

| Type | Format | Destination |
|------|--------|-------------|
| Gap detection report | Markdown | `reports/YYYY.MM.DD_cross-step-gap_[scope].md` |

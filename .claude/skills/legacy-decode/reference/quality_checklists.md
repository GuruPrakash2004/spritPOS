# Quality Checklists, Team Composition Templates, and I/O Specification Details

> **Created**: 2026-06-01

Phase-specific quality checkpoints, team composition patterns, and input/output specification details.

## Phase 0 Checklist (Karo's Completion Judgment)

Karo executes this checklist and when all items are "confirmed", proceeds to Phase 1.

| Item | Verification Content | Judgment |
|-----|---------|------|
| Target validity | Is the target "requiring blackbox decoding"? Is it not a feature where reverse-engineered specs already exist? | Confirmed / Needs Review |
| Three elements of scope | Are target repositories (primary/reference), target features, and table/API/external integration boundaries explicitly stated? | Confirmed / Incomplete |
| Question list | Are approximately 30 questions listed in priority order? | Confirmed / Insufficient |
| Method selection | Is at least one method from Serena / grep / call hierarchy / DB schema / execution log assigned to each question? | Confirmed / Incomplete |
| Quality gate definition | Are completion conditions (80% criterion) and rejection conditions (5 types of finding_id) documented? | Confirmed / Incomplete |

## Phase 1 Checklist (Digging Completion Judgment)

Ashigaru executes this checklist along with the "digging complete" report.

| Item | Verification Content | Judgment |
|-----|---------|------|
| Serena search executed | Was Serena symbol search performed for API endpoints, function names, and table names? | Complete / Skipped |
| Call Hierarchy traced | Was the call chain traced up to maximum 5 levels from the Entry Point? | Complete / Partial / Not Done |
| Grep cross-search | Was grep performed across all files using function names and table names to confirm gaps and hidden references? | Complete / Partial / Not Done |
| DB schema checked | Were CREATE TABLE definitions, constraints, and FKs of target tables verified? | Complete / Partial / Not Done |
| Soft-delete check | For tables using soft-delete, was the filter status in WHERE clauses recorded? (If applicable to your project) | Complete / Warning noted / N/A |
| Completeness of fact recording | Do all findings have "file:line-number" evidence? Are inferences marked as "estimated"? | Confirmed / Incomplete |

## Phase 2 Checklist (Analysis Completion Judgment)

Ashigaru executes this checklist and reports as "analysis complete".

| Item | Verification Content | Judgment |
|-----|---------|------|
| Data flow diagram created | Was the full flow from input to processing to output expressed in text-based or Mermaid diagram? | Complete / Text only |
| Table operation map | Were operation sources and timing for INSERT / UPDATE / DELETE / SELECT organized in a table? Is soft-delete check column included? | Complete / Incomplete |
| Error handling recorded | Were 3 or more abnormal scenarios (payment failure, timeout, etc.) listed? | Complete / Only 1-2 |
| Current implementation promises | Were the constraints and preconditions created by the current implementation recorded? | Complete / Insufficient |
| New question detection and judgment | Were new questions detected? If so, was a judgment made about re-executing Phase 1? | Judged / Pending |

## Phase 3 Checklist (Metsuke Quality Gate Judgment)

Metsuke executes this checklist and determines one of "passed", "conditionally passed", or "rejected".

| Item | Verification Content | Judgment |
|-----|---------|------|
| Evidence existence confirmed | Was grep / Serena existence verification independently performed for all code line numbers cited by Ashigaru? Did 95% or more exist? | Existence rate __% |
| Answer rate calculated | Of the approximately 30 questions defined by Karo, how many have evidence-backed answers? Is it 80% or more? | Answer rate __% / Passed / Failed |
| Soft-delete compliance | Is the soft-delete filter status documented for all queries operating on tables? (If applicable) | Compliant / Warning / Gap detected / N/A |
| Cycle limit compliance | Is the Phase 1->2->3 loop within 3 times? If exceeded, is the reason recorded? | Normal / Exceeded (reason: ____) |

## Phase 4 Checklist (Scribe Deliverable Creation Completion Judgment)

Scribe executes this checklist and reports as "deliverable creation complete".

| Item | Verification Content | Judgment |
|-----|---------|------|
| Reverse-engineered spec created | Was `knowledge/domain/[feature-area]/reverse_engineered_spec_[feature-name].md` created? | Created / Incomplete |
| Spec format compliance | Does it include all of: frontmatter / overview / input / logic / output / error handling / soft-delete table? | Compliant / Incomplete |
| Technical debt catalog entry | Was a new entry added to `knowledge/domain/technical_debt_catalog.md`? | Added / Newly created / Not needed |
| Migration priority matrix connection | Was the corresponding item plotted/appended to `knowledge/domain/migration_priority_matrix.md`? | Appended / Newly created / Not needed |
| File save confirmed | Are all files saved in the appropriate directory under `knowledge/domain/`? | Confirmed / Save incomplete |

## Team Composition Templates

### Standard Composition (Single Feature, Single Repository)

Estimated total duration: 2-4 hours.

| Phase | Assigned Agent | Responsibility | Estimated Duration |
|-------|-----------------|------|---------|
| **Phase 0** | Karo | Scope definition, approximately 30 questions, investigation plan, quality gate definition | 30-40 min |
| **Phase 1-2** | Ashigaru (ashigaru-investigator) | Digging and analysis | 90-120 min |
| **Phase 3** | Metsuke | Independent audit, rejection judgment | 30-45 min |
| **Phase 4** | Ashigaru (ashigaru-scribe) | Writing spec, catalog, matrix | 45-60 min |

### Large-Scale Composition (Cross-Repository)

Estimated total duration: 6-8 hours.

| Phase | Assigned Agent | Responsibility | Estimated Duration |
|-------|-----------------|------|---------|
| **Phase 0** | Karo | Subtask decomposition of multiple features, scope definition, individual question lists | 60-90 min |
| **Phase 1-2 (parallel)** | Ashigaru A + Ashigaru B | Parallel digging in each repository | 90-120 min each |
| **Phase 2 integration** | Ashigaru A + Ashigaru B | Create one integrated "data flow diagram" across repositories | 30-45 min |
| **Phase 3** | Metsuke | Quality audit of integrated version | 45-60 min |
| **Phase 4** | Ashigaru C (scribe) | Integrated spec, catalog, matrix entries | 60-75 min |

## I/O Specification Details

### I/O Specification (Input/Output File List - Retained in Main Body)

| Phase | Output File | Path |
|-------|-----------|------|
| Phase 1-2 | Digging and analysis report | `reports/investigation/YYYYMMDD_[task-name]_phase2_analysis.md` |
| Phase 3 | Metsuke audit result | Appended to end of same file "§Final. Metsuke Audit Results" |
| Phase 4 | Reverse-engineered spec | `knowledge/domain/[feature-area]/reverse_engineered_spec_[feature-name].md` |
| Phase 4 | Technical debt catalog | `knowledge/domain/technical_debt_catalog.md` |
| Phase 4 | Migration priority matrix | `knowledge/domain/migration_priority_matrix.md` |

### INPUT (Karo input, Phase 0 execution)

| Item | Format | Description |
|------|------|------|
| **Scope definition document** | Markdown | Includes [decoding target] [target repositories] [investigation target tables] [investigation target APIs] [scope boundary] |
| **Question list** | List format | Approximately 30 "Fact Finding questions" in priority order, with investigation method for each |
| **Quality gate definition** | Table / list | Explicitly states completion conditions (80% criterion) and 5 types of rejection conditions |

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated quality checklists, team composition templates, and I/O specification details from SKILL.md |

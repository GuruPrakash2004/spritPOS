---
name: legacy-decode
description: Legacy blackbox decoding skill. Adapts the takt research workflow engine to systematically decode legacy code and generate reverse-engineered specifications, technical debt catalogs, and migration priority matrices.
argument-hint: "Phase 0 [target-repository] | Phase 1 [target-feature] | Phase 2 | Phase 3 | Phase 4"
---

# /legacy-decode -- Legacy Blackbox Decoding Skill

## §0 takt Adaptation Reference

> **Source**: `nrslib/takt` main HEAD `7cd901887...` (2026-05-29)
> **Details**: `reference/takt_mapping.md`

This skill adapts the research engine execution rules of the OSS library `nrslib/takt` to the Shogun/Karo/Ashigaru/Metsuke team structure. The four original domains (loop structure / verdict 3-value convention / facet 5-type synthesis / Loop Detector) and their adaptations are detailed in `reference/takt_mapping.md`.

## Overview

A skill for systematically decoding legacy code blackboxes and generating reverse-engineered specifications, technical debt catalogs, and migration priority matrices. This is an execution procedure that adapts the takt research workflow (plan->dig->analyze->supervise 4-stage loop) to the Shogun/Karo/Ashigaru/Metsuke team structure.

**Key characteristic**: A single cycle is defined as Phase 0 (Karo planning) -> Phase 1-2 (Ashigaru digging and analysis) -> Phase 3 (Metsuke quality gate) -> Phase 4 (Scribe deliverable creation). Multiple cycles progressively grow the legacy map built by `/current-legacy`.

## Roles and Responsibilities

| Role | Assignment | Responsibility |
|------|-----------|----------------|
| **Shogun (General)** | Coordination and judgment | Phase selection, team composition, cycle limit monitoring. Does not dig directly (delegate-mode rule) |
| **Karo (Chief Retainer / Planner)** | Phase 0 | Target selection, scope definition, investigation plan creation, quality gate pre-definition |
| **Ashigaru (Foot Soldier / Worker / investigator)** | Phase 1-2 | Serena search, grep, call hierarchy, fact recording, structuring findings, analysis |
| **Metsuke (Inspector)** | Phase 3 | Independent audit, rejection judgment, finding_id tracking |
| **Ashigaru (scribe)** | Phase 4 | Writing reverse-engineered specs, technical debt catalog, migration priority matrix |

### Delegate-Mode Rule
Shogun must not perform legacy decoding directly. Consult Karo for investigation plans, delegate digging to Ashigaru, and request quality audits from Metsuke.

## Tone

Rules for notation within this skill:

- **Subject-first principle**: When explaining flags, statuses, or variable names, write "whose / what's" subject first. Example: "[your-flag] ([your description])", "soft_delete_flag=0 (that record's deletion flag is 0 = not deleted)".
- **Citation requirement**: All evidence such as code, tables, and file paths must be cited in the format "filepath:line-number" or "table.column". "It appears to be" or "it seems like" are not acceptable. If evidence is uncertain, write "estimated" and state the reason.
- **Separation of inference and fact**: Directly extracted code, logs, and schema are "facts". Pattern inferences or "may be the case" must be labeled as "estimated".
- **No emoji or decorative symbols**: Do not use emoji or symbols (such as checkmarks, warning signs) to indicate severity or pass/fail. Use only text such as "high/medium/low", "complete/pending".

## Prerequisites

Confirm the following prerequisites are met before executing this skill:

- **Serena setup complete**: Symbolic search tool is available (tool name: Serena component in Claude Code).
- **Repository access**: Read/Bash access to the target repositories is available.
- **Understanding of your project's soft-delete convention**: Understand whether your project uses a soft-delete flag (e.g., `soft_delete_flag='0'`, `deleted_at IS NULL`, or equivalent) and that all queries must include the appropriate filter. See your project's coding conventions guide for details. Detailed project-specific conventions are documented in `reference/project_conventions.md`.
- **Karo decomposition plan exists**: The "decoding plan" (scope, questions, quality gate definition) created by Karo in Phase 0 is at hand. This skill always follows Karo's plan.

## Relationship with Existing Skills

### Relationship with `/current-legacy`

- **`/current-legacy`**: The **overall framework** for progressively growing the legacy map across multiple cycles. Repeatedly executes Phase 0 (situation assessment) -> Phase 1 (dig from specifics) -> Phase 2 (knowledge integration) -> Phase 3 (priority judgment) and persists reverse-engineered specs and technical debt catalogs to knowledge/. A milestone-type approach targeting "map growth through multiple deep-dives."
- **`/legacy-decode`**: A **procedure for deeply investigating one feature or one repository**. Completes one cycle of Phase 0 (Karo planning) -> Phase 1 (digging) -> Phase 2 (analysis) -> Phase 3 (quality gate) -> Phase 4 (deliverable creation). Specialized in completing a single "blackbox decoding" task.
- **Positioning**: Functions as a **detailed version** that deep-dives into Phase 1 "dig from specifics" of `/current-legacy`. No conflict.

### Integration with Other Skills

- **`/current-spec` integration**: When summarizing service specifications during Phase 1 digging, run `/current-spec` in parallel to aggregate specification information.
- **`/current-distortion` usage**: When evaluating "technical debt" in Phase 2 analysis, link distortion pattern detection with `current-distortion`.
- **`/change-impact` connection**: After reverse-engineered specs are complete, if impact analysis is needed, transition to the `/change-impact` skill.

---

## Phase 0: Decoding Plan (Karo Phase)

> **Source**: nrslib/takt main HEAD 7cd901... (2026-05-29)
> **Correspondence**: builtins/en/workflows/research.yaml `plan:plan(research-planner)` / instructions/research-plan.md
> **Correspondence confirmed**: ST-A1 §5.2

Phase performed by Karo. For one blackbox decoding task, confirms the scope, decomposes the investigation plan, and pre-defines quality gates. Corresponds to the takt research-plan step.

### Detailed Procedure (Phase 0)

**See** `reference/phase0_detail.md` **for details**

This section covers detailed procedures for target selection, scope definition, investigation plan creation, and quality gate definition.

### 0.5 Verification of Source Understanding (Required Before Phase 0)

> **Source**: `nrslib/takt` main HEAD `7cd901887...` (2026-05-29)
> **Correspondence confirmed**: ST-A1 (`reports/investigation/20260531_takt_full_acquisition.md`)

Before Karo drafts the investigation plan in Phase 0, confirm the following takt core mechanisms in the source report (ST-A1) and document the confirmation results in the Phase 0 plan. This is a mandatory step to prevent recurrence of the root cause identified in KPT 2026-06-01 P2: "verification of source understanding and verification of adaptation completion were separated and not connected."

**Core mechanisms requiring confirmation (evidence in ST-A1 §3 to §5.2)**:

| Core Mechanism | Source Reference | Correspondence in Adaptation |
| :--- | :--- | :--- |
| Loop structure (plan->dig->analyze->supervise) | ST-A1 §5.2 research workflow YAML | Phase 0->1->2->3 |
| verdict 3-value (APPROVED / NEEDS_REVISION / REJECTED) | ST-A1 §3.4 transitions.buildStepStatus | Phase 3 judgment output |
| 2-system rollback (on_revision->dig / on_rejection->plan) | ST-A1 §5.2 research workflow YAML | NEEDS_REVISION->Phase 1 / REJECTED->Phase 0 |
| 3-layer runaway prevention | ST-A1 §3.2/§3.3/§3.4 | Phase 3 Loop Detector |
| facet 5-type synthesis (personas/policies/knowledge/instructions/output-contracts) | ST-A1 §3.8/§3.9 | §0 takt adaptation table |

**Format for confirmation**:
Include an "Source Understanding Confirmation Section" in the Phase 0 plan, filling in each row of the above table. Write "Reference file/line: confirmed" or "estimated (reason)" for each. Proceeding to Phase 1 with items still marked "estimated" is prohibited.

**Reason**:
Prevention of recurrence of root cause from KPT 2026-06-01 P2: "verification of source understanding and verification of adaptation completion were separated and not connected." Confirming core source mechanisms with citations in Phase 0 creates the connection point to adaptation completion verification in Phase 4 (§4.5 equivalent).

### 0.4 Quality Gate Pre-definition (Rejection Conditions and Completion Conditions)

Karo defines "what is sufficient" before digging begins. This becomes the judgment criteria for Metsuke's Phase 3 audit.

**Completion conditions (criteria for sufficiency)**:

Of the approximately 30 questions listed by Karo, **80% or more must be answerable**.

- "Perfectly answering all questions" is not realistic. Instead, "all of the top 20 most important questions + approximately 50% of the rest" is the target.
- Answer quality means recording "code line number + citation" as a set. Inference alone does not count.

**Rejection conditions (events judged as insufficient)**:

If any of the following apply, Phase 3 Metsuke rejects back to Phase 1:

1. **Answer rate below 80%**: More than 20% of the listed questions are in an "uninvestigated" or "no results" state.
2. **Insufficient evidence**: There is a finding that "table X is operated," but details of "which code line" and "under what constraints" are unclear.
3. **Contradiction detected**: Multiple investigation results conflict. Example: "API A requires Cognito authentication" is documented, but the code has no authentication check.
4. **Hidden outputs undiscovered**: Side effects such as S3 / SFTP / email that are "within scope but completely overlooked."
5. **Missing soft-delete check**: Your project's required convention. If "this table has X records" is written without considering the soft-delete flag, it is subject to rejection. (See your project's coding conventions guide for the specific convention used. Typical conventions check whether queries filter by soft-delete column, and whether ORM models include soft-delete scope.)

**Concretizing rejections**:

When Metsuke determines a rejection, specific "additional investigation instructions" are written. Example:
```
[Rejection reason] No answer for question "retry on payment failure"
[Additional instructions]
- Search for full implementation of PaymentRetry (or equivalent) with Serena
- Confirm Max/Min constraints of retry_count column in DB schema
- Confirm evidence of retry execution in batch logs (recent 100 executions)
```

---

## Phase 1: Digging (Ashigaru Phase)

> **Source**: research.yaml `dig:execute(research-digger)` / instructions/research-dig.md
> **Correspondence confirmed**: ST-A1 §5.2

Ashigaru extracts facts necessary for blackbox decoding according to Karo's plan. Acts fully autonomously. Records as "not obtained" even when unable to determine, without stopping.

**See `reference/phase1_detail.md` for details**

### 1.1 Fact Extraction (Serena symbol search, grep, call hierarchy)

**Recommended execution order**:

1. **Identify Entry Point with Serena** (highest priority)
   - Symbol search for the API endpoint name (e.g., `POST /api/order`) or function name (e.g., `function createOrder`) specified by Karo.
   - Note the obtained file path and line number. Serena has accurate indexes, making it more reliable than grepping.
2. **Trace call chain with Call Hierarchy**
   - Trace all functions called from the Entry Point (maximum depth 3-5).
   - Record "what this function does" in one line for each call.
3. **Supplement with Grep for cross-file search**
   - Search all repositories with `grep -rn` using function names, table names, and class names.
   - Detect missed calls and hidden references (metaprogramming, reflection, dynamic SQL).
4. **Check DB schema**
   - Once the table names operated by the function are identified, read the PostgreSQL CREATE TABLE definition to confirm constraints, FKs, and indexes.
   - **Soft-delete check**: Does the table have a soft-delete column? If so, is the appropriate filter always included in queries? (See your project's coding conventions guide.)
   - Record in findings: "table `[your-table]` has soft-delete column; verify filter `WHERE soft_delete='0'` in all queries"

**Serena search tips**:
- Search keywords: 4 axes of "function name", "class name", "API endpoint", "table name" - one search each.
- If too many results (>100), narrow by repository and search again.
- Grasp the overall picture with `get_symbols_overview` before proceeding to detailed search rather than `find_symbol`.

### 1.2 Checking Dynamic Dependencies (Direct DB access, raw SQL, batches, external APIs)

Confirm not only static calls in code, but also dependencies that occur dynamically at runtime.

**Finding direct DB access and raw SQL**:
- Search for patterns like `$pdo->query` / `DB::query` / `SQL(` with grep (varies by language/framework).
- When SQL is built dynamically (string concatenation, sprintf), confirm "what values may be inserted."
- Check for SQL injection risks and missing soft-delete filters (potentially fetching deleted records).

**Batch execution dependencies**:
- After reading the endpoint implementation, confirm "is this table also updated by any batch?"
- Find batch files (e.g., `app/Console/Commands/*.php`) with grep and list all batches operating the same table.
- Also confirm the batch execution schedule (cron configuration).

**External API calls**:
- Search for Curl / GuzzleHttp / other client names in functions with grep.
- Record the API destination URL, authentication method, and behavior on failure.

**Asynchronous queues and messaging**:
- If SQS / Kinesis / Redis queues are used, trace "at what point messages enter the queue and which consumer processes them."

### 1.3 Discovering Hidden Outputs (SFTP, S3, email, PDF, etc. side effects)

List side effects such as file generation, external service calls, and notification sending, separate from DB updates.

**Discovering S3 operations**:
- Search for AWS SDK calls like `putObject` / `getObject` / `deleteObject` with grep.
- What is written to which bucket / prefix. Is it a timestamped path or a fixed path?
- Also confirm conditions for file deletion (when it disappears).

**Email and notification sending**:
- Search for `Mail::send` / `Queue::push('SendEmailJob')` / Firebase / LINE API, etc. with grep.
- Also confirm template file location, subject, and information included in the body.
- Record behavior on send failure (retry / log only).

**PDF and form generation**:
- Search for library usage like `TCPDF` / `wkhtmltopdf` / `Puppeteer` with grep.
- Whether the generated file is saved to S3 or returned directly in the response.
- Also record template (HTML / Blade) location.

**File FTP / SFTP sending**:
- Search for SFTP sends to external systems (e.g., print companies) with grep.
- File format (e.g., CSV / XML / PDF), send timing, and resend logic on failure.

### 1.4 Fact Recording Practice (file:line citation requirement, inference marking, honest reporting of non-retrieval)

All facts obtained through digging must be recorded in the following format.

**Recording format** (required elements):

```markdown
## [Question ID / Question text]

**Answer**: [Summarize fact in one line]

**Evidence**:
- File path: `[path]`
  - Line [L123]: [Code citation, maximum 3 lines]
  - Lines [L124-126]: [Range notation for multiple lines]
  - Related database: table `[your-table]`, column `[your-column]`
  - Constraints: NOT NULL, DEFAULT 'pending', FOREIGN KEY users(id)

**When inference is included**:
- Write "estimated" and state the reason.
  Example: "Since no retry logic is visible, failures are estimated to be immediately recorded in Error log"
```

**Notes**:

1. **Soft-delete check (your project's convention)**: When describing tables, always confirm "does this table have a soft-delete column? If so, is the filter applied in queries?" If a gap is found, write "Warning: missing soft-delete check (line X)". Standard checks include:
   - Grep for table name + `SELECT` / `UPDATE` / `DELETE` patterns
   - Confirm SQL text includes soft-delete filter (raw SQL case)
   - Confirm ORM model defines soft-delete scope (e.g., Eloquent local scope)
2. **Citation accuracy**: Not "around here" but "line L234" with precision. Copy from Serena or grep output.
3. **Handling uncertain information**:
   - "Code not found" -> Record honestly as "not obtained: no matching code found searching for [keyword]". Do not substitute with inference.
   - "DB schema not found" -> Distinguish as "not obtained: could not confirm PostgreSQL schema. Estimated from migration file: ~".
4. **Acceptable range of inference**: "Inferred from multiple pieces of evidence" or "confirmed in execution log" are OK. "I kind of think so" is NG.
5. **Prohibition on numerical assertions**: "This table has XX records" type of qualitative assertion is not acceptable. Instead:
   - Record "obtainable at runtime (can be confirmed with SELECT COUNT(*) with appropriate soft-delete filter)" .
   - Or present "sample logs of recent 100 executions."
6. **Honest reporting of non-retrieval**: If investigation fails due to time constraints or insufficient access, write "not obtained: [reason]". Also note that it will be supplemented in the next phase.

---

## Phase 2: Analysis (Ashigaru Second Phase)

> **Source**: research.yaml `analyze:execute(research-analyzer)`
> **Correspondence confirmed**: ST-A1 §5.2

Structures and integrates individual facts extracted in Phase 1. Corresponds to the takt analyze step. Records existing behavior as "the promise of the current implementation" (situation assessment, not modification proposals).

**See** `reference/phase2_detail.md` **for details**

This section covers detailed procedures for structuring findings (4 perspectives), additional investigation judgment, and cycle limit management.

---

## Phase 3: Quality Gate (Metsuke Phase)

> **Source**: research.yaml `supervise:review(research-supervisor)` / instructions/supervise.md, output-contracts/supervisor-validation.md
> **takt execution rule adaptation**: verdict 3-value (APPROVED/NEEDS_REVISION/REJECTED) + 2-system rollback
> **Correspondence confirmed**: ST-A1 §3.4

Metsuke audits Phase 1-2 digging and analysis deliverables and issues a verdict. Corresponds to the takt supervise step. When rejection is necessary, issues NEEDS_REVISION or REJECTED.

### 3.1 Independent Audit (Details)

**See** `reference/phase3_detail.md` **for details**

This section covers detailed procedures for evidence existence verification, scope fulfillment verification, and soft-delete compliance verification. §3.2 verdict 3-value and §3.4 3-layer runaway prevention are retained in this document.

### 3.2 Verdict 3-Value Issuance Rules

Metsuke issues one of the following 3 verdict values. Corresponds to takt's transitions.buildStepStatus (ST-A1 §3.4).

| verdict | Condition | Next Action |
| :--- | :--- | :--- |
| **APPROVED** | All required conditions met. Phase 1-2 digging and analysis satisfies scope and quality requirements | Advance to Phase 4. Begin deliverable creation (writing reverse-engineered specs, technical debt catalog, etc.) |
| **NEEDS_REVISION** | Minor deficiency; additional investigation needed. 60-79% of questions within scope are answered, but evidence for specific questions is weak, or a small newly discovered issue exists | **on_revision**: Roll back to Phase 1. Ashigaru performs additional digging and strengthens evidence for insufficient answers |
| **REJECTED** | Fundamental deficiency; plan revision needed. Fewer than 60% of questions within scope are answered, or a fundamental gap is detected | **on_rejection**: Roll back to Phase 0. Karo revises the investigation plan (scope change, question reset) |

**Default when no marker**: When no marker (APPROVED / NEEDS_REVISION / REJECTED) is explicitly stated in the verdict, the default is **NEEDS_REVISION** (safe side).


### 3.4 3-Layer Runaway Prevention (Loop Detector)

Controls repetition of investigation and analysis for the same question in 3 layers. Adaptation of takt's max_iterations=3 and stuck detection mechanism (ST-A1 §3.2-§3.4).

**Layer 1: Maximum 3 cycle limit**
- The Phase 1->2->3 loop runs a maximum of 3 times. After receiving Phase 1 rollback via on_revision 3 times, forced completion is judged.
- Regardless of any verdict in the 3rd cycle (even NEEDS_REVISION), force advance to Phase 4.

**Layer 2: FND-LOOP stop judgment (no change in same question)**
- When Ashigaru's answer to the same question is completely unchanged between cycles N and N+1, record as FND-LOOP (Found-Loop).
- When FND-LOOP is detected 2 consecutive times, issue APPROVED in the current cycle (judged as a state where no change is expected).

**Layer 3: Forced completion judgment**
- When both 3 cycles exceeded AND 2 consecutive FND-LOOPs occur, Metsuke issues APPROVED and advances to Phase 4. Prioritize "executable deliverable creation" over "sufficient precision."

---

## Phase 4: Deliverable Creation (Scribe Phase)

> **Source**: No corresponding element in takt research workflow (4-stage structure with supervise as the final step)
> **Design intent**: Additional phase specific to team adaptation. In takt, the supervise step output completes as the final deliverable, but in team-based usage, the persistent update approach where deliverables are persisted to knowledge/domain/ and accumulated across multiple cycles is required. This persistence process is defined as Phase 4, assigned to Ashigaru scribe.

Converts Phase 3 APPROVED deliverables into final deliverables. Handled by Ashigaru (scribe).

### Phase 4 Detailed Procedures

**See** `reference/phase4_templates.md` **for details**

This section covers detailed templates for reverse-engineered specs, technical debt catalog, migration priority matrix, and continuous update approach.

### 4.1-4.4 Template Groups (Details in reference/)

Detailed templates for Phase 4 (reverse-engineered specs, technical debt catalog, migration priority matrix, continuous update approach) are available in `reference/phase4_templates.md`.

### 4.5 Source Core Mechanism grep Verification (Quality Gate for Phase 4 Completion Judgment)

> **Source**: KPT 2026-06-01 Try T1 "Before completion judgment of externally adapted OSS skills, verify that source core mechanism keywords exist in deliverables via grep, and retain the verification command and count" - concretization

Before Ashigaru (scribe) submits deliverables (new SKILL.md) to Shogun, execute the following greps and include the execution commands and counts in the Phase 4 work log or report to Shogun.

**Required grep checklist**:

| Verification Item | grep Command | Expected Value | Source |
| :--- | :--- | :--- | :--- |
| facet term existence | `grep -ic "facet" SKILL.md` | 1 or more | ST-A1 §3.8/§3.9 facet synthesis mechanism |
| verdict term existence | `grep -ic "verdict" SKILL.md` | 1 or more | ST-A1 §3.4 transitions.buildStepStatus |
| APPROVED existence | `grep -c "APPROVED" SKILL.md` | 1 or more | ST-A1 §3.4 verdict 3-value |
| NEEDS_REVISION existence | `grep -c "NEEDS_REVISION" SKILL.md` | 1 or more | ST-A1 §3.4 verdict 3-value |
| REJECTED existence | `grep -c "REJECTED" SKILL.md` | 1 or more | ST-A1 §3.4 verdict 3-value |
| on_revision existence | `grep -c "on_revision\|roll back to Phase 1" SKILL.md` | 1 or more | ST-A1 §5.2 research workflow |
| on_rejection existence | `grep -c "on_rejection\|roll back to Phase 0" SKILL.md` | 1 or more | ST-A1 §5.2 research workflow |

**Judgment rules**:
- All items at or above expected value: Judge Phase 4 complete as "adaptation core existence confirmed"
- Even 1 item below expected value (0 items): Do not judge Phase 4 complete as "core not implemented". Identify the missing item and roll back to the responsible party for the corresponding subtask.

**Reporting format (must be included in Ashigaru scribe's report to Shogun)**:
```
[Phase 4 Source Core Mechanism grep Verification Results]
- grep -ic "facet" SKILL.md -> N items
- grep -ic "verdict" SKILL.md -> N items
- grep -c "APPROVED" SKILL.md -> N items
- grep -c "NEEDS_REVISION" SKILL.md -> N items
- grep -c "REJECTED" SKILL.md -> N items
- grep -c "on_revision|roll back to Phase 1" SKILL.md -> N items
- grep -c "on_rejection|roll back to Phase 0" SKILL.md -> N items
Judgment: adaptation core existence confirmed (all items 1 or more) / core not implemented (XX is 0 items)
```

**Positioning of source report**:
The basis for "expected values" in this verification is ST-A1 (`reports/investigation/20260531_takt_full_acquisition.md`). By verifying that core mechanisms confirmed in Phase 0 (confirmation table in §0.5) exist in Phase 4 deliverables, the connection of "source understanding (Phase 0) -> adaptation implementation (Phase 1-4) -> existence verification (Phase 4 §4.5)" is completed.

---

## I/O Specification

### Output Path List (Confirm Before Skill Execution)

| Phase | Output File | Path |
|-------|-----------|------|
| Phase 1-2 | Digging and analysis report | `reports/investigation/YYYYMMDD_[task-name]_phase2_analysis.md` |
| Phase 3 | Metsuke audit result | Appended to end of same file "§Final. Metsuke Audit Results" |
| Phase 4 | Reverse-engineered spec | `knowledge/domain/[feature-area]/reverse_engineered_spec_[feature-name].md` |
| Phase 4 | Technical debt catalog | `knowledge/domain/technical_debt_catalog.md` |
| Phase 4 | Migration priority matrix | `knowledge/domain/migration_priority_matrix.md` |

### Detailed Procedures

**Details on INPUT, OUTPUT, prerequisites, and successor skills: see** `reference/quality_checklists.md`

**Project-specific conventions**: For soft-delete rules, data patterns, and team structure adaptations unique to your project, see `reference/project_conventions.md`

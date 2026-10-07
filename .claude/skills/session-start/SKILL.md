---
name: session-start
description: Automate session context recovery. Aggregate previous incomplete tasks, in-progress work, and open issues to present today's priority tasks.
argument-hint: "[--light (lightweight)] [--full (full, default)]"
---

# Session Start Skill

## Overview

Recovers context from previous sessions at the start of a new session. Aggregates the following information and reports to Shogun (General) so today's tasks can be prioritized efficiently:

- Previous session's incomplete tasks
- Current project state
- Open issues and their priorities
- Changes to knowledge/ since last session

## Role Assignment

| Role | Responsibility |
|------|---------------|
| **Shogun (General)** | Decides when to invoke this skill, receives the report, and determines today's priorities |
| **Ashigaru (Foot Soldier / Worker)** | Executes this skill. Scans actual files to aggregate context information and generates / saves the report |
| **Metsuke (Inspector)** | Verifies the output file exists and checks for hallucination |

> **File save mandatory**: Output must be saved to `reports/session_context/`. stdout-only output is prohibited.

## Trigger Conditions

- New session start
- Resume after long interruption
- Uesama (Lord) mentions "continue from last time"

## Usage

```
/session-start
/session-start --light
/session-start --full
```

### Usage Examples

| Timing | Command |
|--------|---------|
| New session start | `/session-start` (default --full) |
| Resume after interruption | `/session-start --full` |
| Lightweight (local info only) | `/session-start --light` |
| GitHub API unavailable | `/session-start --light` (fallback) |

## Execution Steps

### Step 1: Read knowledge/README.md

**Purpose**: Verify directory structure and references; detect major structural changes since the last session.

**Actions**:
- Read the full `knowledge/README.md` file
- Extract the Last Updated date from the Version section
- Check for directory structure discrepancies

**Output fields**:
- Last updated date
- Change detection result ("Changed since last session" / "No changes")

### Step 2: Read workspace/in_progress/ latest files

**Purpose**: Identify in-progress tasks and extract their current state and next actions.

**Actions**:
- List all `.md` files in `workspace/in_progress/`
- Sort by modification date (descending)
- Read up to 3 files
- Extract "Next Action" / "Status" / "Priority" sections from each

**Output fields**:
- Task name, priority, current status, reference file name
- Next Action (if explicitly stated)

### Step 2.1: Verify completeness of sequentially persisted files (after clear/resume)

**Purpose**: Confirm that session-end files (KPT report and handover document) exist and are complete, ensuring proper context recovery after session clear or boundary.

**Execution condition**: This step runs when Step 2 confirms the presence of a handover document (`workspace/in_progress/*_session_handover.md`). The handover document's existence signals that the previous session has completed clear or session boundary, and this is the trigger to verify the two-file (KPT + handover) completeness.

**Actions**:
- Verify latest KPT report: `ls -t reports/*_kpt.md | head -1`
  - Confirm the file exists
  - Confirm the file is not empty (contains at least header and session overview)
  
- Verify latest handover document: `ls -t workspace/in_progress/*_session_handover.md | head -1`
  - Confirm the file exists
  - Confirm the file is not empty (contains required handover fields)

**Verification result classification**:

| Result | KPT Status | Handover Status | Judgment | Action |
|--------|-----------|-----------------|----------|--------|
| Complete | Exists + content | Exists + content | OK | Smooth session start. Proceed to Step 3 |
| Partial loss | Exists | Missing | Warning | Record warning in output. Report to Shogun before session start |
| Partial loss | Missing | Exists | Warning | Record warning in output. Report to Shogun before session start |
| Total loss | Missing | Missing | Critical | Previous session clear had issues. Report immediately to Shogun. Verify recovery procedure |

**Warning output format**:

If persisted files are incomplete, insert the following warning section immediately after Step 2 (before Step 3):

```markdown
## CRITICAL: Sequential Persistence File Verification Results

Files required for context recovery after the previous session clear are missing. Please verify:

| File Type | Verification Path | Status | Action |
|-----------|------------------|--------|--------|
| KPT Report | `reports/YYYY.MM.DD_kpt.md` | [Exists / Missing] | [OK / Retry / —] |
| Handover Document | `workspace/in_progress/YYYY.MM.DD_session_handover.md` | [Exists / Missing] | [OK / Retry / —] |

**Note**: Both KPT report and handover document must exist per the sequential persistence design. If either is missing, there may have been a save omission at the end of the previous session. Consult Shogun (General) and consider manual recovery if needed.
```

**Fallback**:
- KPT file not found: state "KPT report missing" and continue processing
- Handover file not found: state "Handover document missing" and continue processing
- Both missing: output the warning section and verify with Shogun before session start

### Step 2.5: Detect waiting-state tasks (awaiting kickoff trigger)

**Purpose**: Surface waiting tasks that are stalled pending a kickoff trigger, preventing overlooked blockers at session start.

**Actions**:
- Search all `.md` files in `workspace/in_progress/` for "Status: waiting" or similar markers
- Example command: `grep -l "Status.*waiting" workspace/in_progress/*.md`
- List all matching files (no limit)
- Extract "Kickoff trigger", "Escalation target", "Target deadline" from each file

**Fallback**:
- If 0 waiting files found: output "No waiting tasks"
- If frontmatter markers are absent: fall back to filename convention (e.g., `*_standby.md`) for secondary detection

**Output fields**:
- Waiting task list (table format)
- Kickoff trigger (if observable)
- Deadline estimate (if recorded)

### Step 3 (--full only): Check open Issues in the project repository

**Purpose**: Identify open GitHub Issues and their priorities.

**Actions**:
- Run `gh issue list --repo <owner>/<repo> --state open`
- Extract title, labels, and priority from results
- Prioritize items with these labels:
  - `priority: high`
  - `priority: critical`
  - `blocked`

**Fallback (A-006)**:
- On GitHub API failure: automatically fall back to `--light` mode
- Error message: "Could not check GitHub Issues. Reporting with local information only."

#### Step 3 Extension: Recent PR List (including CLOSED/MERGED)

**Purpose**: Surface recent PRs in all states (OPEN / CLOSED / MERGED) to prevent overlooking PR status changes. Particularly important: detect if handover-recorded PR numbers have been CLOSED or MERGED.

**Actions**:
- Run `gh pr list --repo <owner>/<repo> --state all --limit 5`
- Extract: PR number, title, state (OPEN / CLOSED / MERGED), last updated timestamp
- Cross-reference handover document for any recorded PR numbers; note their current status

**Fallback (A-006)**:
- On GitHub API failure: output "PR check skipped (GitHub API connection failed)" and fall back to `--light` mode
- Continue processing

**Output fields**:
- Recent PR list table (number, title, state, updated timestamp, handover reference)
- Note: If a handover-recorded PR is CLOSED / MERGED, add a note indicating its status change

### Step 4 (--full only): Check project session memory

**Purpose**: Retrieve incomplete tasks and blockers recorded in the previous session.

**Actions**:
- Check the `~/.claude/projects/<project-id>/memory/` directory
- Sort `project_session_*.md` files by modification date (descending)
- Read the latest 1 file
- Extract "Incomplete Tasks", "Blockers", "Next Steps" sections

**Output fields**:
- Previous session date
- Recorded priority tasks
- Blockers (including items awaiting action from others)

### Step 4.5 (--full only): Verify execution of Try items from previous KPT

**Purpose**: Surface the execution status of Try items (improvement actions) recorded in the previous KPT retrospective so Shogun (General) can judge whether they were carried out.

**Actions**:
- Locate the latest KPT file (e.g., `ls -t reports/*_kpt.md | head -1`)
- Extract the `## Try` section from that file
- Pull the following columns from the Try table:
  - Try ID (e.g., T1, T2)
  - Linked Problem ID (e.g., P1, P2)
  - Try content
  - Owner (e.g., Shogun, Karo, ashigaru-scribe, ashigaru-investigator)
  - Deadline (milestone-style: before next session start, before Phase 4 kickoff, etc.)
  - Completion verification method (file existence check, grep pattern, git log check, etc.)

**Fallback**:
- KPT file not found: output "No previous KPT" and continue
- Try section empty: output "No previous Try items"

**Output fields**:
- Try table
- Note for Shogun (General): "Status judgment (completed / partially executed / not executed / no effect) is performed by Shogun in line with the KPT skill definition. Do not auto-classify."

#### Step 4.5 Extension 1: Auto-extract and priority-flag mandatory Try items

**Mandatory Try detection keywords**:
- "mandatory"
- "unconditional start"
- "before next session start"
- "immediately after next session start"
- "forced Five Whys execution target"

**Auto-extraction process**:
- Search the latest KPT file's Try table for the above keywords
- Detect matching Try rows via grep
- Display detected Try items as "Mandatory Try Alert" at the top of the session start summary (immediately after Overview section), with highest priority
- If no matches: output "(No mandatory Try items)"

**Output format extension**:

Add the following section to the session summary immediately after the Overview section:

```markdown
## Mandatory Try Alert (Highest Priority - Unconditional Start)

| Try ID | Try Content | Deadline | Mandatory Reason |
|--------|-------------|----------|------------------|
| [Try ID] | [content summary] | [deadline] | [consecutive unexecuted count, Five Whys forced target, etc.] |

**Note**: Mandatory Try items follow the KPT skill rule (kpt/SKILL.md L63-71 rule). Start unconditionally before any other task.
```

If no mandatory Try: output "(No mandatory Try items)"

#### Step 4.5 Extension 2: Deadline-based Try priority grouping (parallel-task support)

**Purpose**: Distinguish Try items that are NOT mandatory but might run in parallel ("by session end"), enabling Shogun to efficiently decide whether to attempt them in this session.

##### Three-tier deadline grouping

| Group | Deadline Keywords | Priority | Recommended Action |
|-------|-------------------|----------|-------------------|
| Mandatory Try | "mandatory" / "unconditional start" / "before next session start" / "immediately after next session start" / "forced Five Whys execution target" | Highest | Start unconditionally before any other task (per kpt/SKILL.md L63-71) |
| Parallel-candidate Try | "by session end" / "before session end" | Medium | Shogun judges parallel feasibility in this session |
| Standard Try | All others (e.g., "before next-next session") | Low | Judge after main session task completion |

##### Parallel feasibility judgment logic

Shogun applies the following criteria to decide if a parallel-candidate Try should run in this session:

1. **Work estimate**: Is the Try a meta-task (e.g., add feedback memory, small SKILL.md edit) completable within 30 minutes?
2. **Session impact**: Will parallel execution avoid context congestion to the main task?
3. **Dependencies**: Does the Try depend on results from this session's main task?

If all 3 conditions are met, parallel execution is recommended. Otherwise, defer to a later session.

##### Output format extension

Add the following section immediately after the Mandatory Try Alert section:

```markdown
### Deadline-based Try Priority Judgment (Session Parallel Feasibility Support)

#### Mandatory Try (Unconditional Start - before other tasks)

| Try ID | Try Content | Deadline | Mandatory Reason |
|--------|-------------|----------|------------------|
| [Try ID] | [content summary] | [deadline keyword] | [consecutive unexecuted count, Five Whys forced target, etc.] |

**Note**: Mandatory Try items follow kpt/SKILL.md L63-71 rules. Start unconditionally before other tasks.

#### Parallel-candidate Try (Session end target - Shogun judges feasibility)

| Try ID | Try Content | Deadline | Parallel Recommended? | Judgment Reason |
|--------|-------------|----------|----------------------|-----------------|
| [Try ID] | [content summary] | [deadline keyword] | [Recommended / Not Recommended] | [meta-task judgment, dependency, context load] |

**Note**: Parallel recommendation is Karo's pre-judgment. Shogun makes the final call considering this session's load and priorities.

#### Standard Try (After main session task completion)

| Try ID | Try Content | Deadline |
|--------|-------------|----------|
| [Try ID] | [content summary] | [deadline] |
```

**Note**: If a deadline group has no Try items, mark that group "(None)"

##### Extraction process

Automatically classify Try items from the latest KPT file's Try table using the above keywords:
- Mandatory Try: Contains any of the 5 mandatory keywords (Step 4.5 Extension 1)
- Parallel-candidate Try: Contains "by session end" or "before session end"
- Standard Try: All others

### Step 5: Generate and save context summary

**Purpose**: Integrate all collected information into a report and present it to Shogun (General).

**Actions**:
- Integrate information from Steps 1-4 (and Step 4.5 in --full mode)
- Generate report using the standard format (below)
- Save to: `reports/session_context/{YYYYMMDD}-session-start.md`
- Deliver standard report to Shogun (General)

**Output file**:
- `reports/session_context/{YYYYMMDD}-session-start.md` (required, file save mandatory)

## Mode Specification

| Mode | Steps Executed | Scope | Estimated Time |
|------|---------------|-------|---------------|
| `--light` | Step 1 + Step 2 + Step 2.5 (local file scan only) | Local only | Short |
| `--full` (default) | Steps 1-4 + Step 2.1 + Step 2.5 + Step 3 + Step 3 Extension + Step 4.5 + Step 4.5 Extensions (includes GitHub API) | Full scope | Moderate |

Default is `--full`. If GitHub API fails during `--full` execution, automatically falls back to `--light` results.

**Key additions in `--full` mode**:
- **Step 2.1**: Sequential persistence file verification (KPT + handover completeness check)
- **Step 3 Extension**: Recent PR list (`gh pr list --state all --limit 5`) to detect CLOSED/MERGED status changes
- **Step 4.5 Extensions**: Mandatory Try auto-extraction and three-tier Try deadline grouping (mandatory / parallel-candidate / standard)

## Output Format

Generated file: `reports/session_context/{YYYYMMDD}-session-start.md`

```markdown
# Session Start Summary -- {YYYY-MM-DD}

## Overview
State aggregation at the start of this session. Summarizes progress, blockers, and open issues since the previous session.

## Deadline-based Try Priority Judgment (Session Parallel Feasibility Support) (--full only)

### Mandatory Try (Unconditional Start - before other tasks)

| Try ID | Try Content | Deadline | Mandatory Reason |
|--------|-------------|----------|------------------|
| [Try ID] | [content summary] | [deadline keyword] | [consecutive unexecuted count, Five Whys forced target, etc.] |

**Note**: Mandatory Try items follow kpt/SKILL.md L63-71 rules. Start unconditionally before other tasks.

If none: "(No mandatory Try items)"

### Parallel-candidate Try (Session end target - Shogun judges feasibility)

| Try ID | Try Content | Deadline | Parallel Recommended? | Judgment Reason |
|--------|-------------|----------|----------------------|-----------------|
| [Try ID] | [content summary] | [deadline keyword] | [Recommended / Not Recommended] | [meta-task judgment, dependency, context load] |

**Note**: Parallel recommendation is Karo's pre-judgment. Shogun makes the final call considering this session's load and priorities.

If none: "(No parallel-candidate Try items)"

### Standard Try (After main session task completion)

| Try ID | Try Content | Deadline |
|--------|-------------|----------|
| [Try ID] | [content summary] | [deadline] |

If none: "(No standard Try items)"

## State Changes Since Last Session

### Sequential Persistence File Verification (--full only)

When a handover document is detected in Step 2, Step 2.1 verifies that both the KPT report and handover document exist and contain content:

| File Type | Verification Path | Status | Action |
|-----------|------------------|--------|--------|
| KPT Report | `reports/YYYY.MM.DD_kpt.md` | [Exists / Missing] | [OK / Retry] |
| Handover Document | `workspace/in_progress/YYYY.MM.DD_session_handover.md` | [Exists / Missing] | [OK / Retry] |

**Critical Note**: If either file is missing or empty, a CRITICAL warning section is generated and displayed here. Both files must exist and contain content per the sequential persistence design. Report any discrepancies to Shogun immediately.

### knowledge/ Update Status
- Last updated: {date}
- Changes: {Changed / No changes / {updated items}}

### In-Progress Tasks (workspace/in_progress/)

| Priority | Task | Status | Reference | Next Action |
|----------|------|--------|-----------|-------------|
| High | [task name] | Paused | workspace/in_progress/xxx.md | [next action] |
| Medium | ... | ... | ... | ... |

**Notes**: [blockers, dependencies if any]

### Waiting-state Tasks (awaiting kickoff trigger) (--full only)

| Task Name | Kickoff Trigger | Waiting File | Target Deadline |
|-----------|-----------------|--------------|-----------------|
| ... | ... | workspace/in_progress/xxx_standby.md | observable / YYYY-MM-DD |

**Note**: If no waiting tasks: "(No waiting tasks)"

### Open Issues (--full only, priority order)

| Repository | # | Title | Labels | Reference |
|-----------|---|-------|--------|-----------|
| <owner>/<repo> | #NN | [title] | priority: high, blocked | [URL] |
| ... | ... | ... | ... | ... |

**Notes**: Only priority: high / critical shown. Other labeled issues noted if present.

### Recent PR List (--full only, all states)

| # | Title | State | Updated | Handover Reference |
|---|-------|-------|---------|-------------------|
| #NN | [title] | MERGED / CLOSED / OPEN | YYYY-MM-DDTHH:MM:SSZ | Present / Absent |

**Notes**: `gh pr list --state all --limit 5`. If a handover-recorded PR is CLOSED / MERGED, note the status change.

### Previous Session Record
- Previous session date: {YYYY-MM-DD}
- Record file: MEMORY.md > project_session_*.md
- Incomplete tasks:
  - [item 1]
  - [item 2]
- Blockers: [if any]

### Previous KPT Try Execution Status (--full only)

| Try ID | Linked Problem | Try Content | Owner | Deadline | Verification Method | Status (filled in by Shogun) |
|--------|---------------|-------------|-------|----------|---------------------|------------------------------|
| T1 | P1 | [summary] | ashigaru-scribe | before next session | [method] | not yet executed |
| T2 | P2 | [summary] | Karo | before Phase 4 kickoff | [method] | not yet executed |

**Note**: Status judgment (completed / partially executed / not executed / no effect) is performed by Shogun (General) in line with the KPT skill definition. Do not auto-classify; defer to Shogun's visual judgment.

## Recommendations for This Session

> **Question for Shogun (General)**
>
> Based on the above, what are today's priority tasks?
>
> Suggested priority order (for reference):
> 1. Mandatory Try items (if any)
> 2. Issues with priority: high or critical labels
> 3. Previous session's incomplete tasks (no blockers)
> 4. Parallel-candidate Try items (if feasible in this session)
> 5. New tasks

## Execution Environment

- Execution time: {ISO 8601}
- Mode: --light / --full
- GitHub API: Connected / Fallback (connection failed)

---

**Generated by**: /session-start skill
**Version**: 1.2
```

## Notes (Inspector Findings Reflected)

### A-003: Planner consultation note

If complex tasks (spanning 2+ areas, multiple repositories, or requiring impact analysis) are found in the output summary, append the following note:

> **Important**: Consult Karo (Chief Retainer / Planner) before executing this task. Decomposing complex tasks and impact analysis are Karo's responsibility.

### A-004: Mandatory file save (file save mandatory)

- Output must be saved to `reports/session_context/{YYYYMMDD}-session-start.md`
- A stdout report text is also produced, but file save is required
- A session start without a saved file is treated as a failure

### A-006: GitHub API fallback

On GitHub API failure:

1. Detect the failure and automatically fall back to `--light` mode
2. Record "GitHub API: Fallback (connection failed)" in the output report
3. Report to Shogun (General): "Could not check GitHub Issues. Reporting with local information only."
4. Continue processing -- complete the report with Step 1-2 information

### Hallucination Prevention

- Record only facts confirmed by actual file scanning
- Do not interpolate incomplete tasks or task content with guesses
- If a file is not found, explicitly state "No matching file found"
- Do not record uncertain information

---

## Prohibited Actions

- Modifying files or editing code (this skill's purpose is information aggregation and reporting only)
- Guessing or interpolating task content (report only actual file facts)
- Returning stdout only without file save (file save mandatory)

---

## I/O Specification

### INPUT

| Type | Content | Required | Example |
|------|---------|----------|---------|
| Mode | Execution mode | Optional | `--light`, `--full` (default) |

### OUTPUT

| Type | Format | Destination | Required |
|------|--------|-------------|----------|
| Session start summary | Markdown | `reports/session_context/{YYYYMMDD}-session-start.md` | Yes (file save mandatory) |
| Shogun (General) report text | Standard report format | stdout | Yes |

### Preconditions

- `workspace/in_progress/` directory exists
- `reports/session_context/` directory exists (create if missing)
- MEMORY.md exists (--full only)
- KPT files (`reports/*_kpt.md`) must exist (--full mode only, with fallback)

### Subsequent Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/project-guide` | Starting a new task | Report to Shogun (General), obtain approval before executing |
| `/current-spec` | Continuing previous investigation | Report to Shogun (General), obtain approval before executing |
| `/change-impact` | Impact analysis needed for complex task | Consult Karo (Chief Retainer / Planner) before executing |

### Fallback Rules

**GitHub API connection failure**:
- During `--full` execution, on detecting a connection failure, automatically fall back to `--light` mode results
- Record "GitHub API: Fallback" in the output report
- Continue processing

**File not found**:
- If a target file is not found, output "No matching file found"
- Continue processing. Other steps still execute

## Quality Checkpoints

- [ ] `reports/session_context/{YYYYMMDD}-session-start.md` was created
- [ ] File contains frontmatter (generation date, mode, API status)
- [ ] All information from Steps 1-4 + Step 2.1 + Step 2.5 + Step 3 + Step 3 Extension + Step 4.5 + Step 4.5 Extensions is aggregated (for --full mode)
- [ ] Step 2.1: Sequential persistence file verification results are present; KPT report and handover document existence confirmed
- [ ] Step 2.1: If either file is missing, warning section is output before Step 3 (critical state)
- [ ] Step 2.5: Waiting-state task list (if any) matches grep results from `workspace/in_progress/`
- [ ] Step 3 Extension: Recent PR list (`gh pr list --state all --limit 5`) is included (--full only)
- [ ] Step 3 Extension: Handover-recorded PR numbers are cross-referenced with recent PR list (--full only)
- [ ] Step 4.5 Extension 1: Mandatory Try table exists (--full only) with 5 keyword detection
- [ ] Step 4.5 Extension 2: Three-tier Try priority tables (Mandatory / Parallel-candidate / Standard) all present (--full only)
- [ ] Previous KPT Try table matches latest KPT file content (--full only)
- [ ] Try table "Status (filled in by Shogun)" column shows [not yet executed] or remains blank for auto-judgment deferral
- [ ] No hallucination (no guessed or interpolated content)
- [ ] Complex tasks include the Karo (Planner) consultation note (A-003)
- [ ] Priority ordering is reasonable

> Note: See `config/terminology.md` for term customization.

## Change History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-21 | 1.2.1 | Clarity improvements: Removed Japanese grep pattern from Step 2.5 example (English-only mode). Expanded Mode Specification section to explicitly list Step 2.1, Step 3 Extension, and Step 4.5 Extensions. Enhanced quality checkpoints with Step 2.1 missing-file warning detection. Clarified Sequential Persistence File Verification section description to emphasize critical warning output condition. |
| 2026-06-19 | 1.2 | Added Step 2.1 (sequential persistence file verification), Step 2.5 (waiting-state task detection), Step 3 extension (recent PR list with CLOSED/MERGED), Step 4.5 extensions (mandatory Try alert + three-tier Try priority grouping), expanded output format and quality checkpoints |
| 2026-06-06 | 1.1 | Added Step 4.5 (mandatory Try auto-extraction), quality checkpoints, edge case handling |

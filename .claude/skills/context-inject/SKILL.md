---
name: context-inject
description: Before starting a task, enumerate related existing knowledge and reports to prevent information gaps and duplicate investigation. Searches knowledge/, reports/, and agent memory for relevant context.
argument-hint: <keyword or task description>
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# /context-inject — Related Context Injection Skill

## Overview

A skill for automatically listing "existing investigation results and domain knowledge related to this task" before starting a new task.
Lists confirmed knowledge in `knowledge/`, work-in-progress reports in `reports/`, and behavior rules in agent `memory/`, preventing information gaps and duplicate investigation.

---

## Execution Steps

### Step 1: Identify Keywords
Identify relevant keywords from the user's prompt or the current task.

**Keyword examples**:
- Domain terms: print, contract, price, delivery, cameraman, teacher, partner, etc.
- Process names: PR1, PR2, PR3, PR4, PR5 (business processes)
- System names: core API, EC site, partner portal, teacher dashboard, etc.
- Feature terms: sales aggregation, incentive, face recognition, photo quality, notification, etc.

### Step 2: Search knowledge/
Search for related files using the following command:

```bash
grep -rl "<keyword>" ./knowledge/ 2>/dev/null | head -20
```

**Directory meanings**:
- `knowledge/system/` — Project structure and schema DB (confirmed)
- `knowledge/domain/` — Domain knowledge (confirmed)
- `knowledge/quality/` — Quality and risk information (confirmed)
- `knowledge/standards/` — Evaluation criteria (confirmed)

### Step 3: Search reports/
Search past investigation reports for related content. Exclude archived/:

```bash
grep -rl "<keyword>" ./reports/ 2>/dev/null | grep -v archived | head -10
```

**Directory meaning**:
- `reports/` — Work-in-progress reports (reference information)

### Step 4: Search agent memory/
Search for related feedback memories:

```bash
grep -rl "<keyword>" ~/.claude/projects/*/memory/ 2>/dev/null | head -10
```

**Directory meaning**:
- `memory/` — Agent behavior rules and judgment criteria (cross-session learning records)

### Step 5: Format and Output Results

Report to the Shogun (General) in the following format:

```
## Related Context

### knowledge/ (Confirmed domain knowledge)
- [filename](path) — Summary (one-line excerpt from file heading)

### reports/ (Work-in-progress reports / reference information)
- [filename](path) — Summary

### memory/ (Behavior rules / judgment criteria)
- [filename](path) — description

### Recommended Reference Order
1. First, check confirmed knowledge in knowledge/ (SSOT = Single Source of Truth)
2. Next, check past investigations in reports/ (work-in-progress analysis and insights)
3. Keep memory/ rules in mind before starting work (lessons from past sessions)

### Divergence / Conflict Check
If the same topic appears in both knowledge/ and reports/, treat knowledge/ (confirmed version) as SSOT
and flag any differences found in reports/ (work-in-progress version).
```

### Step 6: Divergence and Conflict Check
If the same topic appears in both `knowledge/` and `reports/`, report `knowledge/` (confirmed version) as the SSOT and flag divergences with the `reports/` (work-in-progress version).

---

## When to Use

| Timing | Example |
|--------|---------|
| **Before starting a new investigation** | "Starting investigation of PR2 contract domain" → list existing contract-related knowledge |
| **Before organizing domain knowledge** | "Creating SPEC for photo quality management" → check existing quality-related investigation results |
| **Before impact analysis** | "Changing the pricing API in the core backend" → understand existing pricing feature analyses |
| **Before integrating domain knowledge** | "Organizing legacy features" → list past legacy investigation reports |

---

## Notes

### When search results return 0 matches
```
No related existing information found. New investigation required.

[Recommended Actions]
- Expand keywords and re-search
- Check knowledge/README.md for domain knowledge structure
- Plan to integrate results into knowledge/ after investigation is complete
```

### When search results are too many (20+ files)
Narrow keywords and report only the 5-10 most relevant files.

**Examples**:
- "price" → reclassify as "price API", "price setting", "wholesale price", etc.
- "partner" → reclassify as "partner contract", "partner portal", etc.

### Distinguishing knowledge/ from reports/
- **knowledge/ (confirmed)**: Project-wide confirmed information. Reference as SSOT.
- **reports/ (work-in-progress)**: Investigation results and analyses created during a session. Reference as supplementary information.

---

## Automated Injection via Hook

This skill can also be triggered automatically via the `UserPromptSubmit` hook using `scripts/context_inject.py`.
See `docs/hooks-setup.md` for configuration instructions.

The hook script analyzes keywords in the user's prompt and injects relevant feedback memory
and knowledge file paths into the system context before each response.

---

## Implementation Notes

### Extracting knowledge/ file summaries
Extract 1-3 lines from the file heading (`# Title`) as a summary.

### Path display rules
- Display paths relative to the project root (e.g., `knowledge/domain/...`)
- Use absolute paths only when needed for shell commands

### Case sensitivity / underscores / hyphens
Consider both `-` and `_` during search. For example, search for both `current_spec` and `current-spec`.

---

## Expected Benefits

| Problem | Solution |
|---------|---------|
| Existing information is overlooked when starting a task | Steps 2-4 automatically list existing information |
| Same topic is described redundantly in knowledge/ and reports/ | Step 6 divergence check detects conflicts |
| Duplicate new investigations | "No existing information" judgment confirms that new investigation is truly new |
| Past lessons are not utilized | memory/ search surfaces past judgment criteria and behavior rules |

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-05-25 | 1.0 | Initial release. 6-step search/organize/output flow, notes on 0 results and 20+ results, automated hook integration reference. |

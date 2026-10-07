# Phase 2 Detailed Procedures -- Analysis (Ashigaru Second Phase)

> **Created**: 2026-06-01

Structures and integrates individual facts extracted in Phase 1. Records existing behavior as "the promise of the current implementation" (situation assessment, not modification proposals).

## 2.1 Structuring Findings (4 Perspectives)

Structure the facts individually recorded in Phase 1 from the following 4 perspectives.

**Perspective 1: Data flow diagram**
- Create an overall flow diagram (text-based) from input to output
- For each step, include "filepath:line-number", "database table", and "external API"

**Perspective 2: Table operation map**
- Record how each table is operated "when, by whom, and how": organize operation sources and timing for INSERT / UPDATE / DELETE / SELECT in a table
- Soft-delete check column is also required. For each row, explicitly state "is this table soft-deleted?"

**Perspective 3: Error handling and edge cases**
- Record error behavior and abnormal flow behavior
- List behavior for each scenario such as payment failure, timeout, and network failure

**Perspective 4: Recording existing behavior as "the current promise"**
- List the constraints and assumptions created by the current implementation
- Write in the form "multiple POSTs to /api/order within the same session by the same user are allowed"

## 2.2 Additional Investigation Judgment (Return to Phase 1 when new questions arise. Maximum 3 cycles recommended)

It is not unusual for "new questions not yet understood" to emerge during Phase 2 analysis.

**Criteria for returning (re-execute Phase 1)**:
1. Uninvestigated item within scope
2. Essential for resolving contradiction
3. Suspected hidden path
4. Unknown details of external integration

**Criteria for not returning (proceed as-is)**:
1. Out of scope
2. 80% criterion is satisfied
3. Task for the next cycle

**Cycle limit (maximum 3 cycles recommended)**:
- Cycle 1: Phase 1 first pass -> Phase 2 analysis
- Cycle 2: Phase 1 additional digging -> Phase 2 integration
- Cycle 3: Phase 1 final digging -> Phase 2 completion
- 4 or more cycles are not recommended

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated Phase 2 details from SKILL.md |

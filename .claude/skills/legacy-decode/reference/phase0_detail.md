# Phase 0 Detailed Procedures -- Decoding Plan (Karo Phase)

> **Created**: 2026-06-01
> **Target**: Detailed procedures for Phase 0 (planning performed by Karo)

Phase performed by Karo. For one blackbox decoding task, confirms the scope, decomposes the investigation plan, and pre-defines quality gates.

## 0.1 Target Selection (Migration Priority Matrix Input Criteria)

When selecting a decoding target, use the following as judgment criteria:

**Appropriate targets**:
- Features where current code behavior is unclear. Cases where there is a discrepancy between specs and code, or implementation intent is opaque.
- Areas where responsibility allocation across multiple repositories is unclear. Example: direct DB access between services, 5-service event-driven processing in complex multi-service architectures.
- Things hypothesized as technical debt, but where "what specifically is the debt" and "to what degree" are unconfirmed.

**Targets to avoid**:
- Features where reverse-engineered specs already exist and knowledge is already aggregated in knowledge/. In that case, reference only is sufficient.
- Small-scale features that are self-contained within one service. If scope is too small, cost does not justify running through Phase 0-4. Target "2-4 hours of investigation" as a guideline.
- Pure "knowledge confirmation" purposes where no changes are planned for currently running features. Cases where time until knowledge generates value is too long.

## 0.2 Scope Definition (Defining the Range Covered in One Cycle)

Karo explicitly defines the following:

**Three elements of scope**:

1. **Target repositories**: Separate the main investigation target and related repositories to be read for reference. Explicitly indicate "primary", "reference", and "out of scope" for each.
2. **Target features**: Identify "what to decode" by business process unit or API endpoint unit. Example: Not "PR3.3 purchase/order flow overall" but "operation of [specific table] around [specific endpoint]". Narrow the granularity to a scale that can be dug in "one session (2-4 hours)".
3. **Table, API, and external integration boundaries**: Draw boundary lines for "where investigation stops." Example: "Details of authentication are out of scope" / "Image storage logic is within scope".

**Minimum format for scope definition document**:
```
[Decoding target] [feature name / API name / domain name]
[Target repositories]
  - Primary: [repository name]
  - Reference: [repository name]
[Investigation target tables] [table name 1, table name 2, ...]
[Investigation target APIs] [endpoint 1, endpoint 2, ...]
[Scope boundary] [items explicitly stated as "not investigated beyond this point"]
```

## 0.3 Investigation Plan Creation (Question List, Method Selection)

Karo receives scope and decomposes the following:

**Question decomposition (Fact Finding question list)**:

For the decoding target, enumerate approximately 30 questions in priority order (the following types):

- **Data flow questions**: "From input A passed to function X, through what tables does it pass before reaching output Z?" "What is validated at each step?"
- **Boundary questions**: "In direct DB operations between services, what rules govern synchronization success/failure?" "Is synchronization between batches and APIs guaranteed?"
- **External integration questions**: "On API call failure to external payment provider, what retry logic runs?" "When and by whom is image storage performed?"
- **Hidden side effect questions**: "What side effects such as email sending, PDF output, SFTP transfer occur beyond DB updates?"
- **Error handling questions**: "What error path is taken when each operation fails?" "Is rollback or retry performed?"

**Method selection**:

Select one of the following for each question:

- **Serena symbol search**: Identify entry point from function name or class name. Perform first.
- **Grep**: Cross-file search by keyword (table name, function name). Get line numbers with `grep -rn`.
- **Call Hierarchy**: Trace function call chains. Use IDE symbol functionality (Serena).
- **DB schema check**: Read constraints, indexes, and FKs from actual PostgreSQL schema.
- **Code execution / Log**: Obtain operation logs in local dev or staging environment.
- **Domain knowledge reference**: Reference existing domain spec documents in knowledge/ (PR3 flow, etc.). Avoid duplicate investigation.

## 0.4 Quality Gate Pre-definition (Rejection Conditions and Completion Conditions)

Karo defines "what is sufficient" before digging begins. This becomes the judgment criteria for Metsuke's Phase 3 audit.

**Completion conditions (criteria for sufficiency)**:

Of the approximately 30 questions listed by Karo, **80% or more must be answerable**.

- "Perfectly answering all questions" is not realistic. Instead, "all of the top 20 most important questions + approximately 50% of the rest" is the target.
- Answer quality means recording "code line number + citation" as a set. Inference alone does not count.

**Rejection conditions (events judged as insufficient)**:

If any of the following apply, Phase 3 Metsuke rejects back to Phase 1:

1. **Answer rate below 80%**: More than 20% of the listed questions are in an "uninvestigated" or "no results" state.
2. **Insufficient evidence**: There is a finding that "table X is operated," but details of "which code line" and "under what constraints" are unclear.
3. **Contradiction detected**: Multiple investigation results conflict. Example: "API A requires authentication" is documented, but the code has no authentication check.
4. **Hidden outputs undiscovered**: Side effects such as S3 / SFTP / email that are "within scope but completely overlooked."
5. **Missing soft-delete check**: Your project's required convention. If "this table has X records" is written without considering the soft-delete flag, it is subject to rejection. (See your project's coding conventions guide for the specific convention used.)

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated Phase 0 details from SKILL.md |

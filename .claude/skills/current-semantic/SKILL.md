---
name: current-semantic
description: Create a semantic model of the target business domain. Describe entities, relationships, ubiquitous language, and business rules as business meaning - the single file that developers should open first when working with that domain.
argument-hint: "[domain-name] [main-table-names] [--db database-name]"
---

> This is a generic skill from [CLysis](https://github.com/t-hasuike/CLysis).
> Terminology can be customized via `config/terminology.md`.

# /current-semantic -- Semantic Model Creation Skill

## Overview

Create a semantic model (SEM-XX file) for the specified business domain.

**Purpose Declaration**: This skill produces "a file that conveys the meaning of business." It is not a data dictionary listing all table columns. The goal is to create a single file that developers new to this domain should open first in order to understand the business meaning.

**Axes to Confirm Before Execution (Always Self-Check)**:
1. Can you explain "why does this entity exist?" 
2. Can you communicate "what does this value mean?" through a flag table?
3. Can you show "how does this differ across channels/routes?" through comparison?
4. Are you listing columns (not meaningful), or can you omit meaningless columns?
5. Are you scoping by table's physical location (database/row count), or by "the range of business concepts that are self-contained?"

## Role Assignment

| Role | Responsibility |
|------|---|
| **Leader** | Specify the target domain, delegate to worker, receive deliverable. Do not read code yourself (F002 compliance). |
| **Planner (karo)** | When handling multiple domains simultaneously: task decomposition and pre-analysis of domain boundaries. |
| **Worker** | Execute this skill. Investigate code, create semantic model, draft PR to knowledge repository. |
| **Auditor (metsuke)** | Audit deliverable quality (symbol existence verification, business meaning description check, axis compliance verification). |

> **F002 Compliance**: Leader must not read code yourself. All investigation and creation must be delegated to worker.

## Usage

```
/current-semantic [domain-name] [main-table-names]
```

### Execution Examples

| Timing | Command |
|---------|---------|
| Organization domain (main tables: organizations, groups) | `/current-semantic organization organizations groups` |
| Contract domain (main table: contracts) | `/current-semantic contract contracts` |
| Partner organization domain (multi-database) | `/current-semantic partner-organization --db partnerdb` |

## Workflow

### Step 1: Identify Domain Boundaries and Business Concepts

**Purpose**: Define "what business concepts should developers working with this domain understand?" before listing tables.

**Execution**:
1. Verify existence of domain name and main table names (Bash ls / Glob)
2. Check domain boundaries against existing SEM files (list SEM-XX files in semantic directory)
3. Identify management authority and physical database location
4. Confirm whether channel/path differences exist (do different routes behave differently?)

**Axis Check (Must Confirm Before Step 1 Completion)**:
- [ ] Can you state in 1 sentence: "What should the developer of this domain want to know first?"
- [ ] Are you scoping by "business concept's self-contained range," not "table's physical location?"
- [ ] If path differences exist, can you explain them as "business model differences," not just "different tables?"

**Output**: Step 1 completion report (domain boundary text description, target table list, presence/absence of path differences)

---

### Step 2: Code Investigation (Extract Business Meaning)

**Purpose**: Extract "business meaning" from code. Understand not just column names, but "why does this column exist?"

**Execution**:
1. Investigate Model files (Serena find_symbol or Bash grep)
   - Collect fillable column list (understand all columns)
   - Gather constants/Enum definitions (flag values, status values)
   - Relationships (HasOne, HasMany, BelongsTo) reveal related entities
   - Scope methods (scope*) expose business rules
2. Investigate UseCase files (what changes during registration/update flows)
3. Cross-database cases: Identify cross-system join keys
   - Record join key AND "the business meaning of that join" (example: `maindb.organizations.org_id = partnerdb.institutions.core_org_id` means "partner-registered organization is auto-reflected in main system's organizations table")

**Full Column Investigation Obligation**:
- Read ALL columns in fillable, hidden, and casts. Do NOT sample only representative columns.
- You MAY group together columns with self-evident business meaning (id PK, created_at, generic management columns) as "generic management columns."

**Axis Check (Must Confirm Before Step 2 Completion)**:
- [ ] Can you explain for each column "why does this column exist?"
- [ ] Did you extract flag value "meaning" (what the value represents) from constant definitions?
- [ ] Did you mark columns with non-obvious business meaning as "requires verification?"
- [ ] If cross-database: Can you explain join key business meaning?

---

### Step 3: Draft Section Outline Using 6-Chapter Structure

**Purpose**: Describe business meaning in 6 chapters. Chapter structure follows "developer understanding sequence."

#### Chapter Design Principles

Chapter 1 is most critical. After reading it, developers should understand "this domain's business structure."

| Chapter | Title | Purpose | Writing Axis |
| :--- | :--- | :--- | :--- |
| Chapter 1 | Overview | Help reader grasp domain purpose, routes, main entities | Begin with "why does this domain exist?" NOT "list table names" |
| Chapter 2 | ER Diagram | Visualize entity relationships | Mermaid erDiagram format. Add business meaning to relationship lines |
| Chapter 3 | Table-by-Table Column Definitions | Explain each table's column business meaning | Always include "business meaning" column. Touch on constants/Enum values here (detail in Chapter 4) |
| Chapter 4 | Flags & Pattern Tables | Show flag/status value ↔ business meaning mapping | Base format: 3 columns (value, meaning, constant name) |
| Chapter 5 | Business Pitfalls | Pre-emptively prevent mistakes developers will actually make | Minimum 3 pitfalls required. Attach code evidence to each |
| Chapter 6 | SQL Samples | Present typical business queries | Each query requires "usage comment." delflag='0' mandatory |

#### Chapter 3 Column Selection Criteria (Column Grain for Section 3)

In investigation phase, verify ALL columns (Step 2 obligation). In writing phase, select columns using these criteria.

| Category | Column Type | How to Write |
| :--- | :--- | :--- |
| Must Include | Flag/enum/state columns, foreign keys, business-branch columns | 1 row per column. State "business meaning" explicitly. |
| Can Group | Generic management columns (id PK, created_at, updated_at, delflag, etc.) | "Generic management columns (id, created_at, updated_at, delflag)" as 1 row |
| Can Omit | Low-business-meaning columns (mechanical serial numbers, internal counters) | Omit, BUT state the omission fact clearly ("N additional columns exist but business meaning unclear") |

> **Important**: Selection criteria apply only to WRITING. Investigation step (Step 2) must still verify all columns.

#### Path Differences - Symmetric Structure

When behavior differs across channels/routes, do NOT aim for symmetric formatting. Aim for accurate business model communication.

**Principles**:
- Declare path differences in Chapter 1's comparison table (3-route explanation)
- In Chapter 3, separate sections for path-specific entity column definitions (3.1 organizations [direct/teacher], 3.2 groups [partner])
- Shared flags defined once in Chapter 4. Path-specific flags explained in each path section.
- If path difference is fundamental (example: organizations vs groups entity), don't force artificial symmetry.

**When Multiple Routes Share One Entity**:

When one table serves multiple routes (example: organizations table manages both direct and teacher routes, distinguished by society_contracts.type = 1/2):

- In table column "business meaning," embed path-specific meanings (example: `type` column's meaning reads "1=direct contract / 2=teacher-plan contract")
- If business meaning divergence is large, split Chapter 3 section (example: 3.1.1 Direct / 3.1.2 Teacher)
- Rationale: Direct/Teacher share Society entity; distinguished by SocietyContract type value (full system 3-route explanation in Chapter 1 Section 1.3)

**Cross-Database Cases - Additional Chapter**:

When domain spans multiple databases (maindb / partnerdb / correlateddb):

Insert chapter before Chapter 2 ER diagram:

```
Chapter 2: Cross-System Join Keys (multi-database only)
  2.1 Join key list across databases (physical db, table name, join key, business meaning)
  2.2 ER diagram (including cross-database relationships)
```

**Axis Check (Must Confirm Before Step 3 Completion)**:
- [ ] After reading Chapter 1, can a developer explain "this domain's business structure?"
- [ ] Does Chapter 3's column definition include "business meaning" for all tables?
- [ ] Does Chapter 4's flag table show value meanings (not just "0/1/2," but "what they mean")?
- [ ] Are there minimum 3 pitfalls in Chapter 5?
- [ ] Does Chapter 6's SQL have usage comments?

---

### Step 4: Mermaid Verification (T29 Required)

**Purpose**: Detect ER diagram mermaid syntax errors before draft submission.

**Note**: This step is ONLY for mermaid syntax verification. Business quality confirmation is already done in Step 3 axis check.

**Execution**:
```
mmdc -i [created SEM-XX file] -o /tmp/sem_check.png
```

**PK/FK Notation Rules** (from PR #148 violation history):
- Composite PK/FK: `int org_id PK,FK` (comma-separated, no space)
- Single PK: `int id PK`
- Single FK: `int organization_id FK`

**If errors ≠ 0: Return to Step 3 for correction. Do NOT create PR with errors.**

**Axis Check (Must Confirm Before Step 4 Completion)**:
- [ ] mmdc reports 0 errors?
- [ ] ER diagram relationship lines accurately represent business relationships (1-1, 1-many, many-many)?

---

### Step 5: Forbidden Term Check & PR Creation

**Purpose**: Prevent prohibited terms from entering knowledge repository (source of truth). Create PR.

**Execution**:
1. Forbidden term check (verify 0 matches):
   ```
   grep -rn "[forbidden-term-1]\|[forbidden-term-2]\|[personal-path]" <repo-clone>/knowledge/domain/semantic/SEM-XX_*.md
   ```
2. Git operations (git -C pattern mandatory):
   ```
   git -C <repo-clone> checkout -b docs/sem<number>-<domain-name>-domain
   git -C <repo-clone> add knowledge/domain/semantic/SEM-XX_<domain-name>_domain_model.md
   git -C <repo-clone> commit -m "docs: SEM-XX <domain-name> semantic model creation"
   gh pr create --repo owner/knowledge-repo --title "docs: SEM-XX <domain-name> semantic model" --body "..."
   ```

**Forbidden Comments in Bash**: Do NOT embed # comments in git/gh commands. Describe separately in text.

**Axis Check (Must Confirm Before Step 5 Completion)**:
- [ ] Forbidden term grep shows 0 matches?
- [ ] PR title and body explain the semantic model's purpose (convey business meaning)?

---

## Output Format (Chapter Structure Template)

Deliverable file path: `knowledge/domain/semantic/SEM-XX_<domain-name>_domain_model.md`

```markdown
# SEM-XX <Domain Name> Semantic Model

> **Semantic Model Number**: SEM-XX
> **Domain**: <domain-name>
> **Created**: YYYY-MM-DD
> **Accuracy**: Real Code Investigation (<target repositories>)
> **SSOT**: This file is the single source of truth for SEM-XX concept model, ER diagram, and flag value definitions

---

## Chapter 1: Overview

### 1.1 Domain Purpose

[Why does this domain exist in business terms? 1-3 sentences explaining "what concept this is for?" Do NOT start with table names.]

### 1.2 [Main Domain Terms (Optional)]

[For ubiquitous language (axis 2), state main domain terms (Organization / Group / Party, etc.). Explain each term's business meaning in 1 line. Omit this section if terms are self-evident.]

### 1.3 [If path differences exist] Route-Specific Management Models

| Route | Management Entity | Management Screen | Business Characteristic |
| :--- | :--- | :--- | :--- |
| Direct | [table name] | [management screen] | [business rule unique to this route] |
| Teacher Plan | [table name] | [management screen] | [business rule unique to this route] |
| Partner | [table name] | [management screen] | [business rule unique to this route] |

[If no path differences, replace this section with 1.3 Main Entity List]

### 1.4 Main Table List

| Table Name | Model File | Role | Physical DB | Management Authority |
| :--- | :--- | :--- | :--- | :--- |
| [table name] | [Model.php] | [business role in 1 sentence] | maindb / partnerdb etc | Organization/Partner etc |

**Boundary Exclusion** (SSOT in other SEMs):
- [table name]: [Why excluded]. See [SEM-XX] for details

---

## Chapter 2: [If cross-database] Cross-System Join Keys

### 2.1 Join Keys Across Databases

| DB A | table.column | DB B | table.column | Business Meaning |
| :--- | :--- | :--- | :--- | :--- |

### 2.2 ER Diagram

[If no cross-database span, this chapter becomes "Chapter 2: ER Diagram" with ER diagram only]

```mermaid
erDiagram
    [TableA] {
        int id PK
        int [foreign_key] FK
        string [column_name]
    }
    [TableB] {
        int [TableA_id] PK,FK
    }
    [TableA] ||--o{ [TableB] : "[business relationship description]"
```

---

## Chapter 3: Table-by-Table Column Definitions

### 3.1 [Table Name] ([business role in one phrase])

[Business role of this table in 1-2 sentences]

| Column | Type | Business Meaning | Source |
| :--- | :--- | :--- | :--- |
| id | int PK | [business meaning] | [Model.php L line] |
| [column name] | [type] | [business meaning. value meaning, what conditions change this, etc] | [source] |

**[Notes if needed]**: [logical deletion, timestamp naming convention, etc.]

---

## Chapter 4: Flags & Pattern Tables

### 4.1 [Table Name].[Column Name] ([flag's business role in one phrase])

| Value | Business Meaning | Constant Name | Source |
| :--- | :--- | :--- | :--- |
| 0 | [meaning] | [CONSTANT_NAME or "no constant"] | [source] |
| 1 | [meaning] | [CONSTANT_NAME or "no constant"] | [source] |

**Note**: [business pitfall when using this flag]

---

## Chapter 5: Business Pitfalls

### T-1: [Pitfall Title]

[Pitfall detail. "Why it's a pitfall" and "how to avoid." Attach code evidence (Model.php L line)]

| [comparison axis] | [choice A] | [choice B] | Source |
| :--- | :--- | :--- | :--- |

### T-2: [Next Pitfall]

[Minimum 3 required: T-1, T-2, T-3 mandatory. Additional pitfalls in business importance order]

---

## Chapter 6: SQL Samples

All samples below are raw PostgreSQL. Laravel's Eloquent global scopes are not auto-applied to raw SQL; you MUST manually add `delflag = '0'` for logical deletion tables.

### SQL-1: [One-phrase usage]

[Usage explanation: why is this query needed?]

```sql
SELECT
    [column]
FROM [table]
WHERE [table].delflag = '0'
  AND [business condition]
ORDER BY [table].id ASC;
```

---

## Related Domains

- [Related SEM-XX files]: [relationship explanation]

---

## Update History

| Date | Content |
| :--- | :--- |
| YYYY-MM-DD | Initial creation |
```

---

## Quality Checkpoint (Checklist for Axis Compliance)

Before worker submits deliverable to leader, confirm all items. Submit only when status is "completed."

### 6.1 Axis Compliance Check (Most Critical)

| # | Check Item | Verification Method |
| --- | --- | --- |
| 1 | Can developer explain "this domain's business structure" after reading Chapter 1? | Self-check: After reading Chapter 1, can you explain "why does this entity exist?" |
| 2 | Is this a column list (data dictionary) rather than business meaning? | Check: Does every table have "business meaning" column? Or is it "table has X columns"? |
| 3 | Is ubiquitous language (business terminology) defined? | Check: Is Chapter 1 Section 1.2 present, OR is business terminology embedded in Chapter 3's column descriptions? |
| 4 | Are business rules stated? | Check: Are flag conditionals, state transitions, must-have conditions in Chapter 4 & 5? |
| 5 | Are path differences shown as business model differences? | Check: Do direct/teacher/partner differences show "business meaning difference" not just "different tables?" |
| 6 | Is scope determined by business concept completeness, not table physical location? | Self-check: "This table is in maindb so it's in scope" - if you thought this, you failed. Decide by "are business concepts self-contained?" |
| 7 | If system integration exists, is business meaning explained? | Check: Do cross-database join keys include "business meaning" explanation? |

### 6.2 Technical Quality Check

| # | Check Item | Verification Method |
| --- | --- | --- |
| 8 | Did you actually verify all columns (not sample)? | Check: Is there evidence of reading ALL fillable/hidden/casts? |
| 9 | Did you verify flag values from actual code? | Check: Is there evidence of reading constants from Model.php? |
| 10 | Is mermaid PK,FK notation correct? | Check: Is it `PK,FK` (comma, no space)? |
| 11 | Did mmdc verification show 0 errors? | Check: Is mmdc command result (0 errors) included in report? |
| 12 | Are forbidden terms 0 count? | Check: Is grep result (0 matches) included in report? |
| 13 | Are minimum 3 pitfalls in Chapter 5? | Check: T-1, T-2, T-3 all written? |
| 14 | Do Chapter 6 SQLs have usage comments? | Check: Each SQL has "usage:" in explanatory comment? |
| 15 | Do Chapter 6 SQLs include delflag='0'? | Check: All logical-deletion tables have manual `delflag = '0'` condition? |
| 16 | If columns omitted, is omission fact stated? | Check: If business-meaning-weak columns skipped, is note like "N additional columns present but business meaning unclear?" |
| 17 | If multiple routes share one table, are meanings route-specific? | Check: When type=1/2 distinguishes routes, does "business meaning" column show "1=XXX / 2=XXX" format? |

---

## I/O Specification

### INPUT

| Type | Content | Required/Optional | Example |
|------|---------|-------------------|---------|
| Domain Name | Target business domain name | Required | `organization`, `contract` |
| Main Table Names | Core table names (multiple allowed) | Required | `organizations groups` |
| --db Option | Target database (specify if multi-database) | Optional | `--db partnerdb` |
| SEM Number | Pre-assigned number | Optional | `01`, `10` |

### OUTPUT

| Type | Format | Destination |
|------|--------|-------------|
| Semantic Model | 6-chapter Markdown (ER diagram, column defs, flag tables, pitfalls, SQL samples) | Knowledge repository `knowledge/domain/semantic/SEM-XX_<domain-name>_domain_model.md` |
| Work Report | Command execution & verification results | stdout (report to leader) |

### Prerequisites

- Serena MCP is running
- Knowledge repository clone is up-to-date (`git -C /path/to/repo pull origin main`)
- Knowledge repository clone is on main branch (no work branches present)

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|--------|---------|----------|
| `/doc-check` | After SEM-XX PR merges | Leader proposes semantic model alignment verification |

> **Fallback**: If mmdc unavailable, report to leader and request manual review. Do NOT create PR without mmdc confirmation.

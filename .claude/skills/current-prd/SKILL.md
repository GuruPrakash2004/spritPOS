---
name: current-prd
description: Reverse-engineer a PRD (Product Requirements Document) from an existing codebase by scanning code structure, APIs, services, and DB schemas to create structured specification documents
argument-hint: "[repository-name] [scope: full | module-name]"
---

# /current-prd — Code to Specification Generation Skill

## Overview

Reverse-engineer a PRD (Product Requirements Document) from an existing codebase. Instead of writing specifications from scratch, extract actual behavior from code and organize it as a maintainable, living specification document.

## When to Use

- Onboarding to a legacy codebase (when documentation is lagging)
- Updating obsolete specifications to match current implementation
- Creating baseline specifications before refactoring
- Closing the gap between documented expectations and actual implementation

## Role Assignment

| Role | Responsibility |
|------|----------------|
| **Shogun (General)** | Scope judgment, phase progression, reporting to stakeholders. Does not perform code exploration directly (F002 rule) |
| **Karo (Chief Retainer / Planner)** | Validates phase decomposition relative to target repository size and complexity. Pre-reviews module decomposition |
| **Ashigaru (Foot Soldier / Worker)** | Executes each phase (code scanning, module analysis, PRD assembly). Writes to reports/ |
| **Metsuke (Inspector)** | Final PRD quality audit: detect hallucinations (verify path references, symbol names, table names exist). Quality assurance |

### Language
Business-style, fact-based reporting. Use "OK" or "NG" clearly. When uncertain, state explicitly.

## Three-Phase Workflow

### Phase 1: Global Scan (Automated Collection)

Extract facts that are collectible without human input:

1. **Technology Stack**: Package managers (composer.json, package.json, go.mod), framework versions, language versions
2. **Entry Points**: routes/api.php, web.php, CLI commands, batch scripts
3. **Infrastructure**: docker-compose.yml, Dockerfile, .env.example
4. **Database**: Schema files, migration history, table/column inventory
5. **Directory Structure**: Top-level organization, naming conventions

Output: `reports/current-prd-{repository}-phase1.md`

### Phase 2: Module Analysis (Deep Dive)

For each major module/Service discovered in Phase 1:

1. **Controller -> Service -> Repository Chain**: Trace call hierarchy
2. **Input/Output Contracts**: Request validation, response shape, error handling
3. **Database Operations**: Tables read/written, JOIN patterns, transaction boundaries
4. **External Integrations**: API calls, SFTP, S3, SQS, email
5. **Business Rules**: Conditional logic, flag-based branching, calculations
6. **Cross-Repository Dependencies**: Shared DB tables, inter-repository API calls

#### Analysis Items and Execution Procedure

| # | Analysis Item | Execution Procedure | Extraction Target |
|---|-------------|-------------------|------------------|
| 1 | Callers | Use Serena find_referencing_symbols recursively on target Service/Controller to enumerate callers | Upstream Controllers, Middleware, CLI Commands, other Service calls |
| 2 | Input/Output Contracts | Use Serena find_symbol to locate Request/FormRequest classes; Read to verify validation rules and Response type/structure | Validation rules, response JSON structure, HTTP status codes, error response format |
| 3 | DB Operations | grep for `DB::`, `->table(`, `->join(`, `->where(`, ORM model names. Search `DB::transaction` / `DB::beginTransaction` for transaction boundaries | Table names read/written, JOIN conditions, WHERE conditions, transaction scope |
| 4 | External Integrations | grep for `curl`, `Http::`, `sftp`, `S3`, `SqsQueue`, `Mail::`, `Storage::` keywords | Connection URLs/bucket names, data format sent, error handling approach |
| 5 | Business Rules | Read conditional logic (if/switch/match) in target Service. Use Serena find_symbol to locate Enum/constant classes | Branch conditions, flag names and meanings, formulas, state transitions |

#### Scope Limitation

Analyzing all Services at equal depth wastes tokens. Filter analysis targets by this criterion:

- **Deep Analysis Target**: Services discovered in Phase 1 with high route count, high incoming reference count, or external integrations (target: top 30%)
- **Overview Only**: Remaining Services recorded as file list, endpoint list, main table names only
- **Decision Basis**: Use Phase 1 quantitative results (route count, file LOC, reference count)

Output: `reports/current-prd-{repository}-phase2.md`

### Phase 3: Structured Output (PRD Assembly)

Organize findings into a knowledge framework:

1. **System Overview**: Map to system architecture documentation
2. **Architecture and Structure**: Map to repository structure documentation
3. **Data Flow and Behavior**: Map to data flow and behavior documentation
4. **API Specification**: Endpoint list, request/response schema
5. **Database Model**: Table relationships, key columns, business meaning
6. **Batch Process Specification**: Schedule, input/output, dependencies
7. **Cross-Repository Dependency Map**: Which repositories read/write shared resources

Output: `reports/current-prd-{repository}-final.md`

## Scope Control

Limit scope to manage token consumption in large codebases:

- `full`: Scan entire repository (small repositories, initial baseline)
- `[module-name]`: Limit to specific module/feature area (e.g., "order processing", "payment")

## Output Rules

- Output to `reports/` only. Never directly overwrite documentation directories
- Human reviews output before promotion via `/doc-organize` to knowledge base
- Describe observations only. Do not include application fix recommendations (assessment phase rule)
- Subject-First Rule: Always specify "whose" or "what" when mentioning domain terms

## Integration with Existing Skills

| Existing Skill | Relationship |
|------------|-----------|
| /current-spec | Point investigation and Service-level detail specs. current-prd provides system overview; current-spec performs deep dives |
| /doc-organize | Promotion pathway: reports/ -> knowledge base. Execute after current-prd output is reviewed |
| /current-legacy | Phase 0 auto-collection overlaps. If Phase 0 from current-legacy already completed, reuse its output as Phase 1 substitute. Use current-prd for initial investigation on new repositories |
| /current-distortion | current-prd investigation results are reused by current-distortion Phase 1 Ashigaru A. Run current-prd then current-distortion in sequence to avoid code double-scanning |

## I/O Specification

### INPUT
| Type | Description | Required |
|------|-------------|----------|
| Repository Path | Local path to target repository | Required |
| Scope | `full` or module name | Optional (default: full) |

### OUTPUT
| Type | Format | Destination |
|------|--------|-------------|
| Phase 1 Report | Markdown (tech stack, endpoints, DB tables) | `reports/current-prd-{repository}-phase1.md` |
| Phase 2 Report | Markdown (module analysis, dependencies) | `reports/current-prd-{repository}-phase2.md` |
| Final PRD | Markdown (architecture-mapped PRD assembly) | `reports/current-prd-{repository}-final.md` |

### Downstream Skills (Pipeline)

| Skill | Condition | Instruction |
|-------|-----------|-------------|
| `/doc-organize` | After stakeholder reviews and approves PRD | Report to Shogun (General) for approval, then execute to integrate into knowledge base |
| `/doc-update` | After promotion to knowledge base, if depth adjustment needed per user type | Propose to Shogun (General) and await judgment |
| `/current-distortion` | If PRD discovery reveals risks | Record findings; do not propose fixes. Report to Shogun (General) and recommend `/current-distortion` execution |

> **Fallback**: If prerequisites are unmet, report to Shogun (General) and await instructions

## Quality Checklist (for Inspector)

| # | Checkpoint | Verification Method | Pass Criteria | Failure Response |
|---|-----------|------------------|----------------|-----------------|
| 1 | All endpoints discovered and documented | Read routes/api.php, web.php; cross-reference endpoint count in PRD with actual route definitions. grep for endpoint declarations to detect omissions | Route definition count matches PRD endpoint count. Difference: 0 items | List omitted endpoints; issue correction instruction for Phase 2 re-analysis |
| 2 | DB tables mapped to business domain | Extract table list from schema file or migrations; cross-reference PRD domain mapping | All tables have business domain mapping (process name or entity name) | List unmapped tables; issue correction instruction for investigation |
| 3 | Cross-repository dependencies identified | grep for shared table names, API URLs, shared Enum names across repositories; cross-reference with PRD | PRD cross-repo dependencies verified in code. Undocumented dependencies: 0 | List undocumented dependencies with scope and impact; issue correction instruction |
| 4 | External integrations (SFTP, S3, API calls) documented | grep for `sftp`, `Storage::`, `S3Client`, `Http::`, `curl`, `Guzzle` etc.; cross-reference with PRD | All external integration points in code appear in PRD | List undocumented integration points with target and data format; issue correction instruction |
| 5 | Batch processes documented with schedule, I/O | Read scheduler config and CLI command directory; cross-reference with PRD | All batch commands have schedule, input source, and output destination documented | List incomplete batch command documentation; issue correction instruction for Phase 2 re-analysis |
| 6 | No application fix recommendations included (assessment only) | Full-text review; grep for prescriptive language ("should", "must change", "refactor") | Fix recommendations/improvement suggestions: 0. Contains fact descriptions and state recording only | Remove prescriptive language or rewrite as fact description. Issue correction instruction |

---

## Quality Assurance Criteria

Inspection rules for Metsuke (Inspector):

### Comprehensiveness
- Have all repositories been investigated?
- Have DB, Model, Service, and batch logic been covered?
- Have hardcoded values and edge cases been identified?

### Accuracy
- Is every claim verified against actual code (not assumptions)?
- Are file paths and line numbers documented?
- Have differences between repositories been cross-checked?

### Usability
- Is the impact classification logic clear?
- Are specific modifications documented?
- Are review points and deadlines stated?

### Service Specification Summary
- Do high-impact Service/UseCase entries have specification summaries?
- Do they include the four required elements: role, function, dependencies, special notes?

---

## Output Template: Risk Registration

For risks and concerns discovered during PRD analysis:

| ID | Risk Description | Discovery Date | Impact Level | Status | Resolution Strategy |
|----|------------------|---|---|---|---|
| R001 | [Risk detail] | YYYY-MM-DD | High/Medium/Low | Pending/In Progress/Resolved | [Strategy] |

**Example**:

| ID | Risk Description | Discovery Date | Impact Level | Status | Resolution Strategy |
|----|------------------|---|---|---|---|
| R001 | Schema migration incompleteness | 2026-06-15 | High | In Progress | Migration script pending (Phase 1) |
| R002 | External integration spec confirmation required | 2026-06-15 | Medium | Pending | Verification due by end of week |
| R003 | Existing test suite may fail | 2026-06-16 | Medium | In Progress | Test corrections prepared |

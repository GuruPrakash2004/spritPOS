# Project-Specific Conventions and Patterns

> **Created**: 2026-06-19
> **Purpose**: Document project-specific conventions, patterns, and risks for legacy decoding tasks
> **Customization**: This template should be adapted for your project's specific conventions

This document outlines patterns and conventions specific to your project that must be understood during legacy code analysis.

---

## Soft-Delete / Data Retention Convention

**Rule**: Your project uses soft-delete (logical deletion) rather than hard deletion. Records marked for deletion remain in tables and must be filtered from queries.

**Convention details** (customize for your project):
- Soft-delete column name (e.g., `soft_delete_flag`, `deleted_at`, `is_deleted`)
- Filter value for "active" records (e.g., `soft_delete='0'`, `deleted_at IS NULL`, `is_deleted=false`)
- Default behavior in ORMs and libraries

**Check targets** (Phase 1 investigation):
- When a query references tables, does the table have a soft-delete column?
- If the table has soft-delete column, does the query include the appropriate filter?

**Detection method** (implement in Phase 1):
- Grep for table name combined with `SELECT` / `UPDATE` / `DELETE` keywords
- Confirm SQL text includes soft-delete filter (especially in raw SQL cases)
- For ORM frameworks (e.g., Eloquent, Hibernate), verify that Model classes define soft-delete scope (e.g., local scope in Eloquent: `->where('soft_delete', 0)`)

**When soft-delete gap is detected**:
- Record as "Warning: missing soft-delete check detected"
- Include file:line reference, affected SQL statement, and suggested fix approach (reference only, not a requirement)
- In Phase 2 analysis, describe this as a behavioral fact: "This table query may return both active and deleted records" (observation of current implementation, not a fix recommendation)

---

## God Class Identification Criteria

**Definition**: A single class that carries multiple responsibilities across different layers (Domain / Application / Presentation) with multiple reasons to change. Common in legacy systems.

**Identification method** (implement in Phase 1 digging):
- Search class name with Serena, note file size and method count (>500 lines, >30 methods typical threshold)
- Extract method list with grep. Look for mixed concerns: "get/set", "validate", "save", "send", etc.
- Check change reasons (comments / git log). Multiple change drivers indicate multiple responsibilities (e.g., "updated for business logic", "updated for UI", "updated for external API")

---

## Multi-Repository Coordination for Single Features

**Background**: Your project may consist of multiple repositories where a single business feature spans multiple codebases. Different repositories may have different DB connection patterns, transaction management, and access controls.

**Scope confirmation pattern** (Phase 0 by Karo):

- **Primary repository**: Contains the core business logic. 80% of digging effort. Example considerations: API layer, core domain logic
- **Reference repository**: Calling side or integration point. Detailed digging not required. Example: presentation layer, client code
- **Out of scope**: Related integration exists but not necessary for this decoding task. Example: authentication service, logging system

**Switching perspectives during digging** (Phase 1):

1. **Primary repository**: Full Serena search, complete Call Hierarchy exploration, deep DB schema reading
2. **Reference repository**: Grep focused on "API call points only". Omit implementation details
3. **Out of scope**: Name resolution level only ("Cognito authentication is required" as a fact without tracing into auth system)

---

## Direct DB Access Pattern Classification

**Background**: Your project may use multiple DB connection patterns (ORM-based API / raw SQL / legacy system direct access / cache layers). Each pattern has different implications for data consistency, deletion handling, and access control.

**Pattern classification** (detect with Phase 1 grep):

| Pattern | Characteristics | Risk Level | Check Items |
|---------|-----------------|------------|------------|
| **ORM (type-safe)** | Model/Repository layer, type-checked queries | Low | Does soft-delete scope exist? |
| **Raw SQL (prepared)** | Parameterized queries, SQL string assembly | Medium | Soft-delete filter present? SQL injection risk? |
| **Legacy direct access** | Direct DB connection from old systems | High | Soft-delete checks difficult to confirm |
| **Cache / In-memory** | Cached data, e.g., Redis | Medium | Cache key expiration? Invalidation on data change? |

---

## Database Query and Access Patterns

**Common patterns to investigate**:

- **Read patterns**: How are records fetched? With soft-delete filter? Pagination applied? Any caching?
- **Write patterns**: Which tables are updated? In which order? Transaction scope? Rollback behavior?
- **Batch operations**: Are records processed in batches? How are batches scheduled? Failure recovery?
- **Concurrent access**: Multiple systems updating same tables? Race condition potential?

---

## Version History

| Date | Version | Changes |
|------|---------|---------|
| 2026-06-19 | 1.0 | Initial version. Template for project-specific conventions documentation. |

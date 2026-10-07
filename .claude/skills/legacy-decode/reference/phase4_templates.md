# Phase 4 Template Groups -- Deliverable Creation (Scribe Phase)

> **Created**: 2026-06-01

Converts Phase 3 APPROVED deliverables into final deliverables. Handled by Ashigaru (scribe).

## 4.1 Reverse-Engineered Spec Template

Target: `knowledge/domain/[feature-area]/reverse_engineered_spec_[feature-name].md`

```markdown
# Reverse-Engineered Specification: [Feature Name / API Name]

> **Version**: 1.0
> **Generated**: YYYY-MM-DD
> **Target Repository**: [repository name]
> **Target Version**: main branch at YYYYMMDD
> **Investigator**: ashigaru-investigator
> **Auditor**: metsuke (Metsuke Inspector)

## Overview
[Describe the purpose of the feature in 1-2 sentences]

## Input Interface
- API endpoint (Method, Path, authentication)
- Input value constraints (required fields, types, value ranges)

## Business Logic
- Step 1-N: Explanation with code line numbers for each step

## Output Interface
- Normal response
- Side effects (S3 / email / batch, etc.)

## Error Handling
- Behavior for each error scenario

## Promises (Constraints) of Current Implementation
[Quote Phase 2 analysis results]

## Soft-delete Processing List
| Table | Soft-delete column present | Filter applied in queries | Notes |
|---------|:-:|:-:|--------|
| ... | yes/no | yes/no/n-a | ... |

## References
- [Source citations for code references]
```

## 4.2 Technical Debt Catalog Entry Template

Target: `knowledge/domain/technical_debt_catalog.md`

```markdown
## [Debt ID] Debt Name: [Pattern] Target

**Severity**: [high/medium/low]
**Complexity Score**: [0-10]
**Technical Debt Type**: God Class / Direct DB Access / Tight Coupling / Insufficient Testing
**Target File**: [path]
**Line Range**: L###-###

### Symptoms and Reality
[File size / number of methods / mixed responsibilities / change reasons from git log]

### Impact Scope
[Test complexity / propagation of changes / maintenance degradation]

### Position on Migration Priority Matrix
- Business importance: [high/medium/low]
- Technical debt severity: [high/medium/low]
- Migration effect: [high/medium/low]
- Migration cost: [high/medium/low]

### Evidence
- Investigator: ashigaru-investigator
- Investigation date: YYYY-MM-DD
- Source: reports/investigation/YYYYMMDD_[task-name]_phase2_analysis.md
```

## 4.3 Migration Priority Matrix Template

Target: `knowledge/domain/migration_priority_matrix.md`

Markdown + Mermaid diagram plotting on 2 axes (Y: business importance / X: technical debt severity).

Quadrant 1 (high x high) -> Priority A / Quadrant 2 (high x low) -> Priority B / Quadrant 3 (low x low) -> Priority C / Quadrant 4 (low x high) -> Priority B

For each item, include "feature", "business importance", "technical debt", "migration effect", "migration cost", "judgment", and "evidence".

## 4.4 Continuous Update Checklist (Approach X)

When persisting deliverables from this skill (reverse-engineered specs, technical debt catalog, migration priority matrix) to knowledge/domain/, follow the "Continuous Update Approach X" below.

**Pattern 6: Adding domain knowledge files**
- Updating knowledge/README.md Part 4 (navigation link collection) is required
- Create a `knowledge/domain/[feature-area]/` directory when creating new reports
- Updating birdseye_system_overview.md Part 4 is recommended

**Pattern 2 equivalent - separate operation: Linking to business process diagrams**
- May require mandatory update to birdseye_business_process.md
- Addition and modification of newly discovered business processes are independent update tasks on the wave map

**Version updates for reverse-engineered specs**
- When an existing spec is present and new Phase 0 findings exist, update the version tag (v1.0 -> v1.1)

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated Phase 4 template groups from SKILL.md. §4.5 source core mechanism grep verification retained in main body |

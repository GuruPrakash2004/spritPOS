# takt Adaptation Reference Table -- Connecting Source and Team Implementation

> **Source**: `nrslib/takt` main HEAD `7cd901887...` (2026-05-29)
> **Correspondence confirmed**: `reports/investigation/20260531_takt_full_acquisition.md` (ST-A1)

This document is an adaptation table connecting the four core mechanisms of the OSS library `nrslib/takt` with their implementation in the Shogun/Karo/Ashigaru/Metsuke team structure.

## Four Adaptation Domains

| Domain | Adopted from Source (as-is) | Items Adapted for Environment Differences |
| :--- | :--- | :--- |
| **Loop structure** | 4-stage loop of plan->dig->analyze->supervise, 2-system rollback (on_revision / on_rejection) | takt runtime (Node.js EventEmitter state machine) -> adapted to Agent team (Karo/Ashigaru/Metsuke/Scribe) execution structure |
| **verdict 3-value convention** | Issuance rules and judgment criteria for APPROVED / NEEDS_REVISION / REJECTED | TypeScript Enum TransitionType 3-value -> adapted to Markdown text representation |
| **facet 5-type synthesis** | 5-layer facet structure of personas / policies / knowledge / instructions / output-contracts | takt builtins (JSON files) -> adapted to Markdown sections + Serena symbolic search instructions |
| **Loop Detector** | 3-layer runaway prevention, max_iterations control, stuck detection for same question | TypeScript counter mechanism -> Markdown guide + Shogun cycle limit monitoring |

## facet 5 Types and Team Adaptation Correspondence

| facet Type | Role in takt (ST-A1 §3.8/§3.9 source) | Corresponding Implementation in Adaptation |
| :--- | :--- | :--- |
| **personas** | Researcher behavioral personas (planner / digger / analyzer / supervisor) | Roles and Responsibilities table: responsibility definitions for Karo, Ashigaru, Metsuke, and Scribe |
| **policies** | Execution policies (prerequisites, prohibitions, delegate-mode rule, etc.) | Prerequisites + Tone + Relationship with Existing Skills |
| **knowledge** | Known knowledge base (legacy patterns / domain knowledge) | Phase 0 Scope section, takt source report reference, persistence to knowledge/domain/ |
| **instructions** | Execution procedures for each phase, search queries, extraction rules | Phase 1-4 detailed sections (under reference/) |
| **output-contracts** | Output format, quality requirements, input requirements for next phase | Phase 3 quality gate definition, Phase 4 deliverable format |

## Loop Structure Correspondence Table

| Element | takt research workflow (ST-A1 §5.2) | Adaptation |
| :--- | :--- | :--- |
| **Basic loop** | 4-stage plan->dig->analyze->supervise | Phase 0->1->2->3 |
| **verdict APPROVED** | Research complete, output contract satisfied | Phase 3 APPROVED -> advance to Phase 4 |
| **verdict NEEDS_REVISION** | Minor deficiency, additional investigation needed | on_revision: roll back to Phase 1 |
| **verdict REJECTED** | Fundamental deficiency, plan revision required | on_rejection: roll back to Phase 0 |
| **Default when no marker** | needs_revision (safe side) | needs_revision (safe side) |
| **3-layer runaway prevention** | max_iterations=3, FND-LOOP, forced completion | Phase loop max 3 cycles / same question 2 cycles unchanged = stuck / forced completion when exceeded |

## Items Adapted for Environment Differences

takt runtime layer -> adapted to execution structure of team-based agent approach:

- EventEmitter / FSM state transitions -> Agent team (Karo/Ashigaru/Metsuke/Scribe) role assignments
- Node.js promise / async-await control -> Shogun's sequential checkpoint confirmation control
- TypeScript Enum / Transition classes -> Markdown text representation (APPROVED / NEEDS_REVISION / REJECTED)
- JSON file system (builtins/) -> Markdown sections + .claude/agents/ configuration

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated takt adaptation table from SKILL.md |

# Phase 3 Detailed Procedures -- Quality Gate (Metsuke Phase)

> **Created**: 2026-06-01

Metsuke audits Phase 1-2 digging and analysis deliverables and issues a verdict.

## 3.1 Independent Audit (grep / Serena evidence verification)

Metsuke performs the following 2 independent confirmations in parallel (not solely relying on Ashigaru's self-report):

**Confirmation system 1: Evidence existence verification (hallucination prevention)**
- For all citations where Ashigaru recorded "confirmed soft-delete check at line L234" or similar, Metsuke independently verifies existence with grep / Serena
- When Ashigaru's reported line numbers and Metsuke's actual verification results conflict, record as "missing citation (finding_id: FND-L0001)" and include in rejection

**Confirmation system 2: Scope fulfillment verification (completeness)**
- For the approximately 30 questions listed by Karo in Phase 0, confirm whether Ashigaru's report has "answers" with code evidence
- For each question, record in Ashigaru's answer section: whether file-path:line-number evidence exists / whether inference notes exist / whether 3 or more lines of actual code citation exist
- If all three sub-items are YES, mark as "high quality answer"; if any are missing, mark as "insufficient evidence"

**Confirmation system 3: Soft-delete check compliance verification**
- Compliance with your project's soft-delete convention (if applicable) for all tables
- Read the "soft-delete warning" section of Ashigaru's report

## 3.3 NEEDS_REVISION vs REJECTED Judgment Criteria

| Perspective | NEEDS_REVISION (-> Phase 1) | REJECTED (-> Phase 0) |
| :--- | :--- | :--- |
| **Answer rate** | 60-79% scope coverage | Less than 60% coverage |
| **Question quality** | Weak evidence for specific questions | Fundamental missing questions |
| **Technical issues** | Known small gaps (resolvable with 1-3 additional digs) | Contradiction in design intent |
| **Time required** | 1-2 hours of additional investigation | Plan revision |

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated Phase 3 details from SKILL.md. §3.2 verdict and §3.4 3-layer runaway prevention retained in main body |

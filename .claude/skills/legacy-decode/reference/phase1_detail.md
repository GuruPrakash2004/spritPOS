# Phase 1 Detailed Procedures -- Digging (Ashigaru Phase)

> **Created**: 2026-06-01
> **Target**: Detailed procedures for Phase 1 (fact extraction performed by Ashigaru investigator)

Ashigaru extracts facts necessary for blackbox decoding according to Karo's plan. Acts fully autonomously. Records as "not obtained" even when unable to determine, without stopping.

## 1.1 Fact Extraction (Serena symbol search, grep, call hierarchy)

**Recommended execution order**:

1. **Identify Entry Point with Serena** (highest priority)
   - Symbol search for the API endpoint name (e.g., `POST /api/order`) or function name (e.g., `function createOrder`) specified by Karo.
   - Note the obtained file path and line number. Serena has accurate indexes, making it more reliable than grepping.

2. **Trace call chain with Call Hierarchy**
   - Trace all functions called from the Entry Point (maximum depth 3-5).
   - Record "what this function does" in one line for each call.

3. **Supplement with Grep for cross-file search**
   - Search all repositories with `grep -rn` using function names, table names, and class names.
   - Detect missed calls and hidden references (metaprogramming, reflection, dynamic SQL).

4. **Check DB schema**
   - Once the table names operated by the function are identified, read the PostgreSQL (or equivalent) CREATE TABLE definition to confirm constraints, FKs, and indexes.
   - **Soft-delete check**: Does the table have a soft-delete column? If so, is the appropriate filter always included in queries? (See your project's coding conventions guide.)

**Serena search tips**:
- Search keywords: 4 axes of "function name", "class name", "API endpoint", "table name" - one search each.
- If too many results (>100), narrow by repository and search again.
- Grasp the overall picture with `get_symbols_overview` before proceeding to detailed search rather than `find_symbol`.

## 1.2 Checking Dynamic Dependencies (Direct DB access, raw SQL, batches, external APIs)

Confirm not only static calls in code, but also dependencies that occur dynamically at runtime.

**Finding direct DB access and raw SQL**:
- Search for patterns like `$pdo->query` / `DB::query` / `SQL(` with grep (varies by language/framework).
- When SQL is built dynamically (string concatenation, sprintf), confirm "what values may be inserted."
- Check for SQL injection risks and missing soft-delete filters (potentially fetching deleted records).

**Batch execution dependencies**:
- After reading the endpoint implementation, confirm "is this table also updated by any batch?"
- Find batch files (e.g., `app/Console/Commands/*.php`) with grep and list all batches operating the same table.
- Also confirm the batch execution schedule (cron configuration).

**External API calls**:
- Search for Curl / GuzzleHttp / other client names in functions with grep.
- Record the API destination URL, authentication method, and behavior on failure.

**Asynchronous queues and messaging**:
- If SQS / Kinesis / Redis queues are used, trace "at what point messages enter the queue and which consumer processes them."

## 1.3 Discovering Hidden Outputs (SFTP, S3, email, PDF, etc. side effects)

List side effects such as file generation, external service calls, and notification sending, separate from DB updates.

**Discovering S3 operations**:
- Search for AWS SDK calls like `putObject` / `getObject` / `deleteObject` with grep.
- What is written to which bucket / prefix. Is it a timestamped path or a fixed path?
- Also confirm conditions for file deletion (when it disappears).

**Email and notification sending**:
- Search for `Mail::send` / `Queue::push('SendEmailJob')` / Firebase / LINE API, etc. with grep.
- Also confirm template file location, subject, and information included in the body.
- Record behavior on send failure (retry / log only).

**PDF and form generation**:
- Search for library usage like `TCPDF` / `wkhtmltopdf` / `Puppeteer` with grep.
- Whether the generated file is saved to S3 or returned directly in the response.
- Also record template (HTML / Blade) location.

**File FTP / SFTP sending**:
- Search for SFTP sends to external systems with grep.
- File format (e.g., CSV / XML / PDF), send timing, and resend logic on failure.

---

## Version History

| Date | Version | Changes |
|------|:-------:|---------|
| 2026-06-01 | 1.0 | Initial creation. Separated Phase 1 details from SKILL.md. §1.4 fact recording retained in main body |

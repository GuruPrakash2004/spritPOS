"""Generate migrations/002_web_legacy_tables.sql: a Web_ copy of every legacy vLQMS table (schema only).

Run once, at the office, against the reference database (the one in backend/.env), and commit the output:
    .venv/bin/python tools/gen_web_schema.py

It only reads the catalogue (sys.tables, sys.columns, ...). For each user table it writes
CREATE TABLE dbo.Web_<Name> with the same columns (types, NULLs, identity, defaults, computed columns), the
primary key and the other indexes, plus the functions the computed columns call, the stock triggers on
Web_UtlStockList (they keep Web_TrnQOH, as tr_InsertQOH / tr_DeleteQOH keep TrnQOH) and the web-only tables.
The file holds no data, no server names and no credentials.
"""
import re
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from app import db  # noqa: E402  (loads backend/.env)

OUT = Path(__file__).resolve().parents[1] / "migrations" / "002_web_legacy_tables.sql"
SKIP = {"sysdiagrams", "dtproperties"}          # SQL Server's own diagram tables
# Columns made wider in the Web_ copy than in the desktop table: web document numbers ("26/PB-0000001") are
# longer than the desktop's ("2600025"). Only the web app uses the Web_ tables, so this is safe.
WIDEN = {("TrnHdrPB", "PBNO"): 30, ("TrnHdrSINV", "SINVNO"): 30, ("UtlStockList", "DocNo"): 30}
VERSION = 2


def q(sql: str) -> list[list]:
    return db.read_only(sql, max_rows=100000)["rows"]


def col_type(type_name: str, max_length: int, precision: int, scale: int) -> str:
    t = type_name.lower()
    if t == "sysname":
        return "NVARCHAR(128)"
    if t in ("varchar", "char", "varbinary", "binary"):
        return f"{t.upper()}({'MAX' if max_length == -1 else max_length})"
    if t in ("nvarchar", "nchar"):
        return f"{t.upper()}({'MAX' if max_length == -1 else max_length // 2})"
    if t in ("numeric", "decimal"):
        return f"{t.upper()}({precision},{scale})"
    if t in ("datetime2", "time", "datetimeoffset"):
        return f"{t.upper()}({scale})"
    return t.upper()


def web(name: str) -> str:
    return f"Web_{name}"


def main() -> None:
    if not db.init_db():
        sys.exit("Could not connect to SQL Server (check backend/.env).")
    tables = [r[0] for r in q("SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') "
                              "AND name NOT LIKE 'Web[_]%' ORDER BY name") if r[0] not in SKIP]
    cols = q("""SELECT t.name, c.column_id, c.name, TYPE_NAME(c.user_type_id), c.max_length, c.precision, c.scale,
                       c.is_nullable, c.is_identity, CAST(ic.seed_value AS varchar(40)), CAST(ic.increment_value AS varchar(40)),
                       dc.definition, cc.definition, cc.is_persisted
                FROM sys.columns c JOIN sys.tables t ON t.object_id = c.object_id
                LEFT JOIN sys.identity_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                LEFT JOIN sys.default_constraints dc ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                LEFT JOIN sys.computed_columns cc ON cc.object_id = c.object_id AND cc.column_id = c.column_id
                WHERE t.schema_id = SCHEMA_ID('dbo') ORDER BY t.name, c.column_id""")
    idx = q("""SELECT t.name, i.index_id, i.name, i.type_desc, i.is_primary_key, i.is_unique_constraint, i.is_unique,
                      i.filter_definition, ic.key_ordinal, ic.is_included_column, ic.is_descending_key, c.name
               FROM sys.indexes i JOIN sys.tables t ON t.object_id = i.object_id
               JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
               JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
               WHERE i.type > 0 AND i.is_hypothetical = 0 AND i.is_disabled = 0 AND t.schema_id = SCHEMA_ID('dbo')
               ORDER BY t.name, i.index_id, ic.is_included_column, ic.key_ordinal, ic.index_column_id""")
    by_table: dict[str, list] = {}
    for r in cols:
        by_table.setdefault(r[0], []).append(r)
    indexes: dict[str, dict[int, dict]] = {}
    for t, iid, name, kind, pk, uqc, uq, filt, _ord, incl, desc, col in idx:
        i = indexes.setdefault(t, {}).setdefault(iid, {"name": name, "kind": kind, "pk": pk, "uqc": uqc, "uq": uq,
                                                        "filter": filt, "keys": [], "incl": []})
        (i["incl"] if incl else i["keys"]).append(f"[{col}]" if incl else f"[{col}]{' DESC' if desc else ''}")

    # Functions the computed columns call (e.g. dbo.RemoveNonAlphaCharacters), created only if missing.
    used_funcs = sorted({m for r in cols if r[12] for m in re.findall(r"\[dbo\]\.\[(\w+)\]", r[12])})
    funcs = {name: q(f"SELECT OBJECT_DEFINITION(OBJECT_ID('dbo.{name}'))")[0][0] for name in used_funcs}

    out: list[str] = [
        "-- 002_web_legacy_tables.sql: a Web_ copy of every legacy vLQMS table, same columns (schema only).",
        "-- GENERATED by tools/gen_web_schema.py from the reference database. Don't edit by hand: re-generate.",
        "-- The web app reads and writes only these Web_ tables; the legacy tables are only read, once, by the",
        "-- data copy (app/setup_db.py). Safe to run twice: every CREATE is guarded. Undo: 002_web_legacy_tables.rollback.sql.",
        "-- Holds no data, server names or credentials.",
        "",
        "IF OBJECT_ID('dbo.Web_SchemaVersion', 'U') IS NULL",
        "CREATE TABLE dbo.Web_SchemaVersion (",
        "    Version    INT       NOT NULL CONSTRAINT PK_Web_SchemaVersion PRIMARY KEY,",
        "    AppliedAt  DATETIME2 NOT NULL CONSTRAINT DF_Web_SchemaVersion_AppliedAt DEFAULT SYSUTCDATETIME()",
        ");",
        "GO",
    ]
    for name, definition in funcs.items():
        body = definition.replace("'", "''")
        out += [f"-- Function used by computed columns ({name}); created only when the database doesn't have it.",
                f"IF OBJECT_ID('dbo.{name}') IS NULL",
                f"EXEC('{body}');", "GO"]

    for t in tables:
        lines = []
        for (_t, _id, c, typ, ml, prec, sc, nullable, ident, seed, inc, default, computed, persisted) in by_table[t]:
            if computed:
                lines.append(f"    [{c}] AS {computed}{' PERSISTED' if persisted else ''}")
                continue
            if (t, c) in WIDEN:
                ml = WIDEN[(t, c)]
            s = f"    [{c}] {col_type(typ, ml, prec, sc)}"
            if ident:
                s += f" IDENTITY({seed},{inc})"
            s += " NULL" if nullable else " NOT NULL"
            if default:
                s += f" DEFAULT {default}"
            lines.append(s)
        pk = next((i for i in indexes.get(t, {}).values() if i["pk"]), None)
        if pk:
            kind = "CLUSTERED" if pk["kind"] == "CLUSTERED" else "NONCLUSTERED"
            lines.append(f"    CONSTRAINT [PK_{web(t)}] PRIMARY KEY {kind} ({', '.join(pk['keys'])})")
        out += [f"IF OBJECT_ID('dbo.{web(t)}', 'U') IS NULL", f"CREATE TABLE dbo.[{web(t)}] (", ",\n".join(lines), ");", "GO"]
        others = []   # (name, CREATE INDEX statement, its columns)
        for n, i in enumerate(v for v in indexes.get(t, {}).values() if not v["pk"]):
            iname = f"IX_{web(t)}_{n + 1}" if i["uqc"] else i["name"]
            kind = "CLUSTERED" if i["kind"] == "CLUSTERED" else "NONCLUSTERED"
            s = f"CREATE {'UNIQUE ' if i['uq'] else ''}{kind} INDEX [{iname}] ON dbo.[{web(t)}] ({', '.join(i['keys'])})"
            if i["incl"]:
                s += f" INCLUDE ({', '.join(i['incl'])})"
            if i["filter"]:
                s += f" WHERE {i['filter']}"
            others.append((iname, s, " ".join(i["keys"] + i["incl"])))
            out += [f"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.{web(t)}') AND name = '{iname}')",
                    s + ";", "GO"]
        # A copy made before a column was widened: widen it in place (dropping and re-making its indexes).
        for (wt, wc), width in WIDEN.items():
            if wt != t:
                continue
            col = next(r for r in by_table[t] if r[2] == wc)
            null = "NULL" if col[7] else "NOT NULL"
            body = []
            if pk and f"[{wc}]" in " ".join(pk["keys"]):
                body.append(f"    ALTER TABLE dbo.[{web(t)}] DROP CONSTRAINT [PK_{web(t)}];")
            hit = [(n, stmt) for n, stmt, cols in others if f"[{wc}]" in cols]
            body += [f"    DROP INDEX [{n}] ON dbo.[{web(t)}];" for n, _ in hit]
            body.append(f"    ALTER TABLE dbo.[{web(t)}] ALTER COLUMN [{wc}] {col_type(col[3], width, col[5], col[6])} {null};")
            if pk and f"[{wc}]" in " ".join(pk["keys"]):
                kind = "CLUSTERED" if pk["kind"] == "CLUSTERED" else "NONCLUSTERED"
                body.append(f"    ALTER TABLE dbo.[{web(t)}] ADD CONSTRAINT [PK_{web(t)}] PRIMARY KEY {kind} ({', '.join(pk['keys'])});")
            body += [f"    {stmt};" for _, stmt in hit]
            out += [f"-- Widen {web(t)}.{wc} to {width} (web document numbers) on a copy made before it was widened.",
                    f"IF COL_LENGTH('dbo.{web(t)}', '{wc}') < {width}", "BEGIN", *body, "END", "GO"]

    out += [
        "-- Stock balance: every row added to / removed from Web_UtlStockList moves Web_TrnQOH.CLBal, as the",
        "-- desktop's tr_InsertQOH / tr_DeleteQOH do for UtlStockList / TrnQOH.",
        "IF OBJECT_ID('dbo.tr_Web_InsertQOH', 'TR') IS NULL",
        "EXEC('CREATE TRIGGER dbo.tr_Web_InsertQOH ON dbo.Web_UtlStockList FOR INSERT AS",
        "BEGIN",
        "    SET NOCOUNT ON;",
        "    INSERT INTO dbo.Web_TrnQOH (ITEMID, LOCID, OPQty, TrnQty, CLBal)",
        "    SELECT i.ItemID, i.LOCID, 0, 0, 0 FROM inserted i",
        "    LEFT JOIN dbo.Web_TrnQOH q ON i.ItemID = q.ITEMID AND i.LOCID = q.LOCID",
        "    WHERE q.ITEMID IS NULL GROUP BY i.ItemID, i.LOCID;",
        "    UPDATE t SET CLBal = t.CLBal + s.Qty FROM dbo.Web_TrnQOH t",
        "    JOIN (SELECT ItemID, LOCID, SUM(CONVERT(INT, StkMultiplier) * Qty) AS Qty FROM inserted GROUP BY ItemID, LOCID) s",
        "      ON s.ItemID = t.ITEMID AND s.LOCID = t.LOCID;",
        "END');",
        "GO",
        "IF OBJECT_ID('dbo.tr_Web_DeleteQOH', 'TR') IS NULL",
        "EXEC('CREATE TRIGGER dbo.tr_Web_DeleteQOH ON dbo.Web_UtlStockList FOR DELETE AS",
        "BEGIN",
        "    SET NOCOUNT ON;",
        "    INSERT INTO dbo.Web_TrnQOH (ITEMID, LOCID, OPQty, TrnQty, CLBal)",
        "    SELECT d.ItemID, d.LOCID, 0, 0, 0 FROM deleted d",
        "    LEFT JOIN dbo.Web_TrnQOH q ON d.ItemID = q.ITEMID AND d.LOCID = q.LOCID",
        "    WHERE q.ITEMID IS NULL GROUP BY d.ItemID, d.LOCID;",
        "    UPDATE t SET CLBal = t.CLBal - s.Qty FROM dbo.Web_TrnQOH t",
        "    JOIN (SELECT ItemID, LOCID, SUM(CONVERT(INT, StkMultiplier) * Qty) AS Qty FROM deleted GROUP BY ItemID, LOCID) s",
        "      ON s.ItemID = t.ITEMID AND s.LOCID = t.LOCID;",
        "END');",
        "GO",
        "",
        "-- Web-only data with no desktop table: roles and their screen rights, field/report rights, approvals,",
        "-- document settings, number series, menu layout, merged items, security settings. One JSON value per key.",
        "IF OBJECT_ID('dbo.Web_AppSetting', 'U') IS NULL",
        "CREATE TABLE dbo.Web_AppSetting (",
        "    Section   VARCHAR(50)   NOT NULL,",
        "    [Key]     NVARCHAR(200) NOT NULL,",
        "    Value     NVARCHAR(MAX) NULL,",
        "    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppSetting_ChangedAt DEFAULT SYSUTCDATETIME(),",
        "    ChangedBy VARCHAR(50)   NULL,",
        "    CONSTRAINT PK_Web_AppSetting PRIMARY KEY (Section, [Key])",
        ");",
        "GO",
        "-- The web app's full master rows (store.touch), laid over the legacy-format rows at startup: fields the",
        "-- desktop tables have no column for (password hashes, web-only settings of a row) are kept here.",
        "-- Value NULL = the row was deleted in the web app.",
        "IF OBJECT_ID('dbo.Web_AppRow', 'U') IS NULL",
        "CREATE TABLE dbo.Web_AppRow (",
        "    Kind      VARCHAR(40)   NOT NULL,",
        "    RowKey    NVARCHAR(200) NOT NULL,",
        "    Value     NVARCHAR(MAX) NULL,",
        "    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppRow_ChangedAt DEFAULT SYSUTCDATETIME(),",
        "    ChangedBy VARCHAR(50)   NULL,",
        "    CONSTRAINT PK_Web_AppRow PRIMARY KEY (Kind, RowKey)",
        ");",
        "GO",
        "-- Every document made in the web app, whole (sales invoices and purchase bills are also written in",
        "-- legacy format to Web_TrnHdrSINV/Web_TrnDtlSINV and Web_TrnHdrPB/Web_TrnDtlPB).",
        "IF OBJECT_ID('dbo.Web_AppDoc', 'U') IS NULL",
        "CREATE TABLE dbo.Web_AppDoc (",
        "    Kind      VARCHAR(40)   NOT NULL,",
        "    DocNo     NVARCHAR(60)  NOT NULL,",
        "    DocDate   DATE          NULL,",
        "    Status    VARCHAR(20)   NULL,",
        "    Value     NVARCHAR(MAX) NOT NULL,",
        "    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppDoc_ChangedAt DEFAULT SYSUTCDATETIME(),",
        "    ChangedBy VARCHAR(50)   NULL,",
        "    CONSTRAINT PK_Web_AppDoc PRIMARY KEY (Kind, DocNo)",
        ");",
        "GO",
        "-- The web app's stock ledger (store.MOVEMENTS it made), in order. Each move is also a Web_UtlStockList row",
        "-- (whose trigger keeps Web_TrnQOH); this keeps the web's own kinds (sale-cancel, sales-return, ...).",
        "IF OBJECT_ID('dbo.Web_AppMove', 'U') IS NULL",
        "CREATE TABLE dbo.Web_AppMove (",
        "    Seq       BIGINT        NOT NULL CONSTRAINT PK_Web_AppMove PRIMARY KEY,",
        "    ItemCode  NVARCHAR(60)  NOT NULL,",
        "    Qty       DECIMAL(18,3) NOT NULL,",
        "    Kind      VARCHAR(30)   NOT NULL,",
        "    DocNo     NVARCHAR(60)  NULL,",
        "    MoveDate  DATE          NOT NULL,",
        "    Rate      DECIMAL(18,4) NULL,",
        "    Extra     NVARCHAR(MAX) NULL",
        ");",
        "GO",
        "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Web_AppMove') AND name = 'IX_Web_AppMove_Item')",
        "CREATE INDEX IX_Web_AppMove_Item ON dbo.Web_AppMove (ItemCode);",
        "GO",
        "-- Tools menu runs (SQL Query Execution, Re Write Stock).",
        "IF OBJECT_ID('dbo.Web_ToolAudit', 'U') IS NULL",
        "CREATE TABLE dbo.Web_ToolAudit (",
        "    Id       BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Web_ToolAudit PRIMARY KEY,",
        "    At       DATETIME2     NOT NULL CONSTRAINT DF_Web_ToolAudit_At DEFAULT SYSUTCDATETIME(),",
        "    UserCode VARCHAR(50)   NOT NULL,",
        "    Tool     VARCHAR(50)   NOT NULL,",
        "    Detail   NVARCHAR(MAX) NULL,",
        "    Ok       BIT           NOT NULL,",
        "    RowCnt   INT           NULL,",
        "    Error    NVARCHAR(1000) NULL",
        ");",
        "GO",
        "-- The one-time copy of the legacy data (app/setup_db.py): one row per table per run.",
        "IF OBJECT_ID('dbo.Web_Setup', 'U') IS NULL",
        "CREATE TABLE dbo.Web_Setup (",
        "    RunId      INT           NOT NULL,",
        "    TableName  VARCHAR(128)  NOT NULL,",
        "    SourceRows BIGINT        NULL,",
        "    CopiedRows BIGINT        NULL,",
        "    StartedAt  DATETIME2     NULL,",
        "    DoneAt     DATETIME2     NULL,",
        "    Status     VARCHAR(20)   NOT NULL,",
        "    Error      NVARCHAR(1000) NULL,",
        "    CONSTRAINT PK_Web_Setup PRIMARY KEY (RunId, TableName)",
        ");",
        "GO",
        f"IF NOT EXISTS (SELECT 1 FROM dbo.Web_SchemaVersion WHERE Version = {VERSION})",
        f"INSERT INTO dbo.Web_SchemaVersion (Version) VALUES ({VERSION});",
        "GO",
        "",
    ]
    OUT.write_text("\n".join(out))
    rollback = ["-- 002_web_legacy_tables.rollback.sql: drops every Web_ table made by 002 AND ITS DATA (web bills included).",
                "-- GENERATED by tools/gen_web_schema.py. Only for a test copy or before go-live.", ""]
    for t in tables + ["AppSetting", "AppRow", "AppDoc", "AppMove", "ToolAudit", "Setup"]:
        rollback += [f"IF OBJECT_ID('dbo.{web(t)}', 'U') IS NOT NULL DROP TABLE dbo.[{web(t)}];", "GO"]
    rollback += [f"IF OBJECT_ID('dbo.Web_SchemaVersion', 'U') IS NOT NULL DELETE FROM dbo.Web_SchemaVersion WHERE Version = {VERSION};", "GO", ""]
    OUT.with_name("002_web_legacy_tables.rollback.sql").write_text("\n".join(rollback))
    print(f"Wrote {OUT.name}: {len(tables)} tables, {len(funcs)} function(s), {sum(len(v) for v in indexes.values())} indexes")


if __name__ == "__main__":
    main()

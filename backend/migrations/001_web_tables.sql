-- 001_web_tables.sql: the tables the web app writes to (app/writeback.py).
--
-- Approach: separate Web_* tables. No legacy table (TrnHdrSINV, TrnDtlSINV, TrnQOH, MstItem, ...) is
-- created, altered or written. The desktop app does not read these tables, so web bills do not show in
-- the desktop app; they are kept here for reconciliation (Web_StockBalance vs TrnQOH) and reporting.
--
-- Safe to run twice: every CREATE is guarded by a sys.tables / sys.indexes check.
-- Undo with 001_web_tables.rollback.sql. Apply with:  .venv/bin/python tools/migrate.py up
-- Needs SQL Server 2008 or later (no DROP IF EXISTS / ISJSON).

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_SchemaVersion' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_SchemaVersion (
    Version    INT       NOT NULL CONSTRAINT PK_Web_SchemaVersion PRIMARY KEY,
    AppliedAt  DATETIME2 NOT NULL CONSTRAINT DF_Web_SchemaVersion_AppliedAt DEFAULT SYSUTCDATETIME()
);
GO

-- One row per web document (sales invoice, purchase bill, return, expense, ...). DocJson holds the
-- whole document as the web app saved it; the other columns are for filtering and totals.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_DocHeader' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_DocHeader (
    DocKind        VARCHAR(30)   NOT NULL,
    DocNo          NVARCHAR(50)  NOT NULL,
    DocDate        DATE          NOT NULL,
    Status         VARCHAR(12)   NULL,
    PartyCode      NVARCHAR(50)  NULL,
    CounterCode    NVARCHAR(50)  NULL,
    NetAmount      DECIMAL(18,2) NULL,
    LegacyNoClash  BIT           NOT NULL CONSTRAINT DF_Web_DocHeader_Clash DEFAULT 0,
    DocJson        NVARCHAR(MAX) NOT NULL,
    CreatedBy      NVARCHAR(50)  NULL,
    CreatedAt      DATETIME2     NOT NULL CONSTRAINT DF_Web_DocHeader_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt      DATETIME2     NOT NULL CONSTRAINT DF_Web_DocHeader_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_Web_DocHeader PRIMARY KEY (DocKind, DocNo)
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Web_DocHeader_Date' AND object_id = OBJECT_ID('dbo.Web_DocHeader'))
CREATE INDEX IX_Web_DocHeader_Date ON dbo.Web_DocHeader (DocDate, DocKind);
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_DocLine' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_DocLine (
    DocKind   VARCHAR(30)   NOT NULL,
    DocNo     NVARCHAR(50)  NOT NULL,
    LineNum   INT           NOT NULL,
    ItemCode  NVARCHAR(50)  NOT NULL,
    Qty       DECIMAL(18,3) NOT NULL,
    Rate      DECIMAL(18,2) NULL,
    Amount    DECIMAL(18,2) NULL,
    CONSTRAINT PK_Web_DocLine PRIMARY KEY (DocKind, DocNo, LineNum),
    CONSTRAINT FK_Web_DocLine_Header FOREIGN KEY (DocKind, DocNo)
        REFERENCES dbo.Web_DocHeader (DocKind, DocNo) ON DELETE CASCADE
);
GO

-- Every stock change the web app made (+ in, - out), with the event that wrote it.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_StockMove' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_StockMove (
    MoveId     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Web_StockMove PRIMARY KEY,
    EventId    CHAR(32)      NOT NULL,
    ItemCode   NVARCHAR(50)  NOT NULL,
    MoveDate   DATE          NOT NULL,
    Qty        DECIMAL(18,3) NOT NULL,
    MoveKind   VARCHAR(20)   NOT NULL,
    DocNo      NVARCHAR(50)  NOT NULL,
    Rate       DECIMAL(18,2) NULL,
    CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_Web_StockMove_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Web_StockMove_Item' AND object_id = OBJECT_ID('dbo.Web_StockMove'))
CREATE INDEX IX_Web_StockMove_Item ON dbo.Web_StockMove (ItemCode, MoveDate);
GO

-- Net stock change per item made through the web app. Updated as QtyChange = QtyChange + ?, never
-- read-then-write, so two saves at once can't lose a change. Compare with TrnQOH.CLBal to reconcile.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_StockBalance' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_StockBalance (
    ItemCode   NVARCHAR(50)  NOT NULL CONSTRAINT PK_Web_StockBalance PRIMARY KEY,
    QtyChange  DECIMAL(18,3) NOT NULL CONSTRAINT DF_Web_StockBalance_Qty DEFAULT 0,
    UpdatedAt  DATETIME2     NOT NULL CONSTRAINT DF_Web_StockBalance_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

-- One row per applied write (who, when, which document). EventId is the idempotency key: an event
-- that is already here is skipped, so a retry after a network drop never writes a bill twice.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Web_WriteLog' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.Web_WriteLog (
    EventId      CHAR(32)     NOT NULL CONSTRAINT PK_Web_WriteLog PRIMARY KEY,
    DocKind      VARCHAR(30)  NOT NULL,
    DocNo        NVARCHAR(50) NOT NULL,
    Action       VARCHAR(20)  NOT NULL,
    UserCode     NVARCHAR(50) NULL,
    RequestedAt  DATETIME2    NOT NULL,
    AppliedAt    DATETIME2    NOT NULL CONSTRAINT DF_Web_WriteLog_AppliedAt DEFAULT SYSUTCDATETIME()
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Web_SchemaVersion WHERE Version = 1)
INSERT INTO dbo.Web_SchemaVersion (Version) VALUES (1);
GO

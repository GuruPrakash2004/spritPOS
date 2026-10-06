-- web_app_tables.sql: the web app's own tables, created in the shop's vLQMS database by the Database setup.
-- These five hold what the desktop tables have no column for. No desktop table is created, altered or emptied;
-- the app reads and writes the desktop's own tables (MstITEM, TrnHdrSINV, ...) directly. Safe to run twice:
-- every CREATE is guarded. Holds no data, server names or credentials.

-- Web-only data with no desktop table: roles and their screen rights, field/report rights, approvals,
-- document settings, number series, menu layout, merged items, security settings. One JSON value per key.
IF OBJECT_ID('dbo.Web_AppSetting', 'U') IS NULL
CREATE TABLE dbo.Web_AppSetting (
    Section   VARCHAR(50)   NOT NULL,
    [Key]     NVARCHAR(200) NOT NULL,
    Value     NVARCHAR(MAX) NULL,
    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppSetting_ChangedAt DEFAULT SYSUTCDATETIME(),
    ChangedBy VARCHAR(50)   NULL,
    CONSTRAINT PK_Web_AppSetting PRIMARY KEY (Section, [Key])
);
GO
-- The web app's full master rows (store.touch), laid over the desktop rows at startup: fields the desktop
-- tables have no column for (password hashes, web-only settings of a row) are kept here.
-- Value NULL = the row was deleted in the web app.
IF OBJECT_ID('dbo.Web_AppRow', 'U') IS NULL
CREATE TABLE dbo.Web_AppRow (
    Kind      VARCHAR(40)   NOT NULL,
    RowKey    NVARCHAR(200) NOT NULL,
    Value     NVARCHAR(MAX) NULL,
    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppRow_ChangedAt DEFAULT SYSUTCDATETIME(),
    ChangedBy VARCHAR(50)   NULL,
    CONSTRAINT PK_Web_AppRow PRIMARY KEY (Kind, RowKey)
);
GO
-- Every document made in the web app, whole (sales invoices and purchase bills are also written as the
-- desktop's own rows in TrnHdrSINV/TrnDtlSINV and TrnHdrPB/TrnDtlPB).
IF OBJECT_ID('dbo.Web_AppDoc', 'U') IS NULL
CREATE TABLE dbo.Web_AppDoc (
    Kind      VARCHAR(40)   NOT NULL,
    DocNo     NVARCHAR(60)  NOT NULL,
    DocDate   DATE          NULL,
    Status    VARCHAR(20)   NULL,
    Value     NVARCHAR(MAX) NOT NULL,
    ChangedAt DATETIME2     NOT NULL CONSTRAINT DF_Web_AppDoc_ChangedAt DEFAULT SYSUTCDATETIME(),
    ChangedBy VARCHAR(50)   NULL,
    CONSTRAINT PK_Web_AppDoc PRIMARY KEY (Kind, DocNo)
);
GO
-- The web app's stock ledger (store.MOVEMENTS it made), in order. Each move is also a UtlStockList row
-- (whose desktop trigger keeps TrnQOH); this keeps the web's own kinds (sale-cancel, sales-return, ...).
IF OBJECT_ID('dbo.Web_AppMove', 'U') IS NULL
CREATE TABLE dbo.Web_AppMove (
    Seq       BIGINT        NOT NULL CONSTRAINT PK_Web_AppMove PRIMARY KEY,
    ItemCode  NVARCHAR(60)  NOT NULL,
    Qty       DECIMAL(18,3) NOT NULL,
    Kind      VARCHAR(30)   NOT NULL,
    DocNo     NVARCHAR(60)  NULL,
    MoveDate  DATE          NOT NULL,
    Rate      DECIMAL(18,4) NULL,
    Extra     NVARCHAR(MAX) NULL
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Web_AppMove') AND name = 'IX_Web_AppMove_Item')
CREATE INDEX IX_Web_AppMove_Item ON dbo.Web_AppMove (ItemCode);
GO
-- Tools menu runs (SQL Query Execution, Re Write Stock).
IF OBJECT_ID('dbo.Web_ToolAudit', 'U') IS NULL
CREATE TABLE dbo.Web_ToolAudit (
    Id       BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Web_ToolAudit PRIMARY KEY,
    At       DATETIME2     NOT NULL CONSTRAINT DF_Web_ToolAudit_At DEFAULT SYSUTCDATETIME(),
    UserCode VARCHAR(50)   NOT NULL,
    Tool     VARCHAR(50)   NOT NULL,
    Detail   NVARCHAR(MAX) NULL,
    Ok       BIT           NOT NULL,
    RowCnt   INT           NULL,
    Error    NVARCHAR(1000) NULL
);

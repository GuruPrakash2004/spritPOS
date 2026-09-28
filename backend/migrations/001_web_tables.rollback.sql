-- 001_web_tables.rollback.sql: removes everything 001_web_tables.sql created, and nothing else.
-- DELETES ALL WEB WRITE-BACK DATA in these tables. Take a backup first.
-- Safe to run twice. Run with:  .venv/bin/python tools/migrate.py down 1

IF OBJECT_ID('dbo.Web_WriteLog', 'U') IS NOT NULL DROP TABLE dbo.Web_WriteLog;
GO
IF OBJECT_ID('dbo.Web_StockBalance', 'U') IS NOT NULL DROP TABLE dbo.Web_StockBalance;
GO
IF OBJECT_ID('dbo.Web_StockMove', 'U') IS NOT NULL DROP TABLE dbo.Web_StockMove;
GO
IF OBJECT_ID('dbo.Web_DocLine', 'U') IS NOT NULL DROP TABLE dbo.Web_DocLine;
GO
IF OBJECT_ID('dbo.Web_DocHeader', 'U') IS NOT NULL DROP TABLE dbo.Web_DocHeader;
GO
IF OBJECT_ID('dbo.Web_SchemaVersion', 'U') IS NOT NULL DELETE FROM dbo.Web_SchemaVersion WHERE Version = 1;
GO
IF OBJECT_ID('dbo.Web_SchemaVersion', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.Web_SchemaVersion)
    DROP TABLE dbo.Web_SchemaVersion;
GO

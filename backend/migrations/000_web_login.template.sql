-- 000_web_login.template.sql: a separate SQL login for the web app (not sa, not the desktop app's login).
-- A DBA runs this by hand in SSMS, replacing every <placeholder>. Do NOT save the real password in this
-- file or anywhere in git: it goes only in backend/.env (DB_USER / DB_PASSWORD).
-- tools/migrate.py does not run this file.

-- 1. Login and database user
CREATE LOGIN [<web_login>] WITH PASSWORD = '<strong password>', CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
GO
USE [<database>];
GO
CREATE USER [<web_login>] FOR LOGIN [<web_login>];
GO

-- 2. Read the legacy tables (masters, bills, TrnQOH). No INSERT/UPDATE/DELETE on them.
GRANT SELECT ON SCHEMA::dbo TO [<web_login>];
GO

-- 3. Setup only, while running tools/migrate.py up. Revoke straight after (step 5).
GRANT CREATE TABLE TO [<web_login>];
GRANT ALTER ON SCHEMA::dbo TO [<web_login>];
GO

-- 4. After 001_web_tables.sql is applied: write only to the Web_ tables.
GRANT INSERT, UPDATE ON dbo.Web_DocHeader    TO [<web_login>];
GRANT INSERT, DELETE ON dbo.Web_DocLine      TO [<web_login>];
GRANT INSERT         ON dbo.Web_StockMove    TO [<web_login>];
GRANT INSERT, UPDATE ON dbo.Web_StockBalance TO [<web_login>];
GRANT INSERT         ON dbo.Web_WriteLog     TO [<web_login>];
GO

-- 5. Once the migration has run:
REVOKE CREATE TABLE FROM [<web_login>];
REVOKE ALTER ON SCHEMA::dbo FROM [<web_login>];
GO

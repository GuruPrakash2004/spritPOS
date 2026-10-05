-- 000_web_login.template.sql: a separate SQL login for the web app (not sa, not the desktop app's login).
-- A DBA runs this by hand in SSMS, replacing every <placeholder>. Do NOT save the real password in this
-- file or anywhere in git: the sign-in page's Database setup stores it, sealed, in backend/.env.
-- Optional: at a shop the setup is usually run with the local sa login. Nothing runs this file for you.

-- 1. Login and database user
CREATE LOGIN [<web_login>] WITH PASSWORD = '<strong password>', CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
GO
USE [<database>];
GO
CREATE USER [<web_login>] FOR LOGIN [<web_login>];
GO

-- 2. Read everything (the one-time copy reads the old tables). No INSERT/UPDATE/DELETE on the old tables.
GRANT SELECT ON SCHEMA::dbo TO [<web_login>];
GO

-- 3. The one-time Database setup creates the Web_ tables, a function and two triggers. Either run it with a
--    db_owner login (e.g. sa) and give the web login the rights in step 4 afterwards, or grant these for
--    the setup and revoke them in step 5.
GRANT CREATE TABLE, CREATE FUNCTION TO [<web_login>];
GRANT ALTER ON SCHEMA::dbo TO [<web_login>];
GO

-- 4. After the setup: the web app writes only to the Web_ tables. (ALTER on Web_UtlStockList: Merge Items
--    switches its stock trigger off while it re-points the rows.)
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'GRANT INSERT, UPDATE, DELETE ON dbo.' + QUOTENAME(name) + N' TO [<web_login>];'
FROM sys.tables WHERE name LIKE 'Web[_]%';
EXEC sp_executesql @sql;
GRANT ALTER ON dbo.Web_UtlStockList TO [<web_login>];
GO

-- 5. Once the setup has run:
REVOKE CREATE TABLE, CREATE FUNCTION FROM [<web_login>];
REVOKE ALTER ON SCHEMA::dbo FROM [<web_login>];
GO

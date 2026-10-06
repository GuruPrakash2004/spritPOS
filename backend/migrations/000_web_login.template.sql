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

-- 2. The web app works on the desktop's own tables: it reads them and writes its bills, masters, stock rows
--    and audit rows. (The desktop app keeps using the same tables.)
ALTER ROLE db_datareader ADD MEMBER [<web_login>];
ALTER ROLE db_datawriter ADD MEMBER [<web_login>];
GO

-- 3. The one-time Database setup creates the web app's five own tables (migrations/web_app_tables.sql).
--    Either run it with a db_owner login (e.g. sa) and skip this, or grant these for the setup and revoke
--    them in step 5.
GRANT CREATE TABLE TO [<web_login>];
GRANT ALTER ON SCHEMA::dbo TO [<web_login>];
GO

-- 4. Merge Items switches the stock trigger on UtlStockList off while it re-points the rows.
GRANT ALTER ON dbo.UtlStockList TO [<web_login>];
GO

-- 5. Once the setup has run:
REVOKE CREATE TABLE FROM [<web_login>];
REVOKE ALTER ON SCHEMA::dbo FROM [<web_login>];
GO

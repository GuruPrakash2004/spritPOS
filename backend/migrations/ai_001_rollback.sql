-- Rollback of ai_001.sql. NOT run by the app; run it yourself. Drops only objects in schema `ai`.
-- The schema is dropped only if nothing else (e.g. ai_002's table) is still in it.

IF OBJECT_ID('ai.ItemDailySales', 'U') IS NOT NULL
    DROP TABLE ai.ItemDailySales;
GO

IF EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ai')
   AND NOT EXISTS (SELECT 1 FROM sys.objects WHERE schema_id = SCHEMA_ID('ai'))
    EXEC('DROP SCHEMA ai');
GO

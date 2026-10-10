-- Rollback of ai_002.sql. NOT run by the app; run it yourself. Drops only ai.IndentSuggestion.
-- Indents already created from approved suggestions are normal Purchase Indents and stay.
-- The schema is dropped only if nothing else (e.g. ai_001's table) is still in it.

IF OBJECT_ID('ai.IndentSuggestion', 'U') IS NOT NULL
    DROP TABLE ai.IndentSuggestion;
GO

IF EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ai')
   AND NOT EXISTS (SELECT 1 FROM sys.objects WHERE schema_id = SCHEMA_ID('ai'))
    EXEC('DROP SCHEMA ai');
GO

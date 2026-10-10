-- ai_002: draft purchase-indent suggestions made by the AI forecast (Phase 6).
-- NOT run by the app. Run it yourself in SSMS after a backup. Safe to run twice (every CREATE is guarded).
-- Touches no desktop table. Roll back with ai_002_rollback.sql.
--
-- A suggestion is only a draft. Approving one in the web app creates a normal Purchase Indent through the
-- existing indent code (the new indent's number is kept in IndentDocNo); nothing here creates an indent by itself.

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ai')
    EXEC('CREATE SCHEMA ai');
GO

IF OBJECT_ID('ai.IndentSuggestion', 'U') IS NULL
BEGIN
    CREATE TABLE ai.IndentSuggestion (
        SuggestionID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_IndentSuggestion PRIMARY KEY,
        ItemID        INT            NOT NULL,                 -- MstITEM.ITEMID
        ItemCode      NVARCHAR(50)   NOT NULL,
        ItemName      NVARCHAR(200)  NULL,
        Cases         INT            NOT NULL,                 -- suggested quantity, whole cases
        Bottles       INT            NOT NULL,                 -- the same in bottles (Cases x bottles per case)
        QOH           DECIMAL(18,3)  NULL,                     -- stock when the suggestion was made
        DaysRemaining DECIMAL(9,1)   NULL,
        ReorderPoint  DECIMAL(18,3)  NULL,
        TargetStock   DECIMAL(18,3)  NULL,
        RecStatus     NVARCHAR(30)   NULL,                     -- Reorder now / Reorder soon / ...
        ItemClass     NVARCHAR(20)   NULL,                     -- fast / slow / accessory / insufficient
        Method        NVARCHAR(60)   NULL,                     -- the forecast method used
        Reason        NVARCHAR(500)  NULL,
        Status        NVARCHAR(10)   NOT NULL CONSTRAINT DF_IndentSuggestion_Status DEFAULT 'draft',
        CreatedOn     DATETIME       NOT NULL CONSTRAINT DF_IndentSuggestion_On DEFAULT GETDATE(),
        CreatedBy     NVARCHAR(50)   NULL,
        DecidedOn     DATETIME       NULL,
        DecidedBy     NVARCHAR(50)   NULL,
        IndentDocNo   NVARCHAR(50)   NULL,                     -- set when approved
        CONSTRAINT CK_IndentSuggestion_Status CHECK (Status IN ('draft', 'approved', 'rejected'))
    );
    CREATE NONCLUSTERED INDEX IX_IndentSuggestion_Status ON ai.IndentSuggestion (Status, CreatedOn);
    -- At most one open draft per item: generating again replaces it.
    CREATE UNIQUE NONCLUSTERED INDEX UX_IndentSuggestion_Draft ON ai.IndentSuggestion (ItemID) WHERE Status = 'draft';
END
GO

-- ai_001: schema `ai` and the daily sales history table (AI stock forecasting, Phase 2).
-- NOT run by the app. Run it yourself in SSMS after a backup. Safe to run twice (every CREATE is guarded).
-- Touches no desktop table. Roll back with ai_001_rollback.sql.
--
-- ai.ItemDailySales holds one row per item per day that had sales or returns (days with neither are
-- not stored; app/ai/sales_history.py fills them with zeros when it reads the series).
--   SoldQty     bottles on non-cancelled, non-deleted sales invoices (TrnDtlSINV.SINVQty)
--   ReturnQty   bottles on non-cancelled sales returns (TrnDtlSR.SRQty); 0 where the shop has no TrnDtlSR
--   NetQty      SoldQty - ReturnQty (can be negative on a day with only returns)

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ai')
    EXEC('CREATE SCHEMA ai');
GO

IF OBJECT_ID('ai.ItemDailySales', 'U') IS NULL
BEGIN
    CREATE TABLE ai.ItemDailySales (
        ItemID      INT           NOT NULL,   -- MstITEM.ITEMID
        SaleDate    DATE          NOT NULL,
        SoldQty     DECIMAL(18,3) NOT NULL CONSTRAINT DF_ItemDailySales_Sold DEFAULT 0,
        ReturnQty   DECIMAL(18,3) NOT NULL CONSTRAINT DF_ItemDailySales_Ret  DEFAULT 0,
        NetQty      DECIMAL(18,3) NOT NULL CONSTRAINT DF_ItemDailySales_Net  DEFAULT 0,
        RefreshedOn DATETIME      NOT NULL CONSTRAINT DF_ItemDailySales_On   DEFAULT GETDATE(),
        CONSTRAINT PK_ItemDailySales PRIMARY KEY CLUSTERED (SaleDate, ItemID)
    );
    -- Per-item reads (one item's history) use this; the clustered key serves the date-range refresh.
    CREATE NONCLUSTERED INDEX IX_ItemDailySales_Item ON ai.ItemDailySales (ItemID, SaleDate) INCLUDE (NetQty);
END
GO

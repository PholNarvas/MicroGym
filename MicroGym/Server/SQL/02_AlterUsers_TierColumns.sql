-- =============================================
-- Script      : 02_AlterUsers_TierColumns.sql
-- Description : Replaces AnnualMemberExpiry with
--               TierID + TierExpiryDate on Users.
--               Migrates existing data. Safe to re-run.
-- =============================================

-- 1. Add TierID column (no FK — raw value)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'TierID'
)
BEGIN
    ALTER TABLE [dbo].[Users] ADD [TierID] INT NULL;
END
GO

-- 2. Add TierExpiryDate column
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'TierExpiryDate'
)
BEGIN
    ALTER TABLE [dbo].[Users] ADD [TierExpiryDate] DATE NULL;
END
GO

-- 3. Migrate existing AnnualMemberExpiry → TierExpiryDate
--    Set TierID = 2 (Annual) for anyone who had an active annual membership
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'AnnualMemberExpiry'
)
BEGIN
    UPDATE [dbo].[Users]
    SET [TierID]         = 2,
        [TierExpiryDate] = [AnnualMemberExpiry]
    WHERE [AnnualMemberExpiry] IS NOT NULL
      AND [TierID] IS NULL;

    -- Drop old column
    ALTER TABLE [dbo].[Users] DROP COLUMN [AnnualMemberExpiry];
END
GO

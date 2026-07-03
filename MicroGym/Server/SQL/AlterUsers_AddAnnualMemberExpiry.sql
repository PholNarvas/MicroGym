-- =============================================
-- Script: AlterUsers_AddAnnualMemberExpiry.sql
-- Description: Adds AnnualMemberExpiry column to Users.
--              Safe to re-run.
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users')
      AND name = 'AnnualMemberExpiry'
)
BEGIN
    ALTER TABLE [dbo].[Users]
    ADD [AnnualMemberExpiry] DATE NULL;
END
GO

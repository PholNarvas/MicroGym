-- =============================================
-- Script      : 03_AlterPayments_AddPaymentType.sql
-- Description : Makes MemberShipTypeID nullable (Annual payments
--               have no plan). Adds PaymentType column to
--               distinguish plan payments from tier payments.
--               Safe to re-run.
-- =============================================

-- 1. Make MemberShipTypeID nullable
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Payments')
      AND name = 'MemberShipTypeID'
      AND is_nullable = 0
)
BEGIN
    ALTER TABLE [dbo].[Payments]
    ALTER COLUMN [MemberShipTypeID] INT NULL;
END
GO

-- 2. Add PaymentType column
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Payments') AND name = 'PaymentType'
)
BEGIN
    ALTER TABLE [dbo].[Payments]
    ADD [PaymentType] NVARCHAR(20) NOT NULL DEFAULT 'Plan';
END
GO

-- 3. Mark existing NULL MemberShipTypeID rows as 'Tier'
UPDATE [dbo].[Payments]
SET [PaymentType] = 'Tier'
WHERE [MemberShipTypeID] IS NULL
  AND [PaymentType] = 'Plan';
GO

-- =============================================
-- Script      : AlterMembershipTypes_AddSubscriptionType.sql
-- Description : Adds SubscriptionType column to MembershipTypes.
--               Run once in SSMS. Safe to re-run (IF NOT EXISTS check).
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.MembershipTypes')
      AND name = 'SubscriptionType'
)
BEGIN
    ALTER TABLE [dbo].[MembershipTypes]
    ADD [SubscriptionType] NVARCHAR(50) NULL;
END
GO

-- =============================================
-- Update existing plans with subscription types.
-- Adjust these values to match your actual data.
-- =============================================

-- Example: if your plans are named Boxing, Gym, Zumba
-- and you want to add Monthly as the default subscription type.
-- Customize per your actual MembershipTypes rows.

UPDATE [dbo].[MembershipTypes]
SET [SubscriptionType] = CASE
    WHEN [DurationInMonths] = 1  THEN 'Monthly'
    WHEN [DurationInMonths] = 3  THEN '3 Months'
    WHEN [DurationInMonths] = 6  THEN '6 Months'
    WHEN [DurationInMonths] = 12 THEN 'Annual'
    ELSE 'Monthly'
END
WHERE [SubscriptionType] IS NULL
  AND [IsWalkIn] = 0;

UPDATE [dbo].[MembershipTypes]
SET [SubscriptionType] = 'Walk-in'
WHERE [IsWalkIn] = 1
  AND [SubscriptionType] IS NULL;
GO

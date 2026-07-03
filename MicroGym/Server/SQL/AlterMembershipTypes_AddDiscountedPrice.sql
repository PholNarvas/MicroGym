-- =============================================
-- Script: AlterMembershipTypes_AddDiscountedPrice.sql
-- Description: Adds DiscountedPrice column to MembershipTypes.
--              This is the price applied when the member has
--              an active Annual Membership. Safe to re-run.
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.MembershipTypes')
      AND name = 'DiscountedPrice'
)
BEGIN
    ALTER TABLE [dbo].[MembershipTypes]
    ADD [DiscountedPrice] DECIMAL(18, 2) NULL;
END
GO

-- =============================================
-- Set discounted prices for existing plans.
-- Update these values to match your actual pricing.
-- =============================================
-- UPDATE [dbo].[MembershipTypes] SET [DiscountedPrice] = 400.00 WHERE [Name] = 'Gym' AND [SubscriptionType] = 'Monthly';
-- UPDATE [dbo].[MembershipTypes] SET [DiscountedPrice] = 1100.00 WHERE [Name] = 'Gym' AND [SubscriptionType] = '3 Months';
-- (Add more as needed)

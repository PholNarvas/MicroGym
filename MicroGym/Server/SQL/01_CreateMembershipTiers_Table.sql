-- =============================================
-- Script      : 01_CreateMembershipTiers_Table.sql
-- Description : Creates MembershipTiers lookup table.
--               Run once. Safe to re-run.
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.tables WHERE name = 'MembershipTiers'
)
BEGIN
    CREATE TABLE [dbo].[MembershipTiers]
    (
        [TierID]         INT            IDENTITY(1,1) PRIMARY KEY,
        [TierName]       NVARCHAR(50)   NOT NULL,
        [Fee]            DECIMAL(18,2)  NOT NULL DEFAULT 0.00,
        [DiscountPct]    DECIMAL(5,2)   NOT NULL DEFAULT 0.00,
        [DurationMonths] INT            NOT NULL DEFAULT 12,
        [IsActive]       BIT            NOT NULL DEFAULT 1
    );
END
GO

-- Seed tiers (only insert if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[MembershipTiers])
BEGIN
    INSERT INTO [dbo].[MembershipTiers] ([TierName], [Fee], [DiscountPct], [DurationMonths], [IsActive])
    VALUES
        ('Standard', 0.00,   0.00,  12, 1),   -- default, no extra fee
        ('Annual',   500.00, 10.00, 12, 1);    -- ₱500, 10% off all plans
END
GO

-- =============================================
-- Stored Procedure : pr_GetMembershipTiers
-- Description      : Returns all active membership tiers.
--                    Owner can add/edit tiers directly in the table.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetMembershipTiers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [TierID],
        [TierName],
        [Fee],
        [DiscountPct],
        [DurationMonths],
        [IsActive]
    FROM  [dbo].[MembershipTiers]
    WHERE [IsActive] = 1
    ORDER BY [Fee] ASC;
END
GO

-- =============================================
-- Stored Procedure: pr_GetMembershipTypes
-- Description: Returns all membership plans with price info.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[pr_GetMembershipTypes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [MembershipTypeID],
        [Name],
        [Price],
        [DurationInMonths],
        [IsWalkIn]
    FROM [dbo].[MembershipTypes]
    ORDER BY [MembershipTypeID];
END;
GO

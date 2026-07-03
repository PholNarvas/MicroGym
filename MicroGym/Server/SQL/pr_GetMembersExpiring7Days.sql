-- =============================================
-- Stored Procedure : pr_GetMembersExpiring7Days
-- Description      : Returns members whose subscription OR annual membership
--                    will expire within the next 14 days from today.
--                    Already-expired members are NOT included.
--                    Excludes Admin accounts.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetMembersExpiring7Days]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Today    DATE = CAST(GETDATE() AS DATE);
    DECLARE @Deadline DATE = DATEADD(DAY, 14, @Today);  -- 2 weeks ahead

    SELECT
        [Users].[UserId],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Phone],
        [Users].[MemberShipTypeID],
        [MembershipTypes].[Name] AS [MemberShipType],
        [Users].[ExpiryDate],
        [Users].[TierID],
        [Users].[TierExpiryDate]

    FROM [dbo].[Users]

        INNER JOIN [dbo].[MembershipTypes]
            ON [MembershipTypes].[MembershipTypeID] = [Users].[MemberShipTypeID]

    WHERE [Users].[Role] <> 'Admin'
      AND (
            -- Subscription expiring within the next 14 days (not yet expired)
            (
                CAST([Users].[ExpiryDate] AS DATE) >= @Today
                AND CAST([Users].[ExpiryDate] AS DATE) <= @Deadline
            )
            OR
            -- Annual membership expiring within the next 14 days (not yet expired)
            (
                [Users].[TierID] IS NOT NULL
                AND CAST([Users].[TierExpiryDate] AS DATE) >= @Today
                AND CAST([Users].[TierExpiryDate] AS DATE) <= @Deadline
            )
          )

    ORDER BY [Users].[ExpiryDate] ASC;

END;
GO

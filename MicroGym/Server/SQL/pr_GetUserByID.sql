SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pr_GetUserByID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Users].[UserID],
        [Users].[MemberShipTypeID],
        [MembershipTypes].[Name]                              AS [MemberShipType],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Role],
        [Users].[IsActive],
        [Users].[Phone],
        [Users].[CreatedAt],
        [Users].[ExpiryDate],
        [Users].[ProfilePicture],
        [Users].[TierID],
        [Users].[TierExpiryDate],
        ISNULL([MembershipTiers].[DiscountPct], 0)            AS [TierDiscountPct],
        [LatestPayment].[PaymentMethod],
        [LatestPayment].[AmountPaid]                          AS [Price],
        [LastVisit].[CheckInTime]                             AS [LastVisit],
        (
            SELECT ISNULL(SUM([Payments].[AmountPaid]), 0)
            FROM   [dbo].[Payments]
            WHERE  [Payments].[UserID] = [Users].[UserID]
        )                                                     AS [TotalPaid],
        (
            SELECT COUNT(*)
            FROM   [dbo].[Payments]
            WHERE  [Payments].[UserID]      = [Users].[UserID]
              AND  [Payments].[PaymentType] = 'Plan'
        )                                                     AS [RenewalCount]
    FROM [dbo].[Users]
    LEFT JOIN [dbo].[MembershipTypes]
           ON [MembershipTypes].[MemberShipTypeID] = [Users].[MemberShipTypeID]
    LEFT JOIN [dbo].[MembershipTiers]
           ON [MembershipTiers].[TierID] = [Users].[TierID]
    OUTER APPLY
    (
        SELECT TOP 1
               [Payments].[PaymentMethod],
               [Payments].[AmountPaid]
        FROM   [dbo].[Payments]
        WHERE  [Payments].[UserID] = [Users].[UserID]
        ORDER BY [Payments].[PaymentDate] DESC
    ) AS [LatestPayment]
    OUTER APPLY
    (
        SELECT TOP 1 [Attendance].[CheckInTime]
        FROM   [dbo].[Attendance]
        WHERE  [Attendance].[UserID] = [Users].[UserID]
        ORDER BY [Attendance].[CheckInTime] DESC
    ) AS [LastVisit]
    WHERE [Users].[UserID] = @UserID
      AND [Users].[Role]  <> 'Admin';
END;
GO

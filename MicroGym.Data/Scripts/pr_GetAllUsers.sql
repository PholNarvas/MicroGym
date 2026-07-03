-- =============================================
-- Stored Procedure: pr_GetAllUsers
-- Description: Returns all registered users with
--              current tier info joined from MembershipTiers.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Users].[UserId],
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
        CASE
            WHEN [Users].[ExpiryDate] IS NULL                   THEN 'Active'
            WHEN [Users].[ExpiryDate] < CAST(GETDATE() AS DATE) THEN 'Expired'
            ELSE                                                      'Active'
        END                                                   AS [ExpiryStatus],
        (
            SELECT MAX([Attendance].[CheckInTime])
            FROM   [dbo].[Attendance]
            WHERE  [Attendance].[UserID] = [Users].[UserId]
        )                                                     AS [LastVisit],
        (
            SELECT MAX([Payments].[PaymentDate])
            FROM   [dbo].[Payments]
            WHERE  [Payments].[UserID] = [Users].[UserId]
        )                                                     AS [LastPaymentDate]
    FROM  [dbo].[Users]
    LEFT JOIN [dbo].[MembershipTypes] ON [MembershipTypes].[MemberShipTypeID] = [Users].[MemberShipTypeID]
    LEFT JOIN [dbo].[MembershipTiers] ON [MembershipTiers].[TierID]           = [Users].[TierID]
    ORDER BY [Users].[CreatedAt] DESC;
END
GO

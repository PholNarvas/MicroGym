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
        [MembershipTypes].[Name]  AS [MemberShipType],
        [Users].[FirstName],
        [Users].[LastName],
        [Users].[Email],
        [Users].[Role],
        [Users].[IsActive],
        [Users].[Phone],
        [Users].[CreatedAt],
        [Users].[ExpiryDate],
        [Users].[Status],
        [LatestPayment].[PaymentMethod],
        [LatestPayment].[AmountPaid]  AS [Price],
        [LastVisit].[CheckInTime]     AS [LastVisit]
    FROM [dbo].[Users]
        INNER JOIN [dbo].[MembershipTypes]
            ON [MembershipTypes].[MembershipTypeID] = [Users].[MemberShipTypeID]
        OUTER APPLY
        (
            SELECT TOP 1
                   [Payments].[PaymentMethod],
                   [Payments].[AmountPaid]
            FROM [dbo].[Payments]
            WHERE [Payments].[UserID] = [Users].[UserID]
            ORDER BY [Payments].[PaymentDate] DESC
        ) AS [LatestPayment]
        OUTER APPLY
        (
            SELECT TOP 1 [CheckInTime]
            FROM [dbo].[Attendance]
            WHERE [UserID] = [Users].[UserID]
            ORDER BY [CheckInTime] DESC
        ) AS [LastVisit]
    WHERE [Users].[UserID] = @UserID
      AND [Users].[Role] <> 'Admin';
END;
GO

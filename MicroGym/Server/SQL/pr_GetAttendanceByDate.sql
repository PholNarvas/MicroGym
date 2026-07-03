SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

-- =============================================
-- Stored Procedure : pr_GetAttendanceByDate
-- Description      : Returns all check-ins for a specific date.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetAttendanceByDate]
    @Date DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Attendance].[AttendanceID],
        [Attendance].[UserID],
        [Users].[FirstName],
        [Users].[LastName],
        [MembershipTypes].[Name],
        [Attendance].[CheckInTime],
        [Users].[ExpiryDate]
    FROM [dbo].[Attendance]
        INNER JOIN [dbo].[Users]
            ON [Users].[UserID] = [Attendance].[UserID]
        INNER JOIN [dbo].[MembershipTypes]
            ON [MembershipTypes].[MembershipTypeID] = [Users].[MemberShipTypeID]
    WHERE [Attendance].[CheckInTime] >= @Date
      AND [Attendance].[CheckInTime]  < DATEADD(DAY, 1, @Date)
    ORDER BY [Attendance].[CheckInTime] DESC;

END;
GO

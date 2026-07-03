-- =============================================
-- TABLE: Attendance
-- Run this first if the table doesn't exist yet
-- =============================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Attendance' AND xtype = 'U')
BEGIN
    CREATE TABLE [dbo].[Attendance] (
        [AttendanceID] INT IDENTITY(1,1) PRIMARY KEY,
        [UserID]       INT NOT NULL,
        [CheckInTime]  DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- sp_GetTodayAttendance
-- Returns all check-ins for today
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetTodayAttendance]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TodayStart DATETIME = CAST(GETDATE() AS DATE);
    DECLARE @TomStart   DATETIME = DATEADD(DAY, 1, @TodayStart);

    SELECT
        [Attendance].[AttendanceID],
        [Attendance].[UserID],
        [Users].[FirstName],
        [Users].[LastName],
        [MembershipTypes].[Name],
        [Attendance].[CheckInTime],
        DATEADD(MONTH, [MembershipTypes].[DurationInMonths], [Users].[CreatedAt]) AS ExpiryDate
    FROM [dbo].[Attendance]
        INNER JOIN [dbo].[Users]
            ON [Users].[UserID] = [Attendance].[UserID]
        INNER JOIN [dbo].[MembershipTypes]
            ON [MembershipTypes].[MembershipTypeID] = [Users].[MemberShipTypeID]
    WHERE [Attendance].[CheckInTime] >= @TodayStart
      AND [Attendance].[CheckInTime]  < @TomStart
    ORDER BY [Attendance].[CheckInTime] DESC;
END;
GO

-- =============================================
-- pr_CheckInMember
-- Logs a member's attendance for today
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[pr_CheckInMember]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TodayStart DATETIME = CAST(GETDATE() AS DATE);
    DECLARE @TomStart   DATETIME = DATEADD(DAY, 1, @TodayStart);

    -- Guard 1: Block if membership is expired or user not found.
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[Users]
        WHERE [UserID] = @UserID
          AND [ExpiryDate] >= @TodayStart
    )
    BEGIN
        SELECT -1; -- expired or not found
        RETURN;
    END

    -- Guard 2: Block if already checked in today.
    IF EXISTS (
        SELECT 1 FROM [dbo].[Attendance]
        WHERE [UserID] = @UserID
          AND [CheckInTime] >= @TodayStart
          AND [CheckInTime]  < @TomStart
    )
    BEGIN
        SELECT 0; -- already checked in
        RETURN;
    END

    INSERT INTO [dbo].[Attendance] ([UserID], [CheckInTime])
    VALUES (@UserID, GETDATE());

    SELECT 1; -- success
END;
GO

-- =============================================
-- sp_IsAlreadyCheckedIn
-- Returns 1 if the member already checked in today
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_IsAlreadyCheckedIn]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TodayStart DATETIME = CAST(GETDATE() AS DATE);
    DECLARE @TomStart   DATETIME = DATEADD(DAY, 1, @TodayStart);

    SELECT COUNT(1)
    FROM [dbo].[Attendance]
    WHERE [UserID] = @UserID
      AND [CheckInTime] >= @TodayStart
      AND [CheckInTime]  < @TomStart;
END;
GO

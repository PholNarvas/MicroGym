SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

-- =============================================
-- Stored Procedure : pr_GetWeeklyAttendanceSummary
-- Description      : Returns check-in counts per day for
--                    the last 7 days (including today).
--                    Days with zero check-ins are included.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetWeeklyAttendanceSummary]
AS
BEGIN
    SET NOCOUNT ON;

    WITH DateRange AS
    (
        SELECT CAST(DATEADD(DAY, -6, CAST(GETDATE() AS DATE)) AS DATE) AS AttendanceDate
        UNION ALL
        SELECT CAST(DATEADD(DAY, 1, AttendanceDate) AS DATE)
        FROM DateRange
        WHERE CAST(DATEADD(DAY, 1, AttendanceDate) AS DATE) <= CAST(GETDATE() AS DATE)
    )
    SELECT
        DateRange.AttendanceDate        AS [Date],
        COUNT([Attendance].[AttendanceID]) AS [Count]
    FROM DateRange
        LEFT JOIN [dbo].[Attendance]
            ON [Attendance].[CheckInTime] >= DateRange.AttendanceDate
           AND [Attendance].[CheckInTime]  < DATEADD(DAY, 1, DateRange.AttendanceDate)
    GROUP BY DateRange.AttendanceDate
    ORDER BY DateRange.AttendanceDate ASC;

END;
GO

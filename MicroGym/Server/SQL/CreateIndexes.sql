-- =============================================
-- Script      : CreateIndexes.sql
-- Description : Non-clustered indexes for the most
--               frequently queried columns.
--               Safe to run multiple times.
-- =============================================

-- Attendance.CheckInTime
-- Used in every attendance query (today, by date, weekly, check-in guard)
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Attendance_CheckInTime'
      AND object_id = OBJECT_ID('dbo.Attendance')
)
    CREATE NONCLUSTERED INDEX [IX_Attendance_CheckInTime]
        ON [dbo].[Attendance] ([CheckInTime] DESC);
GO

-- Attendance.UserID
-- Used in OUTER APPLY last-visit lookup and check-in guard
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Attendance_UserID'
      AND object_id = OBJECT_ID('dbo.Attendance')
)
    CREATE NONCLUSTERED INDEX [IX_Attendance_UserID]
        ON [dbo].[Attendance] ([UserID])
        INCLUDE ([CheckInTime]);
GO

-- Users.ExpiryDate
-- Used in pr_GetMembersExpiring7Days and check-in membership validation
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Users_ExpiryDate'
      AND object_id = OBJECT_ID('dbo.Users')
)
    CREATE NONCLUSTERED INDEX [IX_Users_ExpiryDate]
        ON [dbo].[Users] ([ExpiryDate])
        WHERE [IsActive] = 1;
GO

-- Users.IsActive
-- Used in member list queries filtered by active status
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Users_IsActive'
      AND object_id = OBJECT_ID('dbo.Users')
)
    CREATE NONCLUSTERED INDEX [IX_Users_IsActive]
        ON [dbo].[Users] ([IsActive]);
GO

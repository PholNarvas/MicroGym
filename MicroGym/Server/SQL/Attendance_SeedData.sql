-- =============================================
-- Attendance Seed Data
-- Make sure these UserIDs exist in your Users table
-- Adjust UserIDs as needed based on your actual data
-- =============================================

INSERT INTO [dbo].[Attendance] ([UserID], [CheckInTime]) VALUES
(1,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '07:15:00'),
(2,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '07:42:00'),
(3,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '08:05:00'),
(4,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '08:30:00'),
(5,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '09:00:00'),
(6,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '09:22:00'),
(7,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '10:10:00'),
(8,  CAST(CAST(GETDATE() AS DATE) AS DATETIME) + '10:45:00');

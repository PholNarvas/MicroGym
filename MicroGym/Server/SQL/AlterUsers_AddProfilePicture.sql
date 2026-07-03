-- =============================================
-- Migration: Add ProfilePicture to Users
-- Description: Stores member profile photo as
--              base64-encoded string. NULL = use
--              initials avatar.
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Users')
      AND name = 'ProfilePicture'
)
BEGIN
    ALTER TABLE [dbo].[Users]
        ADD [ProfilePicture] NVARCHAR(MAX) NULL;
END

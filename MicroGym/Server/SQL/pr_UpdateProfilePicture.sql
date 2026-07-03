-- =============================================
-- Stored Procedure: pr_UpdateProfilePicture
-- Description: Saves a base64-encoded profile
--              picture string for a member.
--              Pass NULL to clear the photo.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_UpdateProfilePicture]
    @UserID         INT,
    @ProfilePicture NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Users]
       SET [ProfilePicture] = @ProfilePicture
     WHERE [UserID] = @UserID;

    SELECT @@ROWCOUNT; -- 1 = success, 0 = user not found
END

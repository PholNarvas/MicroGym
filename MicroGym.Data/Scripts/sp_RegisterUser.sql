-- =============================================
-- Stored Procedure: sp_RegisterUser
-- Description: Registers a new user with auto-generated MemberID.
--              StatusCode = 'SUCCESS'      -> user created
--              StatusCode = 'EMAIL_EXISTS' -> email already taken
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[sp_RegisterUser]
    @Username     NVARCHAR(50),
    @Email        NVARCHAR(100),
    @PasswordHash NVARCHAR(MAX),
    @Role         NVARCHAR(20) = 'Member',
    @CreatedAt    DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        SELECT 0 AS NewUserId, 0 AS MemberID, 'EMAIL_EXISTS' AS StatusCode;
        RETURN;
    END

    -- Auto-generate next MemberID (MAX existing + 1, starts at 1 if table is empty)
    DECLARE @NextMemberID INT;
    SELECT @NextMemberID = ISNULL(MAX(MemberID), 0) + 1 FROM Users;

    -- Insert new user
    INSERT INTO Users (MemberID, Username, Email, PasswordHash, Role, CreatedAt)
    VALUES (@NextMemberID, @Username, @Email, @PasswordHash, @Role, @CreatedAt);

    -- Return result
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewUserId,
           @NextMemberID AS MemberID,
           'SUCCESS' AS StatusCode;
END

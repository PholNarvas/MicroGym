SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pr_DeleteMember]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete payments first to avoid FK constraint errors
    DELETE FROM [dbo].[Payments]
    WHERE [UserID] = @UserID;

    -- Delete the user
    DELETE FROM [dbo].[Users]
    WHERE [UserID] = @UserID;
END;
GO

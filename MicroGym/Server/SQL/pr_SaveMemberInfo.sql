SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

-- =============================================
-- Stored Procedure : pr_SaveMemberInfo
-- Description      : Updates a member's personal info and status only.
--                    Payment and renewal are handled by pr_RenewMember.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_SaveMemberInfo]
    @UserID           INT,
    @FirstName        NVARCHAR(50),
    @LastName         NVARCHAR(50),
    @Email            NVARCHAR(100),
    @Phone            NVARCHAR(20)  = NULL,
    @MemberShipTypeID INT,
    @IsActive         BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard: user must exist
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [UserID] = @UserID)
    BEGIN
        SELECT 0; -- user not found
        RETURN;
    END

    -- Guard: email must not be taken by another user
    IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Email] = @Email AND [UserID] <> @UserID)
    BEGIN
        SELECT -1; -- duplicate email
        RETURN;
    END

    UPDATE [dbo].[Users]
    SET [FirstName]        = @FirstName,
        [LastName]         = @LastName,
        [Email]            = @Email,
        [Phone]            = @Phone,
        [MemberShipTypeID] = @MemberShipTypeID,
        [IsActive]         = @IsActive
    WHERE [UserID] = @UserID;

    SELECT @@ROWCOUNT; -- 1 = success

END;
GO

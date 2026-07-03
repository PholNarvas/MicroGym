SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

ALTER PROCEDURE [dbo].[pr_SaveMemberInfo]
    @UserID           INT            = NULL OUTPUT,
    @FirstName        NVARCHAR(50),
    @LastName         NVARCHAR(50),
    @Email            NVARCHAR(100),
    @Phone            NVARCHAR(11)   = NULL,
    @PaymentMethod    NVARCHAR(50)   = NULL,
    @PaymentAmount    DECIMAL(18, 2) = NULL,
    @IsActive         BIT,
    @MembershipTypeID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [UserID] = @UserID)
        RETURN;

    -- Get current IsActive to detect renewal
    DECLARE @CurrentIsActive BIT;
    SELECT @CurrentIsActive = [IsActive]
    FROM [dbo].[Users]
    WHERE [UserID] = @UserID;

    -- Compute new ExpiryDate only on renewal (was 0, now 1)
    DECLARE @NewExpiryDate DATE = NULL;
    IF @CurrentIsActive = 0 AND @IsActive = 1
    BEGIN
        DECLARE @DurationInMonths INT;
        DECLARE @IsWalkIn         BIT;

        SELECT @DurationInMonths = [DurationInMonths],
               @IsWalkIn         = [IsWalkIn]
        FROM [dbo].[MembershipTypes]
        WHERE [MemberShipTypeID] = @MembershipTypeID;

        SET @NewExpiryDate = CASE
            WHEN @IsWalkIn = 1 THEN DATEADD(DAY,   1,                   GETDATE())
            ELSE                    DATEADD(MONTH, @DurationInMonths,    GETDATE())
        END;
    END

    -- Update member info
    UPDATE [dbo].[Users]
    SET [FirstName]        = @FirstName,
        [LastName]         = @LastName,
        [Email]            = @Email,
        [Phone]            = @Phone,
        [MembershipTypeID] = @MembershipTypeID,
        [IsActive]         = @IsActive,
        [Status]           = CASE
                                WHEN @CurrentIsActive = 0 AND @IsActive = 1 THEN 'Paid'
                                ELSE [Status]
                             END,
        [ExpiryDate]       = CASE
                                WHEN @NewExpiryDate IS NOT NULL THEN @NewExpiryDate
                                ELSE [ExpiryDate]
                             END
    WHERE [UserID] = @UserID;

    -- Only record payment on actual renewal
    IF @CurrentIsActive = 0 AND @IsActive = 1
    BEGIN
        EXEC [dbo].[pr_SavePayment]
            @UserID           = @UserID,
            @MemberShipTypeID = @MembershipTypeID,
            @PaymentAmount    = @PaymentAmount,
            @PaymentDate      = NULL,
            @PaymentMethod    = @PaymentMethod;
    END
END;
GO

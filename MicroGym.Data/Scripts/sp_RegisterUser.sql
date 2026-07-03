-- =============================================
-- Stored Procedure: sp_RegisterUser
-- Description: Registers a new user, computes ExpiryDate,
--              sets IsActive = 1, and records the first payment.
--              StatusCode = 'SUCCESS'      -> user created
--              StatusCode = 'EMAIL_EXISTS' -> email already taken
--              StatusCode = 'INVALID_PLAN' -> MemberShipTypeID not found
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[sp_RegisterUser]
    @MemberShipTypeID INT,
    @FirstName        NVARCHAR(50),
    @LastName         NVARCHAR(50),
    @Email            NVARCHAR(100),
    @Phone            NVARCHAR(11)   = NULL,
    @PasswordHash     NVARCHAR(MAX),
    @Role             NVARCHAR(20)   = 'Member',
    @CreatedAt        DATETIME2,
    @PaymentMethod    NVARCHAR(50)   = NULL,
    @PaymentAmount    DECIMAL(18, 2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Email] = @Email)
    BEGIN
        SELECT 0 AS NewUserId, 'EMAIL_EXISTS' AS StatusCode;
        RETURN;
    END

    -- Get plan info
    DECLARE @DurationInMonths INT;
    DECLARE @IsWalkIn         BIT;

    SELECT @DurationInMonths = [DurationInMonths],
           @IsWalkIn         = [IsWalkIn]
    FROM [dbo].[MembershipTypes]
    WHERE [MemberShipTypeID] = @MemberShipTypeID;

    IF @DurationInMonths IS NULL AND (@IsWalkIn IS NULL OR @IsWalkIn = 0)
    BEGIN
        SELECT 0 AS NewUserId, 'INVALID_PLAN' AS StatusCode;
        RETURN;
    END

    -- Compute ExpiryDate: walk-in = 1 day, else use DurationInMonths
    DECLARE @ExpiryDate DATE;
    SET @ExpiryDate = CASE
        WHEN @IsWalkIn = 1 THEN DATEADD(DAY,   1,                    @CreatedAt)
        ELSE                    DATEADD(MONTH, @DurationInMonths,     @CreatedAt)
    END;

    -- Insert new user (IsActive = 1, Status = Paid from the start)
    INSERT INTO [dbo].[Users]
    (
        [MemberShipTypeID],
        [FirstName],
        [LastName],
        [Email],
        [Phone],
        [PasswordHash],
        [Role],
        [IsActive],
        [Status],
        [CreatedAt],
        [ExpiryDate]
    )
    VALUES
    (
        @MemberShipTypeID,
        @FirstName,
        @LastName,
        @Email,
        @Phone,
        @PasswordHash,
        @Role,
        1,
        'Paid',
        @CreatedAt,
        @ExpiryDate
    );

    DECLARE @NewUserId INT = CAST(SCOPE_IDENTITY() AS INT);

    -- Record first payment
    IF @PaymentAmount IS NOT NULL AND @PaymentMethod IS NOT NULL
    BEGIN
        EXEC [dbo].[pr_SavePayment]
            @UserID           = @NewUserId,
            @MemberShipTypeID = @MemberShipTypeID,
            @PaymentAmount    = @PaymentAmount,
            @PaymentDate      = NULL,
            @PaymentMethod    = @PaymentMethod;
    END

    SELECT @NewUserId AS NewUserId, 'SUCCESS' AS StatusCode;
END

-- =============================================
-- Stored Procedure: sp_RegisterUser
-- Description: Registers a new user, computes ExpiryDate,
--              sets IsActive = 1, and records the first payment.
--              StatusCode = 'SUCCESS'      -> user created
--              StatusCode = 'EMAIL_EXISTS' -> email already taken
--              StatusCode = 'INVALID_PLAN' -> MemberShipTypeID not found
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_RegisterUser]
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
    END;

    -- Get plan info
    DECLARE @DurationInMonths INT;
    DECLARE @IsWalkIn         BIT;

    SELECT @DurationInMonths = [DurationInMonths],
           @IsWalkIn         = [IsWalkIn]
    FROM   [dbo].[MembershipTypes]
    WHERE  [MembershipTypeID] = @MemberShipTypeID;

    IF @DurationInMonths IS NULL AND (@IsWalkIn IS NULL OR @IsWalkIn = 0)
    BEGIN
        SELECT 0 AS NewUserId, 'INVALID_PLAN' AS StatusCode;
        RETURN;
    END;

    -- Compute ExpiryDate:
    --   Walk-in  → same day (pay today, use today, expired tomorrow)
    --   Regular  → DATEADD(MONTH) - 1 day so the last day is still valid
    --              e.g. 1 month from July 1  = July 31  (valid), Aug 1 = expired
    --              e.g. 6 months from July 1 = Dec 31   (valid), Jan 1 = expired
    DECLARE @ExpiryDate DATE;
    SET @ExpiryDate = CASE
        WHEN @IsWalkIn = 1
            THEN CAST(@CreatedAt AS DATE)
        ELSE
            DATEADD(DAY, -1, DATEADD(MONTH, @DurationInMonths, CAST(@CreatedAt AS DATE)))
    END;

    -- Insert new user
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

    -- Return result FIRST so Dapper's QueryFirstOrDefaultAsync reads the
    -- correct result set before pr_SavePayment potentially emits its own.
    SELECT @NewUserId AS NewUserId, 'SUCCESS' AS StatusCode;

    -- Record first payment (done AFTER the SELECT so it doesn't interfere)
    IF @PaymentAmount IS NOT NULL AND @PaymentMethod IS NOT NULL
    BEGIN
        EXEC [dbo].[pr_SavePayment]
            @UserID           = @NewUserId,
            @MemberShipTypeID = @MemberShipTypeID,
            @PaymentAmount    = @PaymentAmount,
            @PaymentDate      = NULL,
            @PaymentMethod    = @PaymentMethod;
    END;

    -- Auto check-in on registration day
    INSERT INTO [dbo].[Attendance] ([UserID], [CheckInTime])
    VALUES (@NewUserId, GETDATE());

END;
GO

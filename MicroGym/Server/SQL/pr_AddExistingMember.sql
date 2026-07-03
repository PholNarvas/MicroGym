-- =============================================
-- Stored Procedure: pr_AddExistingMember
-- Description: Inserts an existing/logbook member with
--              historical dates (not defaulting to today).
--              Returns the new UserID, or -1 if email already exists.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_AddExistingMember]
    @FirstName        NVARCHAR(50),
    @LastName         NVARCHAR(50),
    @Email            NVARCHAR(100),
    @Phone            NVARCHAR(20)   = NULL,
    @MemberShipTypeID INT,
    @PasswordHash     NVARCHAR(MAX),
    @DateJoined       DATETIME,
    @ExpiryDate       DATE,
    @TierID           INT            = NULL,
    @TierExpiryDate   DATE           = NULL,
    @PaymentAmount    DECIMAL(18, 2) = 0,
    @PaymentMethod    NVARCHAR(50)   = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Block duplicate emails
    IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Users].[Email] = @Email)
    BEGIN
        SELECT -1 AS [NewUserID];
        RETURN;
    END

    -- Insert the member with historical dates
    INSERT INTO [dbo].[Users] (
        [FirstName],
        [LastName],
        [Email],
        [Phone],
        [MemberShipTypeID],
        [PasswordHash],
        [Role],
        [IsActive],
        [CreatedAt],
        [ExpiryDate],
        [TierID],
        [TierExpiryDate]
    )
    VALUES (
        @FirstName,
        @LastName,
        @Email,
        @Phone,
        @MemberShipTypeID,
        @PasswordHash,
        'Member',
        1,
        @DateJoined,
        @ExpiryDate,
        @TierID,
        @TierExpiryDate
    );

    DECLARE @NewUserID INT = SCOPE_IDENTITY();

    -- Record the plan payment if amount was provided
    IF @PaymentAmount > 0 AND @PaymentMethod IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[Payments] (
            [UserID],
            [MemberShipTypeID],
            [AmountPaid],
            [PaymentDate],
            [PaymentMethod],
            [Status],
            [PaymentType]
        )
        VALUES (
            @NewUserID,
            @MemberShipTypeID,
            @PaymentAmount,
            @DateJoined,
            @PaymentMethod,
            'Paid',
            'Plan'
        );
    END

    -- Record the tier payment if a tier was assigned
    IF @TierID IS NOT NULL
    BEGIN
        DECLARE @TierFee DECIMAL(18, 2);
        SELECT @TierFee = [MembershipTiers].[Fee]
        FROM   [dbo].[MembershipTiers]
        WHERE  [MembershipTiers].[TierID] = @TierID;

        IF @TierFee > 0 AND @PaymentMethod IS NOT NULL
        BEGIN
            INSERT INTO [dbo].[Payments] (
                [UserID],
                [MemberShipTypeID],
                [AmountPaid],
                [PaymentDate],
                [PaymentMethod],
                [Status],
                [PaymentType]
            )
            VALUES (
                @NewUserID,
                NULL,
                @TierFee,
                @DateJoined,
                @PaymentMethod,
                'Paid',
                'Tier'
            );
        END
    END

    SELECT @NewUserID AS [NewUserID];
END
GO

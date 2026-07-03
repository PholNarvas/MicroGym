-- =============================================
-- Stored Procedure : pr_RenewMember
-- Description      : Renews a member's subscription using calendar months.
--                    If member is still active, stacks new term on top of
--                    current expiry. If expired or NULL, starts fresh from today.
--
-- Return codes:
--    1  = success
--    0  = member not found
--   -1  = membership type not found
-- SQL errors are re-thrown via THROW so the caller gets the real error.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_RenewMember]
    @UserID           INT,
    @MemberShipTypeID INT,
    @PaymentAmount    DECIMAL(18, 2),
    @PaymentMethod    NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- 1. Validate member exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [UserID] = @UserID)
        BEGIN
            SELECT 0; -- member not found
            RETURN;
        END;

        -- 2. Validate membership type
        DECLARE @DurationInMonths INT;
        DECLARE @IsWalkIn         BIT;

        SELECT @DurationInMonths = [DurationInMonths],
               @IsWalkIn         = [IsWalkIn]
        FROM   [dbo].[MembershipTypes]
        WHERE  [MembershipTypeID] = @MemberShipTypeID;

        IF @DurationInMonths IS NULL AND (@IsWalkIn IS NULL OR @IsWalkIn = 0)
        BEGIN
            SELECT -1; -- membership type not found
            RETURN;
        END;

        -- 3. Get member's current expiry for stacking logic
        DECLARE @CurrentExpiryDate DATE;
        SELECT @CurrentExpiryDate = [ExpiryDate]
        FROM   [dbo].[Users]
        WHERE  [UserID] = @UserID;

        -- 4. Compute new expiry (calendar month formula):
        --    Walk-in      → same day as today
        --    Still active → stack on top of current expiry
        --                   e.g. expires Jul 15, renew 1 month → expires Aug 15
        --    Expired/NULL → start fresh from today
        --                   e.g. today = Jul 1, 1 month → expires Jul 31
        DECLARE @NewExpiryDate DATE;
        IF @IsWalkIn = 1
            SET @NewExpiryDate = CAST(GETDATE() AS DATE);
        ELSE IF @CurrentExpiryDate IS NOT NULL AND @CurrentExpiryDate >= CAST(GETDATE() AS DATE)
            -- Stack: day after current expiry becomes day 1 of the new term
            SET @NewExpiryDate = DATEADD(DAY, -1, DATEADD(MONTH, @DurationInMonths, DATEADD(DAY, 1, @CurrentExpiryDate)));
        ELSE
            -- Fresh start from today
            SET @NewExpiryDate = DATEADD(DAY, -1, DATEADD(MONTH, @DurationInMonths, CAST(GETDATE() AS DATE)));

        -- 5. Update member
        UPDATE [dbo].[Users]
        SET    [MemberShipTypeID] = @MemberShipTypeID,
               [IsActive]         = 1,
               [Status]           = 'Paid',
               [ExpiryDate]       = @NewExpiryDate
        WHERE  [UserID] = @UserID;

        -- 6. Record the payment
        EXEC [dbo].[pr_SavePayment]
            @UserID           = @UserID,
            @MemberShipTypeID = @MemberShipTypeID,
            @PaymentAmount    = @PaymentAmount,
            @PaymentDate      = NULL,
            @PaymentMethod    = @PaymentMethod;

        SELECT 1; -- success

    END TRY
    BEGIN CATCH
        THROW; -- surface SQL errors to C# with the real message + error number
    END CATCH;

END;
GO

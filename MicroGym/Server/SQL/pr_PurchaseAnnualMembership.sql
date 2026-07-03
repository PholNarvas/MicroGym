-- =============================================
-- Stored Procedure : pr_PurchaseAnnualMembership
-- Description      : Purchases a membership tier for a user.
--                    Looks up Fee and DurationMonths from MembershipTiers.
--                    Updates Users.TierID + TierExpiryDate.
--                    Records payment with PaymentType = 'Tier'.
--                    Only inserts a payment row if Fee > 0 (Standard tier is free).
--
-- Return codes:
--    1  = success
--    0  = member not found
--   -1  = tier not found or inactive
-- SQL errors are re-thrown via THROW so the caller gets the real error.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_PurchaseAnnualMembership]
    @UserID        INT,
    @TierID        INT,
    @PaymentMethod NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- 1. Validate member exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [UserID] = @UserID)
        BEGIN
            SELECT 0; -- member not found
            RETURN;
        END

        -- 2. Validate tier exists and is active
        DECLARE @Fee            DECIMAL(18,2);
        DECLARE @DurationMonths INT;

        SELECT @Fee            = [Fee],
               @DurationMonths = [DurationMonths]
        FROM   [dbo].[MembershipTiers]
        WHERE  [TierID] = @TierID AND [IsActive] = 1;

        IF @Fee IS NULL
        BEGIN
            SELECT -1; -- tier not found or inactive
            RETURN;
        END

        -- 3. Update user's tier and expiry
        UPDATE [dbo].[Users]
        SET [TierID]         = @TierID,
            [TierExpiryDate] = DATEADD(DAY, -1, DATEADD(MONTH, @DurationMonths, CAST(GETDATE() AS DATE)))
        WHERE [UserID] = @UserID;

        -- 4. Record payment only if tier has a fee
        IF @Fee > 0
        BEGIN
            INSERT INTO [dbo].[Payments]
                ([UserID], [MemberShipTypeID], [PaymentType], [AmountPaid], [PaymentDate], [PaymentMethod], [Status])
            VALUES
                (@UserID, NULL, 'Tier', @Fee, GETDATE(), @PaymentMethod, 'Paid');
        END

        SELECT 1; -- success

    END TRY
    BEGIN CATCH
        THROW; -- surface SQL errors to C# with the real message + error number
    END CATCH

END
GO

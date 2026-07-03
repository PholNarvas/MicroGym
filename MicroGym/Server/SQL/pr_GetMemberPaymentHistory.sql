-- =============================================
-- Stored Procedure : pr_GetMemberPaymentHistory
-- Description      : Returns all payment records for a given member.
--                    Uses PaymentType to label Tier vs Plan payments.
--                    Newest first.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetMemberPaymentHistory]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Payments].[PaymentID],
        CASE
            WHEN [Payments].[PaymentType] = 'Tier' THEN [MembershipTiers].[TierName]
            ELSE [MembershipTypes].[Name]
        END                           AS [PlanName],
        CASE
            WHEN [Payments].[PaymentType] = 'Tier' THEN 'Tier'
            ELSE [MembershipTypes].[SubscriptionType]
        END                           AS [PlanType],
        [Payments].[AmountPaid],
        [Payments].[PaymentDate],
        [Payments].[PaymentMethod]
    FROM  [dbo].[Payments]
    LEFT JOIN [dbo].[MembershipTypes]
           ON [MembershipTypes].[MemberShipTypeID] = [Payments].[MemberShipTypeID]
    LEFT JOIN [dbo].[MembershipTiers]
           ON [Payments].[PaymentType] = 'Tier'
          AND [MembershipTiers].[TierID] = (
              SELECT [Users].[TierID]
              FROM   [dbo].[Users]
              WHERE  [Users].[UserId] = [Payments].[UserID]
          )
    WHERE [Payments].[UserID] = @UserID
    ORDER BY [Payments].[PaymentDate] DESC;
END
GO

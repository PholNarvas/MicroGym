-- =============================================
-- Stored Procedure : pr_GetRevenues
-- Description      : Returns all payment records for a given month/year
--                    including member name and plan name.
--                    Client filters by Status ('Paid' = income, 'Expense' = expense).
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetRevenues]
    @Months DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Payments].[PaymentID],
        [Payments].[UserID],
        [Users].[FirstName],
        [Users].[LastName],
        [Payments].[MemberShipTypeID],
        CASE
            WHEN [Payments].[PaymentType] = 'Tier' THEN [MembershipTiers].[TierName]
            ELSE [MembershipTypes].[Name]
        END                           AS [PlanName],
        [Payments].[AmountPaid],
        [Payments].[PaymentDate],
        [Payments].[PaymentMethod],
        [Payments].[Status],
        [Payments].[PaymentType]
    FROM [dbo].[Payments]
    LEFT JOIN [dbo].[Users]
           ON [Users].[UserID] = [Payments].[UserID]
    LEFT JOIN [dbo].[MembershipTypes]
           ON [MembershipTypes].[MemberShipTypeID] = [Payments].[MemberShipTypeID]
    LEFT JOIN [dbo].[MembershipTiers]
           ON [Payments].[PaymentType] = 'Tier'
          AND [MembershipTiers].[TierID] = (
              SELECT [Users].[TierID]
              FROM   [dbo].[Users]
              WHERE  [Users].[UserID] = [Payments].[UserID]
          )
    WHERE MONTH([Payments].[PaymentDate]) = MONTH(@Months)
      AND YEAR([Payments].[PaymentDate])  = YEAR(@Months)
    ORDER BY [Payments].[PaymentDate] DESC;
END
GO

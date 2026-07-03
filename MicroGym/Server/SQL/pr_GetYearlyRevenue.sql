-- =============================================
-- Stored Procedure : pr_GetYearlyRevenue
-- Description      : Returns total income (Status = 'Paid') for the current year.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetYearlyRevenue]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ISNULL(SUM([AmountPaid]), 0) AS [TotalRevenue]
    FROM [dbo].[Payments]
    WHERE YEAR([PaymentDate]) = YEAR(GETDATE())
      AND [Status] = 'Paid';
END
GO

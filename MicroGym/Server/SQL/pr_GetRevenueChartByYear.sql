-- =============================================
-- Stored Procedure : pr_GetRevenueChartByYear
-- Description      : Returns monthly income and expense totals for a given year.
--                    One row per month that has data. Client fills zeros for missing months.
--                    Replaces the 12-call pattern in LoadChartData.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[pr_GetRevenueChartByYear]
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MONTH([PaymentDate])                                                    AS [Month],
        SUM(CASE WHEN [Status] = 'Paid'    THEN [AmountPaid] ELSE 0 END)       AS [Income],
        SUM(CASE WHEN [Status] = 'Expense' THEN [AmountPaid] ELSE 0 END)       AS [Expense]
    FROM [dbo].[Payments]
    WHERE YEAR([PaymentDate]) = @Year
    GROUP BY MONTH([PaymentDate])
    ORDER BY [Month];
END
GO

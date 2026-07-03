-- =============================================
-- Stored Procedure: sp_GetAllUsers
-- Description: Returns all registered users from the Users table.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[sp_GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        UserId,
        MemberShipTypeID,
        FirstName,
        LastName,
        Email,
        Role,
        IsActive,
        Phone,
        CreatedAt,
        ExpiryDate,
        PaymentMethod,
        AmountPaid
    FROM Users
    ORDER BY CreatedAt DESC;
END

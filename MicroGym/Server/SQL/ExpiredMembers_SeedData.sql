-- =============================================
-- Seed Data : 20 Expired Members
-- MembershipTypes (actual):
--   1 = 1 Month Subscription    ₱1,000
--   2 = 3 Months Subscription   ₱2,800
--   3 = 1 Year Subscription     ₱10,000
--   5 = Boxing                  ₱2,500
--   6 = Taekwondo               ₱3,000
--   7 = 6 Months Subscription   ₱5,000
--   (4 = Walk-In skipped — single day, no expiry tracking needed)
--
-- All passwords : Test@123 (BCrypt cost 11)
-- IsActive      : 0 (expired / inactive)
-- Safe to re-run — skips existing emails
-- =============================================

SET NOCOUNT ON;

DECLARE @Hash NVARCHAR(MAX) = '$2b$11$tfjWKsfFP43FwHbyD92Xr.sEzkeOriudOCiYITC.17bzCt.tk.KcC';

-- ── Insert 20 expired members ───────────────────────────────────────────────
INSERT INTO [dbo].[Users]
    ([MemberShipTypeID],[FirstName],[LastName],[Email],[Phone],
     [PasswordHash],[Role],[IsActive],[Status],[CreatedAt],[ExpiryDate])
SELECT
    v.[TypeID], v.[FirstName], v.[LastName], v.[Email], v.[Phone],
    @Hash, 'Member', 0, 'Paid', v.[JoinDate], v.[ExpiryDate]
FROM (VALUES
    -- 1 Month Subscription (TypeID 1) — 4 members
    (1,'Andres',   'Villanueva', 'andres.v@expired.com',  '09700000001', DATEADD(MONTH,-1, GETDATE()), DATEADD(DAY,  -1, GETDATE())),
    (1,'Marites',  'Ocampo',     'marites.o@expired.com', '09700000002', DATEADD(MONTH,-2, GETDATE()), DATEADD(MONTH,-1, GETDATE())),
    (1,'Cecilia',  'Manalo',     'cecilia.m@expired.com', '09700000004', DATEADD(MONTH,-4, GETDATE()), DATEADD(MONTH,-3, GETDATE())),
    (1,'Hannah',   'Sicat',      'hannah.s@expired.com',  '09700000006', DATEADD(MONTH,-8, GETDATE()), DATEADD(MONTH,-7, GETDATE())),

    -- 3 Months Subscription (TypeID 2) — 4 members
    (2,'Roberto',  'Lacson',     'roberto.l@expired.com',    '09700000003', DATEADD(MONTH,-3, GETDATE()), DATEADD(DAY,  -5, GETDATE())),
    (2,'Ignacio',  'Buenaventura','ignacio.b@expired.com',   '09700000007', DATEADD(MONTH,-5, GETDATE()), DATEADD(MONTH,-2, GETDATE())),
    (2,'Lorena',   'Bautista',   'lorena.b@expired.com',     '09700000010', DATEADD(MONTH,-7, GETDATE()), DATEADD(MONTH,-4, GETDATE())),
    (2,'Rosario',  'Ilustre',    'rosario.i@expired.com',    '09700000016', DATEADD(MONTH,-10,GETDATE()), DATEADD(MONTH,-7, GETDATE())),

    -- 6 Months Subscription (TypeID 7) — 3 members
    (7,'Dominic',  'Ferrer',     'dominic.f@expired.com',    '09700000005', DATEADD(MONTH,-6, GETDATE()), DATEADD(DAY, -10, GETDATE())),
    (7,'Oscar',    'Fontanilla', 'oscar.f@expired.com',      '09700000013', DATEADD(MONTH,-9, GETDATE()), DATEADD(MONTH,-3, GETDATE())),
    (7,'Umberto',  'Legaspi',    'umberto.l@expired.com',    '09700000019', DATEADD(MONTH,-14,GETDATE()), DATEADD(MONTH,-8, GETDATE())),

    -- 1 Year Subscription (TypeID 3) — 3 members
    (3,'Josephine','Delos Reyes','josephine.dr@expired.com', '09700000008', DATEADD(MONTH,-13,GETDATE()), DATEADD(MONTH,-1, GETDATE())),
    (3,'Norma',    'Espejo',     'norma.e@expired.com',      '09700000012', DATEADD(MONTH,-14,GETDATE()), DATEADD(MONTH,-2, GETDATE())),
    (3,'Violeta',  'Macaraeg',   'violeta.m@expired.com',    '09700000020', DATEADD(MONTH,-15,GETDATE()), DATEADD(MONTH,-3, GETDATE())),

    -- Boxing (TypeID 5) — 3 members
    (5,'Kenneth',  'Agustin',    'kenneth.a@expired.com',    '09700000009', DATEADD(MONTH,-1, GETDATE()), DATEADD(DAY,  -7, GETDATE())),
    (5,'Manuel',   'Contreras',  'manuel.c@expired.com',     '09700000011', DATEADD(MONTH,-3, GETDATE()), DATEADD(MONTH,-2, GETDATE())),
    (5,'Sergio',   'Jimenez',    'sergio.j@expired.com',     '09700000017', DATEADD(MONTH,-9, GETDATE()), DATEADD(MONTH,-8, GETDATE())),

    -- Taekwondo (TypeID 6) — 3 members
    (6,'Perla',    'Galvez',     'perla.g@expired.com',      '09700000014', DATEADD(MONTH,-2, GETDATE()), DATEADD(DAY, -14, GETDATE())),
    (6,'Quirino',  'Herrera',    'quirino.h@expired.com',    '09700000015', DATEADD(MONTH,-4, GETDATE()), DATEADD(MONTH,-3, GETDATE())),
    (6,'Teresa',   'Katigbak',   'teresa.k@expired.com',     '09700000018', DATEADD(MONTH,-6, GETDATE()), DATEADD(MONTH,-5, GETDATE()))

) AS v([TypeID],[FirstName],[LastName],[Email],[Phone],[JoinDate],[ExpiryDate])
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Users] u WHERE u.[Email] = v.[Email]
);

-- ── Insert Plan payment per newly added member ──────────────────────────────
INSERT INTO [dbo].[Payments]
    ([UserID],[MemberShipTypeID],[AmountPaid],[PaymentDate],[PaymentMethod],[Status],[PaymentType])
SELECT
    u.[UserID],
    u.[MemberShipTypeID],
    CASE u.[MemberShipTypeID]
        WHEN 1 THEN 1000.00    -- 1 Month Subscription
        WHEN 2 THEN 2800.00    -- 3 Months Subscription
        WHEN 3 THEN 10000.00   -- 1 Year Subscription
        WHEN 5 THEN 2500.00    -- Boxing
        WHEN 6 THEN 3000.00    -- Taekwondo
        WHEN 7 THEN 5000.00    -- 6 Months Subscription
        ELSE 1000.00
    END,
    u.[CreatedAt],
    CASE (ABS(CHECKSUM(u.[Email])) % 3)
        WHEN 0 THEN 'Cash'
        WHEN 1 THEN 'GCash'
        ELSE        'Maya'
    END,
    'Paid',
    'Plan'
FROM [dbo].[Users] u
WHERE u.[Email] IN (
    'andres.v@expired.com','marites.o@expired.com','roberto.l@expired.com',
    'cecilia.m@expired.com','dominic.f@expired.com','hannah.s@expired.com',
    'ignacio.b@expired.com','josephine.dr@expired.com','kenneth.a@expired.com',
    'lorena.b@expired.com','manuel.c@expired.com','norma.e@expired.com',
    'oscar.f@expired.com','perla.g@expired.com','quirino.h@expired.com',
    'rosario.i@expired.com','sergio.j@expired.com','teresa.k@expired.com',
    'umberto.l@expired.com','violeta.m@expired.com'
)
AND NOT EXISTS (
    SELECT 1 FROM [dbo].[Payments] p
    WHERE p.[UserID] = u.[UserID] AND p.[PaymentType] = 'Plan'
);

-- ── Verify ──────────────────────────────────────────────────────────────────
SELECT
    mt.[Name]                   AS [MembershipType],
    COUNT(u.[UserID])           AS [ExpiredCount],
    SUM(p.[AmountPaid])         AS [TotalRevenue]
FROM [dbo].[Users] u
JOIN [dbo].[MembershipTypes] mt ON mt.[MemberShipTypeID] = u.[MemberShipTypeID]
LEFT JOIN [dbo].[Payments] p    ON p.[UserID] = u.[UserID] AND p.[PaymentType] = 'Plan'
WHERE u.[Email] IN (
    'andres.v@expired.com','marites.o@expired.com','roberto.l@expired.com',
    'cecilia.m@expired.com','dominic.f@expired.com','hannah.s@expired.com',
    'ignacio.b@expired.com','josephine.dr@expired.com','kenneth.a@expired.com',
    'lorena.b@expired.com','manuel.c@expired.com','norma.e@expired.com',
    'oscar.f@expired.com','perla.g@expired.com','quirino.h@expired.com',
    'rosario.i@expired.com','sergio.j@expired.com','teresa.k@expired.com',
    'umberto.l@expired.com','violeta.m@expired.com'
)
GROUP BY mt.[Name]
ORDER BY mt.[Name];

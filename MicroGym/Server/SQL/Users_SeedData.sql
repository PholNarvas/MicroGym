-- =============================================
-- Users Seed Data — 50 Dummy Members
-- Password for all accounts: Test@123
-- Hash: BCrypt cost 11
-- Run this in SSMS against your MicroGym database.
--
-- Assumes MembershipTypeIDs:
--   1 = Boxing / TaekWondo
--   2 = Gym
--   3 = Zumba
-- Adjust IDs if yours differ.
-- =============================================

SET IDENTITY_INSERT [dbo].[Users] OFF;

DECLARE @Hash NVARCHAR(MAX) = '$2b$11$tfjWKsfFP43FwHbyD92Xr.sEzkeOriudOCiYITC.17bzCt.tk.KcC';

INSERT INTO [dbo].[Users]
    ([MemberShipTypeID],[FirstName],[LastName],[Email],[Phone],[PasswordHash],[Role],[IsActive],[Status],[CreatedAt],[ExpiryDate])
VALUES
-- ── Gym Members (Active) ──────────────────────────────────────────────────
(2,'Juan','Santos',       'juan.santos@email.com',     '09171234501',@Hash,'Member',1,'Paid', DATEADD(MONTH,-5,GETDATE()), DATEADD(MONTH, 1,GETDATE())),
(2,'Maria','Reyes',       'maria.reyes@email.com',     '09181234502',@Hash,'Member',1,'Paid', DATEADD(MONTH,-4,GETDATE()), DATEADD(MONTH, 2,GETDATE())),
(2,'Carlo','Cruz',        'carlo.cruz@email.com',      '09191234503',@Hash,'Member',1,'Paid', DATEADD(MONTH,-3,GETDATE()), DATEADD(MONTH, 3,GETDATE())),
(2,'Ana','Garcia',        'ana.garcia@email.com',      '09201234504',@Hash,'Member',1,'Paid', DATEADD(MONTH,-2,GETDATE()), DATEADD(MONTH, 4,GETDATE())),
(2,'Miguel','Flores',     'miguel.flores@email.com',   '09211234505',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(MONTH, 5,GETDATE())),
(2,'Sofia','Mendoza',     'sofia.mendoza@email.com',   '09221234506',@Hash,'Member',1,'Paid', DATEADD(DAY,  -20,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(2,'Luis','Torres',       'luis.torres@email.com',     '09231234507',@Hash,'Member',1,'Paid', DATEADD(DAY,  -15,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(2,'Isabela','Ramos',     'isabela.ramos@email.com',   '09241234508',@Hash,'Member',1,'Paid', DATEADD(DAY,  -10,GETDATE()),DATEADD(MONTH, 2,GETDATE())),
(2,'Diego','Lim',         'diego.lim@email.com',       '09251234509',@Hash,'Member',1,'Paid', DATEADD(DAY,   -7,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(2,'Rosa','Aquino',       'rosa.aquino@email.com',     '09261234510',@Hash,'Member',1,'Paid', DATEADD(DAY,   -3,GETDATE()),DATEADD(MONTH, 1,GETDATE())),

-- ── Boxing / TaekWondo Members (Active) ──────────────────────────────────
(1,'Marco','Dela Cruz',   'marco.delacruz@email.com',  '09271234511',@Hash,'Member',1,'Paid', DATEADD(MONTH,-6,GETDATE()), DATEADD(MONTH, 2,GETDATE())),
(1,'Claire','Bautista',   'claire.bautista@email.com', '09281234512',@Hash,'Member',1,'Paid', DATEADD(MONTH,-4,GETDATE()), DATEADD(MONTH, 2,GETDATE())),
(1,'Patrick','Villanueva','patrick.villa@email.com',   '09291234513',@Hash,'Member',1,'Paid', DATEADD(MONTH,-3,GETDATE()), DATEADD(MONTH, 3,GETDATE())),
(1,'Grace','Castillo',    'grace.castillo@email.com',  '09301234514',@Hash,'Member',1,'Paid', DATEADD(MONTH,-2,GETDATE()), DATEADD(MONTH, 4,GETDATE())),
(1,'Ryan','Morales',      'ryan.morales@email.com',    '09311234515',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(MONTH, 5,GETDATE())),
(1,'Lea','Navarro',       'lea.navarro@email.com',     '09321234516',@Hash,'Member',1,'Paid', DATEADD(DAY,  -25,GETDATE()),DATEADD(MONTH, 2,GETDATE())),
(1,'Jerome','Pascual',    'jerome.pascual@email.com',  '09331234517',@Hash,'Member',1,'Paid', DATEADD(DAY,  -18,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(1,'Tricia','Espinosa',   'tricia.espinosa@email.com', '09341234518',@Hash,'Member',1,'Paid', DATEADD(DAY,  -12,GETDATE()),DATEADD(MONTH, 2,GETDATE())),
(1,'Nico','Fuentes',      'nico.fuentes@email.com',    '09351234519',@Hash,'Member',1,'Paid', DATEADD(DAY,   -5,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(1,'Carla','Salazar',     'carla.salazar@email.com',   '09361234520',@Hash,'Member',1,'Paid', DATEADD(DAY,   -2,GETDATE()),DATEADD(MONTH, 3,GETDATE())),

-- ── Zumba Members (Active) ────────────────────────────────────────────────
(3,'Liza','Domingo',      'liza.domingo@email.com',    '09371234521',@Hash,'Member',1,'Paid', DATEADD(MONTH,-5,GETDATE()), DATEADD(MONTH, 1,GETDATE())),
(3,'Gina','Soriano',      'gina.soriano@email.com',    '09381234522',@Hash,'Member',1,'Paid', DATEADD(MONTH,-3,GETDATE()), DATEADD(MONTH, 3,GETDATE())),
(3,'Nadia','Medina',      'nadia.medina@email.com',    '09391234523',@Hash,'Member',1,'Paid', DATEADD(MONTH,-2,GETDATE()), DATEADD(MONTH, 2,GETDATE())),
(3,'Rachel','Vergara',    'rachel.vergara@email.com',  '09401234524',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(MONTH, 4,GETDATE())),
(3,'Vanessa','Ocampo',    'vanessa.ocampo@email.com',  '09411234525',@Hash,'Member',1,'Paid', DATEADD(DAY,  -22,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(3,'Bianca','Tan',        'bianca.tan@email.com',      '09421234526',@Hash,'Member',1,'Paid', DATEADD(DAY,  -16,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(3,'Maricel','Aguilar',   'maricel.aguilar@email.com', '09431234527',@Hash,'Member',1,'Paid', DATEADD(DAY,   -9,GETDATE()),DATEADD(MONTH, 2,GETDATE())),
(3,'Joyce','Padilla',     'joyce.padilla@email.com',   '09441234528',@Hash,'Member',1,'Paid', DATEADD(DAY,   -4,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(3,'Sheila','Miranda',    'sheila.miranda@email.com',  '09451234529',@Hash,'Member',1,'Paid', DATEADD(DAY,   -1,GETDATE()),DATEADD(MONTH, 3,GETDATE())),
(3,'Felicia','Gutierrez', 'felicia.g@email.com',       '09461234530',@Hash,'Member',1,'Paid', GETDATE(),                   DATEADD(MONTH, 2,GETDATE())),

-- ── Expiring This Week (all plans) ───────────────────────────────────────
(2,'Kevin','Hernandez',   'kevin.hernandez@email.com', '09471234531',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(DAY,  3,GETDATE())),
(1,'Alma','Reyes',        'alma.reyes2@email.com',     '09481234532',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(DAY,  5,GETDATE())),
(3,'Joel','Santos',       'joel.santos@email.com',     '09491234533',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(DAY,  2,GETDATE())),
(2,'Tanya','Flores',      'tanya.flores@email.com',    '09501234534',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(DAY,  6,GETDATE())),
(1,'Harold','Cruz',       'harold.cruz@email.com',     '09511234535',@Hash,'Member',1,'Paid', DATEADD(MONTH,-1,GETDATE()), DATEADD(DAY,  1,GETDATE())),

-- ── Inactive / Expired Members ────────────────────────────────────────────
(2,'Paolo','Bello',       'paolo.bello@email.com',     '09521234536',@Hash,'Member',0,'Paid', DATEADD(MONTH,-8,GETDATE()), DATEADD(MONTH,-2,GETDATE())),
(1,'Sandra','Estrada',    'sandra.estrada@email.com',  '09531234537',@Hash,'Member',0,'Paid', DATEADD(MONTH,-7,GETDATE()), DATEADD(MONTH,-1,GETDATE())),
(3,'Ronnie','Dela Torre', 'ronnie.delatorre@email.com','09541234538',@Hash,'Member',0,'Paid', DATEADD(MONTH,-9,GETDATE()), DATEADD(MONTH,-3,GETDATE())),
(2,'Elena','Pascual',     'elena.pascual@email.com',   '09551234539',@Hash,'Member',0,'Paid', DATEADD(MONTH,-6,GETDATE()), DATEADD(MONTH,-1,GETDATE())),
(1,'Jerome','Lacson',     'jerome.lacson@email.com',   '09561234540',@Hash,'Member',0,'Paid', DATEADD(MONTH,-5,GETDATE()), DATEADD(DAY,  -5,GETDATE())),
(3,'Marjorie','Chan',     'marjorie.chan@email.com',   '09571234541',@Hash,'Member',0,'Paid', DATEADD(MONTH,-4,GETDATE()), DATEADD(DAY,  -3,GETDATE())),
(2,'Dennis','Alvarez',    'dennis.alvarez@email.com',  '09581234542',@Hash,'Member',0,'Paid', DATEADD(MONTH,-3,GETDATE()), DATEADD(DAY,  -1,GETDATE())),
(1,'Cynthia','Roque',     'cynthia.roque@email.com',   '09591234543',@Hash,'Member',0,'Paid', DATEADD(MONTH,-6,GETDATE()), DATEADD(MONTH,-2,GETDATE())),
(3,'Francis','Tolentino', 'francis.t@email.com',       '09601234544',@Hash,'Member',0,'Paid', DATEADD(MONTH,-8,GETDATE()), DATEADD(MONTH,-4,GETDATE())),
(2,'Rowena','Catalan',    'rowena.catalan@email.com',  '09611234545',@Hash,'Member',0,'Paid', DATEADD(MONTH,-7,GETDATE()), DATEADD(MONTH,-2,GETDATE())),

-- ── Mixed Recent Signups ──────────────────────────────────────────────────
(2,'Arthur','Mercado',    'arthur.mercado@email.com',  '09621234546',@Hash,'Member',1,'Paid', DATEADD(DAY,  -1,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(1,'Jasmine','Yap',       'jasmine.yap@email.com',     '09631234547',@Hash,'Member',1,'Paid', DATEADD(DAY,  -2,GETDATE()),DATEADD(MONTH, 2,GETDATE())),
(3,'Bernard','Ong',       'bernard.ong@email.com',     '09641234548',@Hash,'Member',1,'Paid', DATEADD(DAY,  -3,GETDATE()),DATEADD(MONTH, 1,GETDATE())),
(2,'Patricia','Delos Santos','patricia.ds@email.com',  '09651234549',@Hash,'Member',1,'Paid', DATEADD(DAY,  -4,GETDATE()),DATEADD(MONTH, 3,GETDATE())),
(1,'Eduardo','Manalo',    'eduardo.manalo@email.com',  '09661234550',@Hash,'Member',1,'Paid', DATEADD(DAY,  -6,GETDATE()),DATEADD(MONTH, 2,GETDATE()));

-- =============================================
-- Verify
-- =============================================
SELECT COUNT(*) AS TotalSeeded FROM [dbo].[Users] WHERE [Role] = 'Member';

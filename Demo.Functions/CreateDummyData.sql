IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Role')
BEGIN
	CREATE TABLE dbo.[Role]
	(
		Id int NOT NULL PRIMARY KEY,
		[Name] nvarchar(100) NOT NULL
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User')
BEGIN	
	CREATE TABLE dbo.[User] 
	(
		Id int IDENTITY(1, 1) PRIMARY KEY,
		[Name] nvarchar(100) NOT NULL,
		Email nvarchar(255) NOT NULL,
		Mobile nvarchar(20) NOT NULL,
		RoleId int NOT NULL,
		CONSTRAINT [FK_User_Role] FOREIGN KEY (RoleId)
			REFERENCES dbo.[Role] (Id)
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Location')
BEGIN
	CREATE TABLE dbo.[Location]
	(
		Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
		Address1 nvarchar(100) NOT NULL,
		Address2 nvarchar(100) NULL,
		City nvarchar(100) NOT NULL,
		Postcode nvarchar(4) NOT NULL,
		[State] nvarchar(20) NOT NULL
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RentalProperty')
BEGIN
	CREATE TABLE dbo.[RentalProperty]
	(
		Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
		LocationId int NOT NULL,
		Bedrooms int NOT NULL,
		Bathrooms int NOT NULL,
		Carports int NOT NULL,
		Constraint [FK_RentalProperty_Location] FOREIGN KEY (LocationId)
			REFERENCES dbo.[Location] (Id)
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RentArrangement')
BEGIN
	CREATE TABLE dbo.[RentArrangement]
	(
		Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
		ClientId int NOT NULL,
		PropertyId int NOT NULL,
		ManagerId int NOT NULL,
		StartDate datetime NOT NULL,
		EndDate datetime NOT NULL,
		WeeklyRent decimal(20,4) NOT NULL,
		ReferenceId UNIQUEIDENTIFIER NOT NULL 
			CONSTRAINT [DF_RentArrangement_ReferenceId] DEFAULT NEWID(),
		CONSTRAINT [FK_RentArrangement_ClientId_User] FOREIGN KEY (ClientId)
			REFERENCES dbo.[User] (Id),
		CONSTRAINT [FK_RentArrangement_ManagerId_User] FOREIGN KEY (ManagerId)
			REFERENCES dbo.[User] (Id),
		CONSTRAINT [FK_RentArrangement_RentalProperty] FOREIGN KEY (PropertyId)
			REFERENCES dbo.[RentalProperty] (Id)
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'WeeklyCalendar')
BEGIN
	CREATE TABLE dbo.[WeeklyCalendar]
	(
		Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
		[Year] int NOT NULL,
		[Week] int NOT NULL,
		StartDate DATETIME NOT NULL,
		EndDate DATETIME NOT NULL
	)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RentPayment')
BEGIN
	CREATE TABLE dbo.[RentPayment]
	(
		Id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
		RentArrangementId int NOT NULL,
		WeeklyCalendarId int NOT NULL,
		DueAmount decimal(20,4) NOT NULL,
		DueDate datetime NOT NULL,
		Paid decimal(20,4) NULL,
		PaidDate datetime NULL 
	)
END
GO

INSERT INTO dbo.[Role] (Id, [Name])
SELECT 1, 'Client'
WHERE NOT EXISTS (SELECT 1 FROM dbo.[Role] WHERE [Name] = 'Client');
INSERT INTO dbo.[Role] (Id, [Name])
SELECT 2, 'Property Manager'
WHERE NOT EXISTS (SELECT 1 FROM dbo.[Role] WHERE [Name] = 'Property Manager');
GO

INSERT INTO dbo.[User] ([Name], Email, Mobile, RoleId)
SELECT '2cuteSuper', '2cuteSuper@test.com', '0430111111', 1
WHERE NOT EXISTS (SELECT * FROM dbo.[User] WHERE [Name] = '2cuteSuper');
INSERT INTO dbo.[User] ([Name], Email, Mobile, RoleId)
SELECT 'BitSky', 'bitsky@test.com', '0430222222', 1
WHERE NOT EXISTS (SELECT * FROM dbo.[User] WHERE [Name] = 'BitSky');
INSERT INTO dbo.[User] ([Name], Email, Mobile, RoleId)
SELECT 'Critical', 'Critical@test.com', '0430333333', 1
WHERE NOT EXISTS (SELECT * FROM dbo.[User] WHERE [Name] = 'Critical');
INSERT INTO dbo.[User] ([Name], Email, Mobile, RoleId)
SELECT 'lilin', 'linli1506@gmail.com', '0430444444', 2
WHERE NOT EXISTS (SELECT * FROM dbo.[User] WHERE [Name] = 'lilin');
GO

-- $460, br 2 bathroom 1 cp 1
DECLARE 
	@LocationId int,
	@PropertyId int;
IF NOT EXISTS (SELECT * FROM dbo.[Location] WHERE Address1 = '8/14 Creswick Street')
BEGIN
	INSERT INTO dbo.[Location] (Address1, City, Postcode, [State])
	SELECT '8/14 Creswick Street', 'Hawthorn', '3122', 'VIC';
	SET @LocationId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentalProperty (LocationId, Bedrooms, Bathrooms, Carports)
	SELECT @LocationId, 2, 1, 1;
	SET @PropertyId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentArrangement (ClientId, PropertyId, ManagerId, StartDate, EndDate, WeeklyRent)
	SELECT 
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = '2cuteSuper'),
		@PropertyId,
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = 'lilin'),
		'2020-02-01',
		'2020-03-01',
		460
END

-- $600, br 2 bathroom 1 cp 1
IF NOT EXISTS (SELECT * FROM dbo.[Location] WHERE Address1 = '53 Chrystobel Crescent')
BEGIN
	INSERT INTO dbo.[Location] (Address1, City, Postcode, [State])
	SELECT '53 Chrystobel Crescent', 'Hawthorn', '3122', 'VIC'
	SET @LocationId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentalProperty (LocationId, Bedrooms, Bathrooms, Carports)
	SELECT @LocationId, 2, 1, 1;
	SET @PropertyId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentArrangement (ClientId, PropertyId, ManagerId, StartDate, EndDate, WeeklyRent)
	SELECT 
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = 'BitSky'),
		@PropertyId,
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = 'lilin'),
		'2020-02-01',
		'2020-04-01',
		600
END

-- $465, br 1 bathroom 1 cp 1
IF NOT EXISTS (SELECT * FROM dbo.[Location] WHERE Address1 = '510/29-31 Queens Avenue')
BEGIN
	INSERT INTO dbo.[Location] (Address1, City, Postcode, [State])
	SELECT '510/29-31 Queens Avenue', 'Hawthorn', '3122', 'VIC'
	SET @LocationId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentalProperty (LocationId, Bedrooms, Bathrooms, Carports)
	SELECT @LocationId, 1, 1, 1;
	SET @PropertyId = SCOPE_IDENTITY();
	
	INSERT INTO dbo.RentArrangement (ClientId, PropertyId, ManagerId, StartDate, EndDate, WeeklyRent)
	SELECT 
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = 'Critical'),
		@PropertyId,
		(SELECT TOP 1 Id FROM dbo.[User] WHERE [Name] = 'lilin'),
		'2020-02-01',
		'2020-05-01',
		600
END
GO


DECLARE 
	@StartDate DATETIME,
	@EndDate DATETIME;
	
SELECT 
	@StartDate = '2020-01-01',
	@EndDate = '2020-06-01'

IF NOT EXISTS (SELECT 1 FROM dbo.WeeklyCalendar)
BEGIN
	WHILE (@StartDate < @EndDate)
	BEGIN
		INSERT INTO dbo.WeeklyCalendar ([Year], [Week], StartDate, EndDate)
		SELECT 
			DATEPART(YEAR, @StartDate) AS [Year],
			DATEPART(WEEK, @StartDate) AS [Week], 
			DATEADD(dd, -(DATEPART(dw, @StartDate)-1), @StartDate) AS WeekStart,
			DATEADD(ms, -3, DATEADD(dd, 8-(DATEPART(dw, @StartDate)), @StartDate)) [WeekEnd]

		SELECT @StartDate = DATEADD(DAY, 7, @StartDate)
	END
END
GO
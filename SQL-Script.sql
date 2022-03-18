-- Use Master
USE master
GO

--Create User
CREATE LOGIN [Astrow-v2] WITH PASSWORD=N'Albinolover69', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

--Drop Database
ALTER  DATABASE  [Astrow-2.0] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE IF EXISTS [Astrow-2.0]
GO

-- Create Database
CREATE DATABASE [Astrow-2.0]
GO

-- Use Astrow-2.0
USE [Astrow-2.0]
GO

CREATE USER [Astrow-v2] FOR LOGIN [Astrow-v2]
GO

ALTER ROLE [db_accessadmin] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_backupoperator] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_datareader] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_datawriter] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_ddladmin] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_owner] ADD MEMBER [Astrow-v2]
GO

ALTER ROLE [db_securityadmin] ADD MEMBER [Astrow-v2]
GO

-------------- Create Table --------------

-- Users Name
CREATE TABLE [Name] (
[Name_ID] INT IDENTITY(1,1) NOT NULL,
[FirstName] NVARCHAR(15) NOT NULL,
[MiddleName] NVARCHAR(15),
[LastName] NVARCHAR(15) NOT NULL,
[FullName] NVARCHAR(50) NOT NULL

PRIMARY KEY ([Name_ID])
)
GO


-- User
CREATE TABLE [User] (
[User_ID] INT IDENTITY(1,1) NOT NULL,
[UserName] NVARCHAR(20) NOT NULL,
[Password] NVARCHAR(MAX) NOT NULL,
[Name_ID] INT NOT NULL,
[Status] NVARCHAR(25) NOT NULL,
[StartDate] DATETIME NOT NULL,
[EndDate] DATETIME NOT NULL,
[Salt] NVARCHAR(max) NOT NULL,
[IsDeleted] BIT NOT NULL


PRIMARY KEY([User_ID]),
FOREIGN KEY ([Name_ID]) REFERENCES [Name]([Name_ID])
)
GO

CREATE TABLE [AbscenseType] (
[AbscenseID] INT IDENTITY(1,1) NOT NULL,
[Type] NVARCHAR(150) NOT NULL

PRIMARY KEY([AbscenseID])
)
GO

CREATE TABLE [RequestAbscense] (
[RequestID] INT IDENTITY(1,1) NOT NULL,
[User_ID] INT NOT NULL,
[AbscenseID] INT NOT NULL,
[Text] NVARCHAR(MAX),
[Date] DATETIME NOT NULL,
[Answered] BIT,
[SecDate] DATETIME

PRIMARY KEY([RequestID]),
FOREIGN KEY ([AbscenseID]) REFERENCES [AbscenseType]([AbscenseID]),
FOREIGN KEY ([User_ID]) REFERENCES [User]([User_ID])
)
GO

-- Days
CREATE TABLE [Days] (
[Days_ID] INT IDENTITY(1,1) NOT NULL,
[User_ID] INT NOT NULL,
[AbscenseID] INT,
[Date] DATETIME NOT NULL,
[AbscenceText] NVARCHAR(MAX),
[StartDay] DATETIME,
[EndDay] DATETIME,
[Min] INT,
[Hour] INT,
[Saldo] NVARCHAR(10),
[TotalMin] INT,
[TotalHour] INT,
[TotalSaldo] NVARCHAR(10)

PRIMARY KEY([Days_ID]),
FOREIGN KEY ([User_ID]) REFERENCES [User]([User_ID]),
FOREIGN KEY ([AbscenseID]) REFERENCES [AbscenseType]([AbscenseID]),
)
GO



---------------------------- Create Admin user ----------------------------
INSERT INTO [Name] ([FirstName], [MiddleName], [LastName], [FullName])
VALUES ('Adam', 'The', 'Admin', 'Adam The Admin')
GO

INSERT INTO [User] ([UserName], [Password], [Name_ID], [Status], [StartDate], [EndDate], [Salt], [IsDeleted])
VALUES ('Admin', 'SPagn++JbYrbIp2/QqKNVA1UU3C572UZ1iNp9lEjMC8=', 1, 'Instructør', '200720 00:00:00 AM', '250101 00:00:00 AM','Roh+S6zwQ9vnp8Soaxw+tA==', 0)
GO

-- Abscense types
INSERT INTO [AbscenseType] ([Type])
VALUES ('Igen')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Sygdom')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Læge/Tandlæge/Fysioterapi')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Køretime')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Jobsøgning')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Flexfri')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('SBO Ferie')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Skoleophold')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('COVID')
GO

INSERT INTO [AbscenseType] ([Type])
VALUES ('Ulovligt fravær')
GO

-- Days

INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220306 00:00:00 AM', 1, '', '220306 08:00:00 AM', '220306 15:24:00 PM',0 ,0, '00:00', 0, 0, '00:00')
GO

INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220307 00:00:00 AM', 1, '', '220307 08:00:00 AM', '220307 15:00:00 PM', -24, 0, '-00:24', -24, 0, '-00:24')
GO


INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220308 00:00:00 AM', 1, '', '220308 08:00:00 AM', '220308 16:24:00 PM',0 ,1, '01:00', 36, 0, '00:36')
GO



---------------------------- Create Procedures ----------------------------
 
-- Create User
CREATE PROCEDURE [CreateUser]
@id INT,
@UserName NVARCHAR(20),
@Password NVARCHAR(MAX),
@Status NVARCHAR(25),
@salt NVARCHAR(max),
@startDate DATETIME,
@endDate DATETIME
AS
INSERT INTO [User] ([UserName], [Password], [Name_ID], [Status], [IsDeleted], [Salt], [StartDate], [EndDate])
VALUES (@UserName, @Password, @id, @Status, 0, @salt, @startDate, @endDate)
GO


-- Create Days
CREATE PROCEDURE [CreateDay]
@id INT,
@date DATETIME,
@abscenseID INT,
@abscenseText NVARCHAR(MAX),
@startDay DATETIME,
@endDay DATETIME,
@min INT,
@hour INT,
@saldo NVARCHAR(10),
@toMin INT,
@toHour INT,
@totalSaldo NVARCHAR(10)
AS
INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (@id, @date, @abscenseID, @abscenseText, @startDay, @endDay, @min, @hour, @saldo, @toMin, @toHour, @totalSaldo) 
GO

-- Create Name
CREATE PROCEDURE [CreateName]
@firstName NVARCHAR(15),
@middleName NVARCHAR(15),
@lastName NVARCHAR(15),
@fullName NVARCHAR(50)
AS
INSERT INTO [Name] ([FirstName], [MiddleName], [LastName], [FullName])
VALUES (@firstName, @middleName, @lastName, @firstName + ' ' + @middleName + ' ' + @lastName) 
GO


-- Create Request
CREATE PROCEDURE [CreateRequest]
@UserID INT,
@AbsID INT,
@Text NVARCHAR(MAX),
@Date DATETIME,
@SecDate DATETIME
AS
INSERT INTO [RequestAbscense] ([User_ID], [AbscenseID], [Text], [Date], [SecDate])
VALUES (@UserID, @AbsID, @Text, @Date, @secDate)
GO


---------------------------- Update ----------------------------

-- Update request
CREATE PROCEDURE [UpdateRequest]
@ID INT,
@UserID INT,
@AbsID INT,
@Text NVARCHAR(MAX),
@Date DATETIME,
@SecDate DATETIME
AS
UPDATE [RequestAbscense] 
SET [User_ID] = @UserID,
[AbscenseID] = @AbsID,
[Text] = @Text,
[Date] = @Date,
[SecDate] = @SecDate
WHERE [RequestID] = @ID
GO

-- Update request answer
CREATE PROCEDURE [UpdateRequestAnswered]
@ID INT,
@Answered BIT
AS
UPDATE [RequestAbscense]
SET [Answered] = @Answered
WHERE [RequestID] = @ID
GO

--Upate day
CREATE PROCEDURE [UpdateDay]
@id INT,
@date DATETIME,
@abscenseID INT,
@abscenseText NVARCHAR(MAX),
@startDay DATETIME,
@endDay DATETIME,
@min INT,
@hour INT,
@saldo NVARCHAR(10),
@toMin INT,
@toHour INT,
@totalSaldo NVARCHAR(10)
AS
UPDATE [Days]
SET [Date] = @date,
[StartDay] = @startDay,
[EndDay] = @endDay,
[AbscenseID] = @abscenseID,
[AbscenceText] = @abscenseText,
[Min] = @min,
[Hour] = @hour,
[Saldo] = @saldo,
[TotalMin] = @toMin,
[TotalHour] = @toHour,
[TotalSaldo] = @totalSaldo
WHERE  [Days_ID] = @id
GO

-- Update
CREATE PROCEDURE [UpdateUser]
@id INT,
@UserName NVARCHAR(20),
@Password NVARCHAR(MAX),
@Status NVARCHAR(25),
@startDate DATETIME,
@endDate DATETIME
AS
UPDATE [User]
SET [UserName] = @UserName,
[Password] = @Password,
[Status] = @Status,
[StartDate] = @startDate,
[EndDate] = @endDate
WHERE [User_ID] = @id
GO


-- Update User info
CREATE PROCEDURE [UpdateUserInfo]
@id INT,
@firstNavn NVARCHAR(15),
@middleNavn NVARCHAR(15),
@lastNavn NVARCHAR(15)
AS
UPDATE [Name]
SET [FirstName] = @firstNavn,
[MiddleName] = @middleNavn,
[LastName] = @lastNavn
WHERE [Name_ID] = @id
GO

-- Update Abscence
CREATE PROCEDURE [UpdateAbscence]
@id INT,
@AbscenceText NVARCHAR(100)
AS
UPDATE [Days]
SET [AbscenceText] = @AbscenceText
WHERE [Days_ID] = @id
GO

-- Update Start day
CREATE PROCEDURE [UpdateStart]
@id INT,
@StartDay DATETIME
AS
UPDATE [Days]
SET [StartDay] = @StartDay
WHERE [Days_ID] = @id
GO

-- Update End day
CREATE PROCEDURE [UpdateEnd]
@id INT,
@EndDay DATETIME
AS
UPDATE [Days]
SET [EndDay] = @EndDay
WHERE [Days_ID] = @id
GO

-- Update Abscense type
CREATE PROCEDURE [UpdateAbsencseType]
@id INT,
@dayID INT
AS
UPDATE [Days]
SET [AbscenseID] = @id
WHERE [Days_ID] = @dayID
GO

-- Update Saldo
CREATE PROCEDURE [UpdateSaldo]
@id INT,
@min INT,
@hour INT,
@Saldo NVARCHAR(10)
AS
UPDATE [Days]
SET [Min] = @min,
[Hour] = @hour,
[Saldo] = @Saldo
WHERE [Days_ID] = @id
GO


-- Update TotalSaldo
CREATE PROCEDURE [UpdateTotalSaldo]
@id INT,
@totalMin INT,
@totalHour INT,
@saldo NVARCHAR(10)
AS
UPDATE [Days]
SET [TotalMin] = @totalMin,
[TotalHour] = @totalHour,
[TotalSaldo] = @saldo
WHERE [Days_ID] = @id
GO
---------------------------- Delete ----------------------------


-- Delete
CREATE PROCEDURE [DeleteUser]
@id INT
AS
UPDATE [User]
SET [IsDeleted] = 1
WHERE [User_ID] = @id
GO



---------------------------- Read ----------------------------

-- Find request by id
CREATE PROCEDURE [FindRequest]
@ID INT
AS
SELECT * FROM [RequestAbscense]
WHERE [RequestID] = @ID
GO

-- Get all request
CREATE PROCEDURE [GetRequests]
AS
SELECT [RequestID], [User_ID], [AbscenseID], [Text], [Date], [SecDate] FROM [RequestAbscense]
GO


-- Read All
CREATE PROCEDURE [ReadAllUsers]
AS
SELECT * FROM [User]
WHERE [IsDeleted] = 0
GO

-- Get by ID
CREATE PROCEDURE [GetByID]
@id INT
AS
SELECT * FROM [User]
WHERE [User_ID] = @id
GO

-- Get by UserName
CREATE PROCEDURE [GetByUserName]
@UserName NVARCHAR(20)
AS
SELECT [salt], [User_ID] FROM [User]
WHERE [UserName] = @UserName
GO

-- Get User 
CREATE PROCEDURE [GetUser]
@userName NVARCHAR(20),
@password NVARCHAR(MAX)
AS
SELECT * FROM [User]
WHERE [UserName] = @userName AND [Password] = @password
GO

-- Get user info
CREATE PROCEDURE [GetUserInfo]
@fullName NVARCHAR(50)
AS
SELECT [Name_ID] FROM [Name]
WHERE [FullName] = @fullName
GO

-- Find all days by id and date
CREATE PROCEDURE [FindAllDays]
@id INT,
@date DATETIME
AS
SELECT * FROM Days
WHERE [User_ID] = @id AND [Date] = @date
GO

-- Find all days by id
CREATE PROCEDURE [FindAllDaysByID]
@id INT
AS
SELECT * FROM Days
WHERE [User_ID] = @id
GO

-- Get user info by id
CREATE PROCEDURE [FindUserInfo]
@id INT
AS
SELECT * FROM Name
WHERE [Name_ID] = @id
GO

-- Get day by start date and id
CREATE PROCEDURE [FindDay]
@Date DATETIME,
@id INT
AS
SELECT * FROM Days
WHERE [Date] = @Date AND [User_ID] = @id
GO

-- Find day by id
CREATE PROCEDURE [FindDayByID]
@id INT
AS
SELECT * FROM [Days]
WHERE [Days_ID] = @id
GO

--Find saldo of the day before
CREATE PROCEDURE [FindTotalSaldo]
AS
SELECT * FROM [Days]
WHERE [Days_ID] =( 
	SELECT MAX([Days_ID]) FROM [Days])
GO


-- Get all abscense type
CREATE PROCEDURE [GetAllAbscenseTypes]
AS
SELECT * FROM [AbscenseType]
GO


-- Get Abscense type text from id
CREATE PROCEDURE [GetAbscenseText]
AS
SELECT * FROM [AbscenseType]
GO

-- Find abscense by text
CREATE PROCEDURE [FindAbscenseByText]
@Text NVARCHAR(30)
AS
SELECT * FROM [AbscenseType]
WHERE [Type] = @Text
GO




-- Astrow-Create-Day

DECLARE @Users INT
SET @Users = (SELECT DISTINCT MIN([User_ID]) FROM [Days] WHERE [Date] != GETDATE())

WHILE (@Users <= (SELECT DISTINCT MAX([User_ID]) FROM [Days] WHERE [Date] != GETDATE()))
	BEGIN
	IF ((SELECT [Date] FROM [Days] WHERE [Date] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd'))) AND [User_ID] = @Users) IS NULL)
	 BEGIN
			INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
			VALUES (@Users, (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd'))), 1, '', (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd'))), 
			(SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd'))), 0, 0, '00:00', (SELECT [TotalMin] FROM [Days] WHERE [Days_ID] = 
			(SELECT MAX([Days_ID]) FROM [Days])), (SELECT [TotalHour] FROM [Days] WHERE [Days_ID] = (SELECT MAX([Days_ID]) FROM [Days])), (SELECT [TotalSaldo] FROM [Days] WHERE [Days_ID] = (SELECT MAX([Days_ID]) FROM [Days])))
	 END
		SET @Users = @Users + 1
	END
GO

-- Astrow-Auto-Abscense

DECLARE @Users INT,@TotalHour INT, @TotalMin INT, @MinHolder INT, @HourHolder INT, @HourStr NVARCHAR(10), @MinStr NVARCHAR(10)
SET @Users = (SELECT DISTINCT MIN([User_ID]) FROM [Days] WHERE [StartDay] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd'))))

SET @TotalHour = (SELECT [TotalHour] FROM [Days] WHERE [Days_ID] = (SELECT MAX([Days_ID]) FROM [Days])) 
SET @TotalMin = (SELECT [TotalMin] FROM [Days] WHERE [Days_ID] = (SELECT MAX([Days_ID]) FROM [Days]))

SET @TotalMin = @TotalMin - 24
SET @TotalHour = @TotalHour - 7


-- If hour is less then 0 and min is more then 0
IF (@TotalHour < 0 AND @TotalMin > 0)
BEGIN
	SET @TotalMin = @TotalMin - 60
	SET @TotalHour = @TotalHour + 1
END

-- If min is greater or equal to 60 or if min is less or equal to 60
IF (@TotalMin >= 60 OR @TotalMin <= -60)
BEGIN
	-- If min is less then 0 
	IF (@TotalMin < 0)
	BEGIN
		-- Add 60 to min and subtract 1 from our
		SET @TotalMin = @TotalMin + 60
		SET @TotalHour = @TotalHour - 1
	END
	ELSE
	BEGIN
		-- Subtract 60 from min and add 1 to hour 
		SET @TotalMin = @TotalMin - 60
		SET @TotalHour = @TotalHour + 1
	END
END

-- If min is less then 0 multiply min wiht -1 to remove the minus infront
IF (@TotalMin < 0)
BEGIN 
	SET @MinHolder = @TotalMin * -1
END

-- If hour is less then 0 multiply hour wiht -1 to remove the hour infront
IF (@TotalHour < 0)
BEGIN
	SET @HourHolder = @TotalHour * -1
END

-- If length of hour is 1 and is a negatvie number add -0 infront
IF (LEN(@HourHolder) = 1)
BEGIN
	IF (@TotalHour < 0)
	BEGIN 
		SET @HourStr = '-0' + CAST(@HourHolder AS VARCHAR)
	END
END
-- Else set string as hour cast as nvarchar
ELSE
BEGIN
	SET @HourStr = CAST(@TotalHour AS NVARCHAR)
END

-- If length of min is 1 and is a negatvie number add -0 infront
IF (LEN(@MinHolder) = 1)
BEGIN
	SET @MinStr = '0' + @MinHolder
END
-- Else set string as min cast as nvarchar
ELSE
BEGIN
	SET @MinStr = CAST(@MinHolder AS NVARCHAR)
END

-- Step 2

-- While user is less or equal to user_id where startday is equal to today
WHILE (@Users <= (SELECT DISTINCT MIN([User_ID]) FROM [Days] WHERE [StartDay] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd')))))
	BEGIN
		-- If abscense id from days where startday is equal to today
		IF ((SELECT [AbscenseID] FROM [Days] WHERE [StartDay] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd')))) = 1)
		BEGIN
			-- Update today with new total saldo where startday is equal to today
			UPDATE [Days]
			SET TotalSaldo = @HourStr + ':' + @MinStr,
			TotalHour = @TotalHour,
			TotalMin = @TotalMin,
			AbscenseID = 10
			WHERE [StartDay] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd')))
		END
		SET @Users = @Users + 1
	END
GO

DECLARE @SaldooHour INT, @SaldoMin INT, @Saldo NVARCHAR(10)

SET @SaldooHour = -7
SET @SaldoMin = -24

SET @Saldo = '-0' + CAST((@SaldooHour * -1) AS NVARCHAR) + ':' + CAST((@SaldoMin * -1) AS NVARCHAR)

UPDATE [Days]
SET [Saldo] = @Saldo,
[Hour] = @SaldooHour,
[Min] = @SaldoMin
WHERE [StartDay] = (SELECT CONVERT(DATETIME, FORMAT(getdate(), 'yyyy-MM-dd')))
GO
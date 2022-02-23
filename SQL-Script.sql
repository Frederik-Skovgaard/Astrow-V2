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
[FirstName] NVARCHAR(30) NOT NULL,
[MiddleName] NVARCHAR(30),
[LastName] NVARCHAR(30) NOT NULL,
[FullName] NVARCHAR(90) NOT NULL

PRIMARY KEY ([Name_ID])
)
GO


-- User
CREATE TABLE [User] (
[User_ID] INT IDENTITY(1,1) NOT NULL,
[UserName] NVARCHAR(50) NOT NULL,
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

-- Days

INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220201 00:00:00 AM', 1, '', '220201 08:00:00 AM', '220130 15:24:00 PM',0 ,0, '00:00', 0, 0, '00:00')
GO

INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220202 00:00:00 AM', 1, '', '220202 08:00:00 AM', '220202 15:00:00 PM', -24, 0, '-00:24', -24, 0, '-00:24')
GO


INSERT INTO [Days] ([User_ID], [Date], [AbscenseID], [AbscenceText], [StartDay], [EndDay], [Min], [Hour], [Saldo], [TotalMin], [TotalHour], [TotalSaldo])
VALUES (1, '220203 00:00:00 AM', 1, '', '220201 08:00:00 AM', '220130 16:24:00 PM',0 ,1, '00:00', 36, 0, '00:36')
GO


---------------------------- Create Procedures ----------------------------
 
-- Create User
CREATE PROCEDURE [CreateUser]
@id INT,
@UserName NVARCHAR(50),
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
@firstName NVARCHAR(30),
@middleName NVARCHAR(30),
@lastName NVARCHAR(30),
@fullName NVARCHAR(90)
AS
INSERT INTO [Name] ([FirstName], [MiddleName], [LastName], [FullName])
VALUES (@firstName, @middleName, @lastName, @firstName + ' ' + @middleName + ' ' + @lastName) 
GO



---------------------------- Update ----------------------------

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
@UserName NVARCHAR(50),
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
@firstNavn NVARCHAR(30),
@middleNavn NVARCHAR(30),
@lastNavn NVARCHAR(30)
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
[AbscenceText] = @AbscenceText
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
@UserName NVARCHAR(50)
AS
SELECT [salt], [User_ID] FROM [User]
WHERE [UserName] = @UserName
GO

-- Get User 
CREATE PROCEDURE [GetUser]
@userName NVARCHAR(50),
@password NVARCHAR(MAX)
AS
SELECT * FROM [User]
WHERE [UserName] = @userName AND [Password] = @password
GO

-- Get user info
CREATE PROCEDURE [GetUserInfo]
@fullName NVARCHAR(90)
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




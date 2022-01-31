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

-- Days
CREATE TABLE [Days] (
[Days_ID] INT IDENTITY(1,1) NOT NULL,
[User_ID] INT NOT NULL,
[Date] DATETIME NOT NULL,
[AbsenceDate] DATETIME,
[AbscenceText] NVARCHAR(100),
[StartDay] DATETIME,
[EndDay] DATETIME,
[Saldo] NVARCHAR(5),
[TotalSaldo] NVARCHAR(5)

PRIMARY KEY([Days_ID]),
FOREIGN KEY ([User_ID]) REFERENCES [User]([User_ID])
)
GO



---------------------------- Create Admin user ----------------------------
INSERT INTO [Name] ([FirstName], [MiddleName], [LastName], [FullName])
VALUES ('Adam', 'The', 'Admin', 'Adam The Admin')
GO

INSERT INTO [User] ([UserName], [Password], [Name_ID], [Status], [StartDate], [EndDate], [Salt], [IsDeleted])
VALUES ('Admin', 'SPagn++JbYrbIp2/QqKNVA1UU3C572UZ1iNp9lEjMC8=', 1, 'Instructør', '200720 00:00:00 AM', '250101 00:00:00 AM','Roh+S6zwQ9vnp8Soaxw+tA==', 0)
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
@startDay DATETIME,
@endDay DATETIME,
@saldo NVARCHAR(5),
@totalSaldo NVARCHAR(5)
AS
INSERT INTO [Days] ([User_ID], [Date], [StartDay], [EndDay], [Saldo], [TotalSaldo])
VALUES (@id, @date, @startDay, @endDay, @saldo, @totalSaldo) 
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
@AbscenceDate DATETIME,
@AbscenceText NVARCHAR(100)
AS
UPDATE [Days]
SET [AbsenceDate] = @AbscenceDate,
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


-- Update Saldo
CREATE PROCEDURE [UpdateSaldo]
@id INT,
@Saldo NVARCHAR(5)
AS
UPDATE [Days]
SET [Saldo] = @Saldo
WHERE [Days_ID] = @id
GO


-- Update TotalSaldo
CREATE PROCEDURE [UpdateTotalSaldo]
@id INT,
@Saldo NVARCHAR(5)
AS
UPDATE [Days]
SET [TotalSaldo] = @Saldo
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
SELECT * FROM [Name]
WHERE [FullName] = @fullName
GO

-- Find all days by id and date
CREATE PROCEDURE [FindAllDays]
@id INT,
@date DATETIME
AS
SELECT [Days_ID], [User_ID], [Date], [StartDay], [EndDay], [Saldo] FROM Days
WHERE [User_ID] = @id AND [Date] = @date
GO

-- Find all days by id
CREATE PROCEDURE [FindAllDaysByID]
@id INT
AS
SELECT [Days_ID], [User_ID], [Date], [StartDay], [EndDay], [Saldo] FROM Days
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
SELECT [Date], [StartDay] FROM Days
WHERE [StartDay] = @Date AND [User_ID] = @id
GO





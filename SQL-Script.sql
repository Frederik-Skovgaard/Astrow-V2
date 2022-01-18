-- Use Master
USE master
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

-------------- Create Table --------------

--USE [Astrow-2.0]
--GO

--DROP TABLE [InBox]
--GO

--DROP TABLE [TimeCard]
--GO

--DROP TABLE [Files]
--GO

--DROP TABLE [Message]
--GO

--DROP TABLE [Days]
--GO

--DROP TABLE [File]
--GO

--DROP TABLE [User]
--GO

--DROP TABLE [Name]
--GO



CREATE TABLE [Name] (
[Name_ID] INT IDENTITY(1,1) NOT NULL,
[FirstName] NVARCHAR(30) NOT NULL,
[MiddleName] NVARCHAR(30),
[LastName] NVARCHAR(30) NOT NULL,
[FullName] NVARCHAR(90) NOT NULL

PRIMARY KEY (Name_ID)
)
GO

CREATE TABLE [Message] (
[Message_ID] INT IDENTITY(1,1) NOT NULL,
[Message] NVARCHAR(MAX) NOT NULL,
[Sender] NVARCHAR(90) NOT NULL,
[Date] DATETIME NOT NULL

PRIMARY KEY(Message_ID)
)
GO

CREATE TABLE [File] (
[File_ID] INT IDENTITY(1,1) NOT NULL,
[Name] NVARCHAR(100) NOT NULL,
[Type] NVARCHAR(25)NOT NULL,
[Details] NVARCHAR(75) NOT NULL,
[Description] NVARCHAR(75) NOT NULL,
[Date] DATETIME NOT NULL,
[SensitiveData] BIT NOT NULL

PRIMARY KEY(File_ID)
)
GO

CREATE TABLE [User] (
[User_ID] INT IDENTITY(1,1) NOT NULL,
[UserName] NVARCHAR(50) NOT NULL,
[Password] NVARCHAR(MAX) NOT NULL,
[Name_ID] INT NOT NULL,
[Status] NVARCHAR(25) NOT NULL,
[IsDeleted] BIT NOT NULL,
[Salt] NVARCHAR(max) NOT NULL,
[StartDate] DATETIME NOT NULL,
[EndDate] DATETIME NOT NULL

PRIMARY KEY(User_ID)

FOREIGN KEY (Name_ID) REFERENCES [Name](Name_ID),

)
GO


CREATE TABLE [Days] (
[Days_ID] INT IDENTITY(1,1) NOT NULL,
[User_ID] INT NOT NULL,
[Date] DATETIME NOT NULL,
[AbsenceDate] DATETIME,
[AbscenceText] NVARCHAR(100),
[StartDay] DATETIME,
[EndDay] DATETIME,
[Saldo] DATETIME


PRIMARY KEY(Days_ID)

FOREIGN KEY (User_ID) REFERENCES [User](User_ID)
)
GO




CREATE TABLE [Files] (
[Files_ID] INT IDENTITY(1,1) NOT NULL,
[File_ID] INT NOT NULL,
[User_ID] INT NOT NULL

PRIMARY KEY(Files_ID)

FOREIGN KEY (File_ID) REFERENCES [File](File_ID),
FOREIGN KEY (User_ID) REFERENCES [User](User_ID)
)
GO

CREATE TABLE [TimeCard] (
[TimeCard_ID] INT IDENTITY(1,1) NOT NULL,
[Days_ID] INT NOT NULL,
[User_ID] INT NOT NULL

PRIMARY KEY(TimeCard_ID)

FOREIGN KEY (Days_ID) REFERENCES [Days](Days_ID),
FOREIGN KEY (User_ID) REFERENCES [User](User_ID)
)
GO


CREATE TABLE [InBox] (
[Inbox_ID] INT IDENTITY(1,1) NOT NULL,
[Message_ID] INT NOT NULL,
[User_ID] INT NOT NULL

PRIMARY KEY(Inbox_ID)

FOREIGN KEY (Message_ID) REFERENCES [Message](Message_ID),
FOREIGN KEY (User_ID) REFERENCES [User](User_ID)
)
GO




---------------------------------- Stored Procedure ----------------------------------



-------------- Create-Container --------------


-- Create User
CREATE PROCEDURE [CreateUser]
@UserName NVARCHAR(50),
@Password NVARCHAR(MAX),
@id INT,
@Status NVARCHAR(25),
@salt NVARCHAR(max),
@startDate DATETIME,
@endDate DATETIME
AS
INSERT INTO [User] (UserName, Password, Name_ID, Status, IsDeleted, Salt, StartDate, EndDate)
VALUES (@UserName, @Password, @id, @Status, 0, @salt, @startDate, @endDate)
GO

-- Create Inbox
CREATE PROCEDURE [CreateInBox]
@messageID INT,
@userID INT
AS
INSERT INTO [InBox] (Message_ID, User_ID)
VALUES(@messageID, @userID)
GO

-- Create Timecard
CREATE PROCEDURE [CreateTimeCard]
@days INT,
@userID INT
AS
INSERT INTO [TimeCard] (Days_ID, User_ID)
VALUES (@days, @userID)
GO

-- Create Files
CREATE PROCEDURE [CreateFiles]
@file_ID INT,
@userID INT
AS
INSERT INTO [Files] (File_ID, User_ID)
VALUES (@file_ID, @userID)
GO


---------------------------- Create-Items ----------------------------


-- Create File
CREATE PROCEDURE [CreateFile]
@Name NVARCHAR(90),
@Type NVARCHAR(25),
@Details NVARCHAR(75),
@Description NVARCHAR(75),
@Date DATETIME,
@SensitiveDate BIT
AS
INSERT INTO [File] (Name, Type, Details, Description, Date, SensitiveData)
VALUES (@Name, @Type, @Details, @Description, @Date, @SensitiveDate)
GO


-- Create Days
CREATE PROCEDURE [CreateDay]
@date DATETIME,
@userID INT,
@abscenceDate DATETIME,
@abscenceText NVARCHAR(100),
@startDay DATETIME,
@endDay DATETIME,
@saldo DATETIME
AS
INSERT INTO [Days] (Date, User_ID, AbsenceDate, AbscenceText, StartDay, EndDay, Saldo)
VALUES (@date, @userID, @abscenceDate, @abscenceText, @startDay, @endDay, @saldo)
GO

-- Create Message
CREATE PROCEDURE [CreateMessage]
@message NVARCHAR(MAX),
@sender NVARCHAR(90)
AS
INSERT INTO [Message] (Message, Sender, Date)
VALUES (@message, @sender, CURRENT_TIMESTAMP)
GO


-- Create Name
CREATE PROCEDURE [CreateName]
@firstName NVARCHAR(30),
@middleName NVARCHAR(30),
@lastName NVARCHAR(30),
@fullName NVARCHAR(90)
AS
INSERT INTO [Name] (FirstName, MiddleName, LastName, FullName)
VALUES (@firstName, @middleName, @lastName, @firstName + ' ' + @middleName + ' ' + @lastName)
GO


-------------- Update-Data --------------

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
SET UserName = @UserName,
Password = @Password,
Status = @Status,
StartDate = @startDate,
EndDate = @endDate
WHERE User_ID = @id
GO


-- Update User info
CREATE PROCEDURE [UpdateUserInfo]
@id INT,
@firstNavn NVARCHAR(30),
@middleNavn NVARCHAR(30),
@lastNavn NVARCHAR(30)
AS
UPDATE [Name]
SET FirstName = @firstNavn,
MiddleName = @middleNavn,
LastName = @lastNavn
WHERE Name_ID = @id
GO

-- Update Abscence
CREATE PROCEDURE [UpdateAbscence]
@id INT,
@AbscenceDate DATETIME,
@AbscenceText NVARCHAR(100)
AS
UPDATE [Days]
SET AbsenceDate = @AbscenceDate,
AbscenceText = @AbscenceText
WHERE Days_ID = @id
GO

-- Update Start day
CREATE PROCEDURE [UpdateStart]
@id INT,
@StartDay DATETIME
AS
UPDATE [Days]
SET StartDay = @StartDay
WHERE Days_ID = @id
GO

-- Update End day
CREATE PROCEDURE [UpdateEnd]
@id INT,
@EndDay DATETIME
AS
UPDATE [Days]
SET EndDay = @EndDay
WHERE Days_ID = @id
GO

-- Update Saldo
CREATE PROCEDURE [UpdateSaldo]
@id INT,
@Saldo DATETIME
AS
UPDATE [Days]
SET Saldo = @Saldo
WHERE Days_ID = @id
GO

-------------- Delete-Data --------------


-- Delete
CREATE PROCEDURE [DeleteUser]
@id INT
AS
UPDATE [User]
SET IsDeleted = 1
WHERE User_ID = @id
GO



-------------- Fetch-Data --------------



-- Read All
CREATE PROCEDURE [ReadAllUsers]
AS
SELECT * FROM [User]
WHERE IsDeleted = 0
GO



-- Get by ID
CREATE PROCEDURE [GetByID]
@id INT
AS
SELECT * FROM [User]
WHERE User_ID = @id
GO

-- Get by UserName
CREATE PROCEDURE [GetByUserName]
@UserName NVARCHAR(50)
AS
SELECT salt FROM [User]
WHERE UserName = @UserName
GO


CREATE PROCEDURE [GetUser]
@userName NVARCHAR(50),
@password NVARCHAR(MAX)
AS
SELECT * FROM [User]
WHERE UserName = @userName AND Password = @password
GO

CREATE PROCEDURE [GetUserInfo]
@fullName NVARCHAR(90)
AS
SELECT * FROM [Name]
WHERE FullName = @fullName
GO


CREATE PROCEDURE [FindAllDays]
@id INT
AS
SELECT * FROM Days
WHERE User_ID = @id
GO

CREATE PROCEDURE [FindUserInfo]
@id INT
AS
SELECT * FROM Name
WHERE Name_ID = @id
GO

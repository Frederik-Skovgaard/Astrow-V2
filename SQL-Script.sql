-- Use Master
USE master
GO


--Drop Database
ALTER DATABASE [Astrow-2.0] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE IF EXISTS [Astrow-2.0]
GO

-- Create Database
CREATE DATABASE [Astrow-2.0]
GO

-- Use Astrow-2.0
USE [Astrow-2.0]
GO

-------------- Create Table --------------

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

CREATE TABLE [InBox] (
[Inbox_ID] INT IDENTITY(1,1) NOT NULL,
[Message_ID] INT


PRIMARY KEY(Inbox_ID)

FOREIGN KEY (Message_ID) REFERENCES [Message](Message_ID)
)
GO


CREATE TABLE [Days] (
[Days_ID] INT IDENTITY(1,1) NOT NULL,
[Date] DATETIME NOT NULL,
[Absence] DATETIME,
[Registry] DATETIME,
[Saldo] DATETIME NOT NULL,
[Flex] DATETIME NOT NULL

PRIMARY KEY(Days_ID)
)
GO

CREATE TABLE [TimeCard] (
[TimeCard_ID] INT IDENTITY(1,1) NOT NULL,
[Days_ID] INT

PRIMARY KEY(TimeCard_ID)

FOREIGN KEY (Days_ID) REFERENCES [Days](Days_ID)
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

CREATE TABLE [Files] (
[Files_ID] INT IDENTITY(1,1) NOT NULL,
[File_ID] INT

PRIMARY KEY(Files_ID)

FOREIGN KEY (File_ID) REFERENCES [File](File_ID)
)
GO


CREATE TABLE [User] (
[User_ID] INT IDENTITY(1,1) NOT NULL,
[UserName] NVARCHAR(50) NOT NULL,
[Password] TINYINT NOT NULL,
[Name_ID] INT NOT NULL,
[Inbox_ID] INT NOT NULL,
[TimeCard_ID] INT NOT NULL,
[Files_ID] INT NOT NULL,
[Status] NVARCHAR(25) NOT NULL,
[IsDeleted] BIT NOT NULL,
[Salt] NVARCHAR(6) NOT NULL

PRIMARY KEY(User_ID)

FOREIGN KEY (Name_ID) REFERENCES [Name](Name_ID),
FOREIGN KEY (Inbox_ID) REFERENCES [InBox](Inbox_ID),
FOREIGN KEY (TimeCard_ID) REFERENCES [TimeCard](TimeCard_ID),
FOREIGN KEY (Files_ID) REFERENCES [Files](Files_ID)
)
GO

----------------- Stored Procedure -----------------


-- Create User
CREATE PROCEDURE [CreateUser]
@UserName NVARCHAR(50),
@Password TINYINT,
@id INT,
@Status NVARCHAR(25),
@salt NVARCHAR(6)
AS
INSERT INTO [User] (UserName, Password, Status, IsDeleted, Salt)
VALUES (@UserName, @Password, @Status, 0, @salt)
GO


-- Create Inbox
CREATE PROCEDURE [CreateInBox]
@messageID INT
AS
INSERT INTO [InBox] ([Message_ID])
VALUES (@messageID)
GO

-- Create Timecard
CREATE PROCEDURE [CreateTimeCard]
@days INT
AS
INSERT INTO [TimeCard] (Days_ID)
VALUES (@days)
GO

-- Create Files
CREATE PROCEDURE [CreateFiles]
@file_ID INT
AS
INSERT INTO [Files] (Files_ID)
VALUES (@file_ID)
GO




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
@absence DATETIME,
@registry DATETIME,
@saldo DATETIME,
@flex DATETIME
AS
INSERT INTO [Days] (Date, Absence, Registry, Saldo, Flex)
VALUES (@date, @absence, @registry, @saldo, @flex)
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
VALUES (@firstName, @middleName, @lastName, @firstName + ' ' + @lastName)
GO



-- Update
CREATE PROCEDURE [UpdateUser]
@id INT,
@UserName NVARCHAR(50),
@Password TINYINT,
@Status NVARCHAR(25)
AS
UPDATE [User]
SET UserName = @UserName,
Password = @Password,
Status = @Status
WHERE User_ID = @id
GO

-- Update user
CREATE PROCEDURE [UpdateForgienkeys]
@id INT
AS
UPDATE [User]
SET Name_ID = @id,
Inbox_ID = @id,
TimeCard_ID = @id,
Files_ID = @id
WHERE User_ID = @id
GO

-- Delete
CREATE PROCEDURE [DeleteUser]
@id INT
AS
UPDATE [User]
SET IsDeleted = 1
WHERE User_ID = @id
GO

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




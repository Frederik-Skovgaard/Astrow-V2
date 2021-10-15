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
[MiddleName] NVARCHAR(30) NOT NULL,
[LastName] NVARCHAR(30) NOT NULL,
[FullName] NVARCHAR(90) NOT NULL

PRIMARY KEY (Name_ID)
)
GO

CREATE TABLE [InBox] (
[Inbox_ID] INT IDENTITY(1,1) NOT NULL,
[Message] NVARCHAR(MAX) NOT NULL,
[Sender] NVARCHAR(90) NOT NULL,
[Date] DATETIME NOT NULL

PRIMARY KEY(Inbox_ID)
)
GO


CREATE TABLE [TimeCard] (
[TimeCard_ID] INT IDENTITY(1,1) NOT NULL,
[Date] DATETIME NOT NULL,
[Absence] DATETIME NOT NULL,
[Registry] DATETIME NOT NULL,
[Saldo] DATETIME NOT NULL,
[Flex] DATETIME NOT NULL

PRIMARY KEY(TimeCard_ID)
)
GO

CREATE TABLE [Files] (
[Files_ID]INT IDENTITY(1,1) NOT NULL,
[Name] NVARCHAR(100) NOT NULL,
[Type] NVARCHAR(25)NOT NULL,
[Details] NVARCHAR(75) NOT NULL,
[Description] NVARCHAR(75) NOT NULL,
[Date] DATETIME NOT NULL,
[SensitiveData] BIT NOT NULL

PRIMARY KEY(Files_ID)
)
GO


CREATE TABLE [User] (
[User_ID] INT IDENTITY(1,1) NOT NULL,
[UserName] NVARCHAR(50) NOT NULL,
[Password] NVARCHAR(MAX) NOT NULL,
[Name_ID] INT NOT NULL,
[Inbox_ID] INT NOT NULL,
[TimeCard_ID] INT NOT NULL,
[Files_ID] INT NOT NULL,
[Status] NVARCHAR(25) NOT NULL,
[IsDeleted] BIT NOT NULL

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
@Password NVARCHAR(MAX),
@Status NVARCHAR(25)
AS
INSERT INTO [User] (UserName, Password, Status, IsDeleted)
VALUES (@UserName, @Password, @Status, 0)
GO

-- Update
CREATE PROCEDURE [UpdateUser]
@id INT,
@UserName NVARCHAR(50),
@Password NVARCHAR(MAX),
@Status NVARCHAR(25)
AS
UPDATE [User]
SET UserName = @UserName,
Password = @Password,
Status = @Status
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




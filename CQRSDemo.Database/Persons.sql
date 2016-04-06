CREATE TABLE [dbo].[Persons]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FirstName] NVARCHAR(50),
	[LastName] NVARCHAR(50),
	[EMailAddress] NVARCHAR(100), 
    [AddressId] INT NULL
)

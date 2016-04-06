CREATE TABLE [dbo].[Addresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[PersonId] INT NOT NULL,
	[Street] NVARCHAR(50),
	[City] NVARCHAR(50),
	[PhoneNumber] NVARCHAR(50),
	[CountryId] INT FOREIGN KEY REFERENCES Countries(id),
	[ValidFrom] Date
)

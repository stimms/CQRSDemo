CREATE TABLE [dbo].[Addresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Street] NVARCHAR(50),
	[City] NVARCHAR(50),
	[PhoneNumber] NVARCHAR(50),
	[CountryId] INT FOREIGN KEY REFERENCES Countries(id),
	[ValidFrom] Date
)

CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Password] NVARCHAR(50) NOT NULL, 
	[AccountExpiry] Date NOT NULL,
	[PersonId] INT FOREIGN KEY REFERENCES persons(id)
)

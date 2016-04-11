CREATE TABLE [dbo].[IndexUserList]
(
	[UserId] int,
	[FirstName] NVARCHAR(50),
	[LastName] NVARCHAR(50),
	[AccountExpiry] Date NOT NULL,
	[Street] NVARCHAR(50),
	[City] NVARCHAR(50),
	[Name] NVARCHAR(50)
)

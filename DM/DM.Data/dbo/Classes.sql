CREATE TABLE [dbo].[Classes]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [HitDie] INT NOT NULL, 
    [Source] VARCHAR(50) NOT NULL 
)

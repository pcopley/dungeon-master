CREATE TABLE [dbo].[Proficiencies]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [Type] VARCHAR(50) NOT NULL
)

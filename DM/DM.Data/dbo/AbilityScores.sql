CREATE TABLE [dbo].[AbilityScores]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(3) NOT NULL, 
    [Name] VARCHAR(3) NOT NULL, 
    [FullName] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL
)

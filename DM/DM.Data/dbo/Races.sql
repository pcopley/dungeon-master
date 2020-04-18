CREATE TABLE [dbo].[Races]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(50) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [Speed] INT NOT NULL DEFAULT 30, 
    [Alignment] VARCHAR(MAX) NOT NULL, 
    [Age] VARCHAR(MAX) NOT NULL, 
    [Size] VARCHAR(50) NOT NULL, 
    [SizeDescription] VARCHAR(MAX) NOT NULL, 
    [LanguageDesc] VARCHAR(MAX) NOT NULL 
)

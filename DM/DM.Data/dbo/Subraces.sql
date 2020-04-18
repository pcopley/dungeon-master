CREATE TABLE [dbo].[Subraces]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(50) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [RaceId] VARCHAR(24) NULL, 
    CONSTRAINT [FK_Subraces_Races] FOREIGN KEY ([RaceId]) REFERENCES [Races]([Id])  
)

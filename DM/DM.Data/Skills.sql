CREATE TABLE [dbo].[Skills]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(50) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [AbilityScoreId] VARCHAR(24) NOT NULL, 
    CONSTRAINT [FK_Skills_AbilityScore] FOREIGN KEY ([AbilityScoreId]) REFERENCES [AbilityScores]([Id])
)

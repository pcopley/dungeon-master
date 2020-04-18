CREATE TABLE [dbo].[Monsters]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Type] VARCHAR(50) NOT NULL, 
    [Subtype] VARCHAR(50) NOT NULL, 
    [Alignment] VARCHAR(50) NOT NULL, 
    [ArmorClass] INT NOT NULL DEFAULT 0, 
    [HitPoints] INT NOT NULL DEFAULT 0, 
    [HitDice] VARCHAR(24) NOT NULL, 
    [Languages] VARCHAR(255) NOT NULL, 
    [ChallengeRating] DECIMAL NULL, 
    [Source] VARCHAR(50) NOT NULL 
)

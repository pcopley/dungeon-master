CREATE TABLE [dbo].[RacialBonuses]
(
	[RaceId] VARCHAR(24) NOT NULL , 
    [AbilityScoreId] VARCHAR(24) NOT NULL, 
    [Bonus] INT NOT NULL, 
    PRIMARY KEY ([RaceId], [AbilityScoreId]), 
    CONSTRAINT [FK_RacialBonuses_Races] FOREIGN KEY ([RaceId]) REFERENCES [Races]([Id]),
    CONSTRAINT [FK_RacialBonuses_AbilityScores] FOREIGN KEY ([AbilityScoreId]) REFERENCES [AbilityScores]([Id])
)

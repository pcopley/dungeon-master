CREATE TABLE [dbo].[RacialTraits]
(
	[RaceId] VARCHAR(24) NOT NULL , 
    [TraitId] VARCHAR(24) NOT NULL 
    PRIMARY KEY ([RaceId], [TraitId]), 
    CONSTRAINT [FK_RacialTraits_Races] FOREIGN KEY ([RaceId]) REFERENCES [Races]([Id]),
    CONSTRAINT [FK_RacialTraits_Traits] FOREIGN KEY ([TraitId]) REFERENCES [Traits]([Id])
)

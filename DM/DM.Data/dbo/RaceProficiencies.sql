CREATE TABLE [dbo].[RaceProficiencies]
(
    [ID] UNIQUEIDENTIFIER NOT NULL,
    [RaceId] VARCHAR(24) NULL, 
    [SubraceId] VARCHAR(24) NULL, 
    [ProficiencyId] VARCHAR(24) NULL, 
    CONSTRAINT [PK_RaceProficiencies] PRIMARY KEY ([ID]), 
    CONSTRAINT [FK_RaceProficiencies_Races] FOREIGN KEY ([RaceId]) REFERENCES [Races]([ID]),
    CONSTRAINT [FK_RaceProficiencies_Subraces] FOREIGN KEY ([SubraceId]) REFERENCES [Subraces]([ID]),
    CONSTRAINT [FK_RaceProficiencies_Proficiencies] FOREIGN KEY ([ProficiencyId]) REFERENCES [Proficiencies]([ID]),
)

CREATE TABLE [dbo].[ClassSavingThrows]
(
    [ClassId] VARCHAR(24) NOT NULL, 
    [AbilityScoreId] VARCHAR(24) NOT NULL, 
    CONSTRAINT [PK_ClassSavingThrows] PRIMARY KEY ([ClassId], [AbilityScoreId]), 
    CONSTRAINT [FK_ClassSavingThrows_Classes] FOREIGN KEY ([ClassId]) REFERENCES [Classes]([Id]),
    CONSTRAINT [FK_ClassSavingThrows_AbilityScores] FOREIGN KEY ([AbilityScoreId]) REFERENCES [AbilityScores]([Id])
)

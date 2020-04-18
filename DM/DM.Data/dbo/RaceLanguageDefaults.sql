CREATE TABLE [dbo].[RaceLanguageDefaults]
(
	[RaceId] VARCHAR(24) NOT NULL , 
    [LanguageId] VARCHAR(24) NOT NULL 
    PRIMARY KEY ([RaceId], [LanguageId]), 
    CONSTRAINT [FK_RaceLanguageDefaults_Races] FOREIGN KEY ([RaceId]) REFERENCES [Races]([Id]),
    CONSTRAINT [FK_RaceLanguageDefaults_Languages] FOREIGN KEY ([LanguageId]) REFERENCES [Languages]([Id])
)

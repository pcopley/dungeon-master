CREATE TABLE [dbo].[Languages]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(50) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [TypicalSpeakers] VARCHAR(MAX) NOT NULL, 
    [Type] VARCHAR(50) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [Script] VARCHAR(50) NOT NULL
)

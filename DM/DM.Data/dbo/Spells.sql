CREATE TABLE [dbo].[Spells]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [HigherLevel] VARCHAR(MAX) NULL,
    [Component_Verbal] BIT NOT NULL DEFAULT 0,
    [Component_Somatic] BIT NOT NULL DEFAULT 0,
    [Component_Material] BIT NOT NULL DEFAULT 0, 
    [Concentration] BIT NOT NULL DEFAULT 0, 
    [Level] INT NOT NULL, 
    [Ritual] BIT NOT NULL DEFAULT 0, 
    [Material] VARCHAR(MAX) NULL, 
    [CastingTime] VARCHAR(50) NOT NULL, 
    [SchoolId] VARCHAR(24) NOT NULL, 
    [Source] VARCHAR(50) NOT NULL, 
    [Duration] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Spells_MagicSchools] FOREIGN KEY ([SchoolId]) REFERENCES [MagicSchools]([Id])
)

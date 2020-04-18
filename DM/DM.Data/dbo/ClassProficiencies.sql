CREATE TABLE [dbo].[ClassProficiencies]
(
    [ID] UNIQUEIDENTIFIER NOT NULL,
    [ClassId] VARCHAR(24) NULL, 
    [ProficiencyId] VARCHAR(24) NULL, 
    CONSTRAINT [PK_ClassProficiencies] PRIMARY KEY ([ID]), 
    CONSTRAINT [FK_ClassProficiencies_Classes] FOREIGN KEY ([ClassId]) REFERENCES [Classes]([ID]),
    CONSTRAINT [FK_ClassProficiencies_Proficiencies] FOREIGN KEY ([ProficiencyId]) REFERENCES [Proficiencies]([ID])
)

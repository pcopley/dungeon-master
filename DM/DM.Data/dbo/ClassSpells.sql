CREATE TABLE [dbo].[ClassSpells]
(
    [ClassId] VARCHAR(24) NOT NULL, 
    [SpellId] VARCHAR(24) NOT NULL, 
    CONSTRAINT [PK_ClassSpells] PRIMARY KEY ([ClassId], [SpellId]), 
    CONSTRAINT [FK_ClassSpells_Classes] FOREIGN KEY ([ClassId]) REFERENCES [Classes]([ID]),
    CONSTRAINT [FK_ClassSpells_Spells] FOREIGN KEY ([SpellId]) REFERENCES [Spells]([ID])
)

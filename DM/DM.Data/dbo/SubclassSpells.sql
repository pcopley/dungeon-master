CREATE TABLE [dbo].[SubclassSpells]
(
    [SubclassId] VARCHAR(24) NOT NULL, 
    [SpellId] VARCHAR(24) NOT NULL, 
    CONSTRAINT [PK_SubclassSpells] PRIMARY KEY ([SubclassId], [SpellId]), 
    CONSTRAINT [FK_SubclassSpells_Subclasses] FOREIGN KEY ([SubclassId]) REFERENCES [Subclasses]([ID]),
    CONSTRAINT [FK_SubclassSpells_Spells] FOREIGN KEY ([SpellId]) REFERENCES [Spells]([ID])
)

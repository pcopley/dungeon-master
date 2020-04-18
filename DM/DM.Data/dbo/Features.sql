CREATE TABLE [dbo].[Features]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [Level] INT NOT NULL DEFAULT 0,
    [Source] VARCHAR(255) NOT NULL, 
    [ClassId] VARCHAR(24) NULL, 
    [SubclassId] VARCHAR(24) NULL, 
    CONSTRAINT [FK_Features_Class] FOREIGN KEY ([ClassId]) REFERENCES [Classes]([Id]),
    CONSTRAINT [FK_Features_Subclass] FOREIGN KEY ([SubclassId]) REFERENCES [Subclasses]([Id])
)

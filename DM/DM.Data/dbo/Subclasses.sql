CREATE TABLE [dbo].[Subclasses]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [SubclassFlavor] VARCHAR(50) NOT NULL ,
    [Source] VARCHAR(255) NOT NULL 
)

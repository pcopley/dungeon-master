CREATE TABLE [dbo].[Equipment]
(
	[Id] VARCHAR(24) NOT NULL PRIMARY KEY, 
    [Index] VARCHAR(255) NOT NULL, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [EquipmentCategory] VARCHAR(50) NULL,
    [GearCategory] VARCHAR(50) NULL,
    [Cost] VARCHAR(25) NULL,
    [Source] VARCHAR(50) NOT NULL, 
    [Weight] DECIMAL NOT NULL DEFAULT 0.0
)

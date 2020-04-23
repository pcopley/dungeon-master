CREATE TABLE [dbo].[MonsterAbilities]
(
	[Id] uniqueidentifier NOT NULL PRIMARY KEY,
	[MonsterId] VARCHAR(24) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(50) NOT NULL, 
    [DC_AbilityScoreId] VARCHAR(24) NULL, 
    [DC_Value] INT NULL, 
    [DC_SuccessType] VARCHAR(255) NULL
)

CREATE TABLE [dbo].[MonsterActions]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NOT NULL, 
    [UsageLimit] INT NULL, 
    [UsageTimeframe] VARCHAR(50) NULL, 
    [DC_AbilityScoreId] VARCHAR(24) NULL, 
    [DC_Value] INT NULL, 
    [DC_SuccessType] VARCHAR(50) NULL, 
    [IsLegendary] BIT NOT NULL DEFAULT 0, 
    [AttackBonus] INT NULL
)

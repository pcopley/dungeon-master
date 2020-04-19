using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DM.Data.Utilities.DataLoadUtility.RemoteData;
using DM.Data.Utilities.DataLoadUtility.RemoteData.POCO;

// todo replace console logging with a real logger
namespace DM.Data.Utilities.DataLoadUtility
{
    public class Program
    {
        private const string ConnectionString =
            @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DM;Trusted_Connection=True;";

        private static readonly ConsoleColor ForegroundColor = Console.ForegroundColor;

        private static ApiHelper _apiHelper;

        public static async Task Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            _apiHelper = new ApiHelper();

            //await TruncateAllTheThings();

            //// Types with no dependencies
            //await LoadTypesWithoutDependencies();

            //// Types with dependencies, but loading the data first
            //await LoadSkills();
            //await LoadSpells();
            //await LoadFeatures();
            //await LoadTraits();
            //await LoadClasses();
            //await LoadSubclasses();
            //await LoadRaces();
            //await LoadSubraces();
            //await LoadMonsters();

            //// Load the foreign keys for the above types
            //await LoadFeatureRelationships();
            //await LoadProficiencyRelationships();
            //await LoadRaceRelationships();
            //await LoadSubraceRelationships();
            //await LoadSpellRelationships();
            await LoadClassRelationships();

            ////// Since we loaded data with no FKs, tighten up
            ////// the referential integrity constraints
            ////await EnforceForeignKeys();

            sw.Stop();
            Console.WriteLine($"Loaded in {sw.ElapsedMilliseconds} ms");
        }

        private static async Task LoadClasses()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading classes from API");
            Console.ForegroundColor = ForegroundColor;

            var classes = await _apiHelper.LoadData<Class>("/api/classes");

            if (classes.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var @class in classes)
                {
                    Console.WriteLine($"Loading class {@class.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO Classes " +
                        "([Id], [Index], [Name], [HitDie], [Source]) VALUES " +
                        $"('{@class.ID}', '{@class.Index}', '{@class.Name}', '{@class.HitDie}', '{@class.Source}')");
                }
            }
        }

        private static async Task LoadClassRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading class relationships from API");

            Console.WriteLine("  *  Loading classes");
            var classes = await _apiHelper.LoadData<Class>("/api/classes");

            Console.WriteLine("  *  Loading saving throws");
            var abilityScores = await _apiHelper.LoadData<AbilityScore>("/api/ability-scores");
            Console.ForegroundColor = ForegroundColor;

            if (classes.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var @class in classes)
                {
                    Console.WriteLine($"Updating saving throws for class {@class.Index}");

                    if (@class.SavingThrows != null && @class.SavingThrows.Any())
                    {
                        foreach (var savingThrow in @class.SavingThrows)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO [ClassSavingThrows] ([ClassId], [AbilityScoreId]) VALUES ('{@class.ID}', '{abilityScores.First(x => x.Name == savingThrow.Name).ID}')");
                        }
                    }
                }
            }
        }

        private static async Task LoadFeatureRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading feature relationships from API");

            Console.WriteLine("  *  Loading features");
            var features = await _apiHelper.LoadData<Feature>("/api/features");

            Console.WriteLine("  *  Loading classes");
            var classes = await _apiHelper.LoadData<Class>("/api/classes");

            Console.WriteLine("  *  Loading subclasses");
            var subclasses = await _apiHelper.LoadData<Subclass>("/api/subclasses");
            Console.ForegroundColor = ForegroundColor;

            if (features.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var feature in features)
                {
                    Console.WriteLine($"Updating relationships for feature {feature.Index}");

                    if (feature.Class != null)
                    {
                        await connection.ExecuteAsync($@"UPDATE Features
                                            SET ClassId='{classes.First(x => x.Name == feature.Class.Name).ID}'
                                            WHERE ID='{feature.ID}'");
                    }

                    if (feature.Subclass != null)
                    {
                        await connection.ExecuteAsync($@"UPDATE Features
                                            SET SubclassId='{subclasses.First(x => x.Name == feature.Subclass.Name).ID}'
                                            WHERE ID='{feature.ID}'");
                    }
                }
            }
        }

        private static async Task LoadFeatures()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading features from API");
            Console.ForegroundColor = ForegroundColor;

            var features = await _apiHelper.LoadData<Feature>("/api/features");

            if (features.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var feature in features)
                {
                    Console.WriteLine($"Loading feature {feature.Index}");

                    try
                    {
                        await connection.ExecuteAsync(
                            "INSERT INTO Features " +
                            "([Id], [Index], [Name], [Description], [Source], [Level]) VALUES " +
                            $"('{feature.ID}', '{feature.Index}', '{feature.Name}', '{string.Join(" ", feature.Description)}', '{feature.Source}', '{feature.Level}')");
                    }
                    catch (SqlException e)
                    {
                        if (e.Message.StartsWith("Violation of PRIMARY KEY",
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Duplicate data in API, ignore
                            var color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Skipping duplicate record {feature.Index}");
                            Console.ForegroundColor = color;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private static async Task LoadMonsters()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading monsters from API");
            Console.ForegroundColor = ForegroundColor;

            var monsters = await _apiHelper.LoadData<Monster>("/api/monsters");

            if (monsters.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var monster in monsters)
                {
                    Console.WriteLine($"Loading monster {monster.Index}");

                    var sql = $@"INSERT INTO [dbo].[Monsters] ([Id] ,[Index] ,[Name] ,[Type] ,[Subtype] ,[Alignment] ,
            [ArmorClass] ,[HitPoints] ,[HitDice] ,[Languages] ,[ChallengeRating] ,[Source]) VALUES
           ('{monster.ID}' ,'{monster.Index}' ,'{monster.Name}' ,'{monster.Type}' ,'{monster.Subtype}' ,'{monster.Alignment}' ,
		   {monster.ArmorClass} ,{monster.HitPoints} ,'{monster.HitDice}' ,'{monster.Languages}' ,{monster.ChallengeRating} ,'{monster.Source}')";

                    await connection.ExecuteAsync(sql);
                }
            }
        }

        private static async Task LoadProficiencyRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading proficiency relationships from API");

            Console.WriteLine("  *  Loading proficiencies");
            var proficiencies = await _apiHelper.LoadData<Proficiency>("/api/proficiencies");

            Console.WriteLine("  *  Loading classes and subclasses");
            var classes = await _apiHelper.LoadData<Class>("/api/classes");
            var subclasses = await _apiHelper.LoadData<Subclass>("/api/subclasses");

            Console.WriteLine("  *  Loading races and subraces");
            var races = await _apiHelper.LoadData<Race>("/api/races");
            var subraces = await _apiHelper.LoadData<Subrace>("/api/subraces");
            Console.ForegroundColor = ForegroundColor;

            if (proficiencies.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var proficiency in proficiencies)
                {
                    Console.WriteLine($"Updating relationships for proficiency {proficiency.Index}");

                    if (proficiency.Classes != null && proficiency.Classes.Any())
                    {
                        foreach (var @class in proficiency.Classes)
                        {
                            Console.WriteLine($"Saving proficiency for class {@class.Name}");

                            var id = Guid.NewGuid();

                            await connection.ExecuteAsync(
                                $@"INSERT INTO ClassProficiencies ([ID], [ClassId], [ProficiencyId]) VALUES ('{id}', '{classes.First(x => x.Name == @class.Name).ID}', '{proficiency.ID}')");
                        }
                    }

                    if (proficiency.Races != null && proficiency.Races.Any())
                    {
                        foreach (var race in proficiency.Races)
                        {
                            Console.WriteLine($"Saving proficiency for race {race.Name}");

                            var id = Guid.NewGuid();

                            await connection.ExecuteAsync(
                                $@"INSERT INTO RaceProficiencies ([ID]) VALUES ('{id}')");

                            if (race.URL.Contains("/subraces/"))
                            {
                                var subrace = subraces.First(x => x.Name == race.Name);

                                await connection.ExecuteAsync(
                                    $@"UPDATE RaceProficiencies SET [SubraceId] = '{subrace.ID}', [ProficiencyId] = '{proficiency.ID}' WHERE ID = '{id}'");
                            }
                            else
                            {
                                await connection.ExecuteAsync(
                                    $@"UPDATE RaceProficiencies SET [RaceId] = '{races.First(x => x.Name == race.Name).ID}', [ProficiencyId] = '{proficiency.ID}' WHERE ID = '{id}'");
                            }
                        }
                    }
                }
            }
        }

        private static async Task LoadRaceRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading race relationships from API");

            Console.WriteLine("  *  Loading races");
            var races = await _apiHelper.LoadData<Race>("/api/races");

            Console.WriteLine("  *  Loading ability scores");
            var abilityScores = await _apiHelper.LoadData<AbilityScore>("/api/ability-scores");

            Console.WriteLine("  *  Loading languages");
            var languages = await _apiHelper.LoadData<Language>("/api/languages");

            Console.WriteLine("  *  Loading traits");
            var traits = await _apiHelper.LoadData<Trait>("/api/traits");

            // Bug fix for trait
            traits.First(x => x.Name == "Internal Legacy").Name = "Infernal Legacy";

            Console.ForegroundColor = ForegroundColor;

            if (races.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var race in races)
                {
                    Console.WriteLine($"Updating relationships for race {race.Index}");

                    if (race.AbilityBonuses != null && race.AbilityBonuses.Any())
                    {
                        foreach (var bonus in race.AbilityBonuses)
                        {
                            var ability = abilityScores.First(x => x.Name == bonus.Name);

                            await connection.ExecuteAsync(
                                $@"INSERT INTO RacialBonuses ([RaceId], [AbilityScoreId], [Bonus]) VALUES ('{race.ID}', '{ability.ID}', {bonus.Bonus})");
                        }
                    }

                    if (race.Languages != null && race.Languages.Any())
                    {
                        foreach (var language in race.Languages)
                        {
                            // One-off bugfix for orc v. orcish as language name
                            if (language.Name == "Orcish")
                                language.Name = "Orc";

                            var lang = languages.First(x => x.Name == language.Name);

                            await connection.ExecuteAsync(
                                $@"INSERT INTO RaceLanguageDefaults ([RaceId], [LanguageId]) VALUES ('{race.ID}', '{lang.ID}')");
                        }
                    }

                    if (race.Traits != null && race.Traits.Any())
                    {
                        foreach (var trait in race.Traits)
                        {
                            // One-off bugfix for trait name in half-orc data
                            if (trait.Name == "Restless Endurance")
                                trait.Name = "Relentless Endurance";

                            if (trait.Name == "Internal Legacy")
                                trait.Name = "Infernal Legacy";

                            var t = traits.First(x => x.Name == trait.Name);

                            await connection.ExecuteAsync(
                                $@"INSERT INTO RacialTraits ([RaceId], [TraitId]) VALUES ('{race.ID}', '{t.ID}')");
                        }
                    }
                }
            }
        }

        private static async Task LoadRaces()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading races from API");
            Console.ForegroundColor = ForegroundColor;

            var races = await _apiHelper.LoadData<Race>("/api/races");

            if (races.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var race in races)
                {
                    Console.WriteLine($"Loading race {race.Index}");
                    var sql = "INSERT INTO Races " +
                              "([Id], [Index], [Name], [Source], [Speed], [Alignment], [Age], [Size], [SizeDescription], [LanguageDesc]) VALUES " +
                              $"('{race.ID}', '{race.Index}', '{race.Name}', '{race.Source}', {race.Speed}, '{race.Alignment}', '{race.Age}', '{race.Size}', '{race.SizeDescription}', '{race.LanguageDescription}')";
                    await connection.ExecuteAsync(
                        sql);
                }
            }
        }

        private static async Task LoadSkills()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading skills from API");
            Console.ForegroundColor = ForegroundColor;

            var skills = await _apiHelper.LoadData<Skill>("/api/skills");

            if (skills.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var skill in skills)
                {
                    Console.WriteLine($"Loading skill {skill.Index}");

                    var abilityId = await connection.QueryFirstAsync<string>(
                        "SELECT Id FROM AbilityScores WHERE Name = @Name", new { skill.AbilityScore.Name });

                    await connection.ExecuteAsync(
                        "INSERT INTO Skills " +
                        "([Id], [Index], [Name], [Description], [Source], [AbilityScoreId]) VALUES " +
                        $"('{skill.ID}', '{skill.Index}', '{skill.Name}', '{string.Join(" ", skill.Description)}', '{skill.Source}', '{abilityId}')");
                }
            }
        }

        private static async Task LoadSpellRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading spell relationships from API");

            Console.WriteLine("  *  Loading spells");
            var spells = await _apiHelper.LoadData<Spell>("/api/spells");

            Console.WriteLine("  *  Loading classes");
            var classes = await _apiHelper.LoadData<Class>("/api/classes");

            Console.WriteLine("  *  Loading subclasses");
            var subclasses = await _apiHelper.LoadData<Subclass>("/api/subclasses");

            Console.ForegroundColor = ForegroundColor;

            if (spells.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var spell in spells)
                {
                    Console.WriteLine($"Updating relationships for spell {spell.Index}");

                    // Class relationships
                    if (spell.Classes != null && spell.Classes.Any())
                    {
                        foreach (var @class in spell.Classes)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO ClassSpells ([ClassId], [SpellId]) VALUES ('{classes.First(x => x.Name == @class.Name).ID}', '{spell.ID}')");
                        }
                    }

                    // Subclass relationship
                    if (spell.Subclasses != null && spell.Subclasses.Any())
                    {
                        foreach (var subclass in spell.Subclasses)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO SubclassSpells ([SubclassId], [SpellId]) VALUES ('{subclasses.First(x => x.Name == subclass.Name).ID}', '{spell.ID}')");
                        }
                    }
                }
            }
        }

        private static async Task LoadSpells()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading spells from API");
            Console.ForegroundColor = ForegroundColor;

            var spells = await _apiHelper.LoadData<Spell>("/api/spells");

            if (spells.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var spell in spells)
                {
                    Console.WriteLine($"Loading spell {spell.Index}");

                    var schoolId =
                        await connection.QueryFirstAsync<string>("SELECT Id FROM MagicSchools WHERE Name = @Name",
                            new { spell.MagicSchool.Name });

                    spell.HigherLevel ??= new string[] { };

                    var sql = @$"INSERT INTO [dbo].[Spells]
                            ([Id],[Index],[Name],[Description],[HigherLevel],[Component_Verbal],[Component_Somatic],[Component_Material],[Concentration],[Level],[Ritual],[Material],[CastingTime],[SchoolId],[Source],[Duration]) VALUES
                        ('{spell.ID}'
                        ,'{spell.Index}'
                        ,'{spell.Name}'
                        ,'{string.Join(" ", spell.Description)}'
                        ,'{string.Join(" ", spell.HigherLevel)}'
                        ,{(spell.HasVerbalComponent ? 1 : 0)}
                        ,{(spell.HasSomaticComponent ? 1 : 0)}
                        ,{(spell.HasMaterialComponent ? 1 : 0)}
                        ,{(spell.Concentration ? 1 : 0)}
                        ,{spell.Level}
                        ,{(spell.IsRitual ? 1 : 0)}
                        ,'{spell.Material}'
                        ,'{spell.CastingTime}'
                        ,'{schoolId}'
                        ,'{spell.Source}'
                        ,'{spell.Duration}')";

                    await connection.ExecuteAsync(sql);
                }

                await connection.ExecuteAsync("UPDATE Spells SET HigherLevel = NULL WHERE HigherLevel = ''");
                await connection.ExecuteAsync("UPDATE Spells SET Material = NULL WHERE Material = ''");
            }
        }

        private static async Task LoadSubclasses()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading subclasses from API");
            Console.ForegroundColor = ForegroundColor;

            var subclasses = await _apiHelper.LoadData<Subclass>("/api/subclasses");

            if (subclasses.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var subclass in subclasses)
                {
                    Console.WriteLine($"Loading subclass {subclass.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO Subclasses " +
                        "([Id], [Index], [Name], [SubclassFlavor], [Description], [Source]) VALUES " +
                        $"('{subclass.ID}', '{subclass.Index}', '{subclass.Name}', '{subclass.SubclassFlavor}', '{string.Join(" ", subclass.Description)}', '{subclass.Source}')");
                }
            }
        }

        private static async Task LoadSubraceRelationships()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLoading subrace relationships from API");

            Console.WriteLine("  *  Loading subraces");
            var subraces = await _apiHelper.LoadData<Subrace>("/api/subraces");

            Console.WriteLine("  *  Loading races");
            var races = await _apiHelper.LoadData<Race>("/api/races");
            Console.ForegroundColor = ForegroundColor;

            if (subraces.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var subrace in subraces)
                {
                    Console.WriteLine($"Updating relationships for subrace {subrace.Index}");

                    await connection.ExecuteAsync($@"UPDATE Subraces SET RaceId = '{races.First(x => x.Name == subrace.Race.Name).ID}' WHERE ID = '{subrace.ID}'");
                }
            }
        }

        private static async Task LoadSubraces()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading subraces from API");
            Console.ForegroundColor = ForegroundColor;

            var races = await _apiHelper.LoadData<Subrace>("/api/subraces");

            if (races.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var race in races)
                {
                    Console.WriteLine($"Loading subrace {race.Index}");
                    var sql = "INSERT INTO Subraces " +
                              "([Id], [Index], [Name], [Source], [Description]) VALUES " +
                              $"('{race.ID}', '{race.Index}', '{race.Name}', '{race.Source}', '{race.Description}')";
                    await connection.ExecuteAsync(sql);
                }
            }
        }

        private static async Task LoadTraits()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading traits from API");
            Console.ForegroundColor = ForegroundColor;

            var traits = await _apiHelper.LoadData<Trait>("/api/traits");

            if (traits.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var trait in traits)
                {
                    Console.WriteLine($"Loading trait {trait.Index}");

                    // Bug fix for typo in data
                    if (trait.Index == "internal-legacy")
                        trait.Index = "infernal-legacy";

                    if (trait.Name == "Internal Legacy")
                        trait.Name = "Infernal Legacy";

                    await connection.ExecuteAsync(
                        "INSERT INTO Traits " +
                        "([Id], [Index], [Name], [Description], [Source]) VALUES " +
                        $"('{trait.ID}', '{trait.Index}', '{trait.Name}', '{string.Join(" ", trait.Description)}', '{trait.Source}')");
                }
            }
        }

        private static async Task LoadTypesWithoutDependencies()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading ability scores from API");
            Console.ForegroundColor = ForegroundColor;

            var abilityScores = await _apiHelper.LoadData<AbilityScore>("/api/ability-scores");

            if (abilityScores.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var score in abilityScores)
                {
                    Console.WriteLine($"Loading ability {score.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO AbilityScores " +
                        "([Id], [Index], [Name], [FullName], [Description], [Source]) VALUES " +
                        $"('{score.ID}', '{score.Index}', '{score.Name}', '{score.FullName}', '{string.Join(" ", score.Description).Replace("'", "&rsquo;")}', '{score.Source}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading conditions from API");
            Console.ForegroundColor = ForegroundColor;

            var conditions = await _apiHelper.LoadData<Condition>("/api/conditions");

            if (conditions.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var condition in conditions)
                {
                    Console.WriteLine($"Loading condition {condition.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO Conditions " +
                        "([Id], [Index], [Name], [Description], [Source]) VALUES " +
                        $"('{condition.ID}', '{condition.Index}', '{condition.Name}', '{string.Join(" ", condition.Description)}', '{condition.Source}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading damage types from API");
            Console.ForegroundColor = ForegroundColor;

            var damageTypes = await _apiHelper.LoadData<DamageType>("/api/damage-types");

            if (damageTypes.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var damageType in damageTypes)
                {
                    Console.WriteLine($"Loading damage type {damageType.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO DamageTypes " +
                        "([Id], [Index], [Name], [Description], [Source]) VALUES " +
                        $"('{damageType.ID}', '{damageType.Index}', '{damageType.Name}', '{string.Join(" ", damageType.Description)}', '{damageType.Source}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading schools of magic from API");
            Console.ForegroundColor = ForegroundColor;

            var magicSchools = await _apiHelper.LoadData<MagicSchool>("/api/magic-schools");

            if (magicSchools.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var magicSchool in magicSchools)
                {
                    Console.WriteLine($"Loading magic school {magicSchool.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO MagicSchools " +
                        "([Id], [Index], [Name], [Description], [Source]) VALUES " +
                        $"('{magicSchool.ID}', '{magicSchool.Index}', '{magicSchool.Name}', '{magicSchool.Description}', '{magicSchool.Source}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading weapon properties from API");
            Console.ForegroundColor = ForegroundColor;

            var weaponProperties = await _apiHelper.LoadData<WeaponProperty>("/api/weapon-properties");

            if (weaponProperties.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var weaponProperty in weaponProperties)
                {
                    Console.WriteLine($"Loading weapon property {weaponProperty.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO WeaponProperties " +
                        "([Id], [Index], [Name], [Description], [Source]) VALUES " +
                        $"('{weaponProperty.ID}', '{weaponProperty.Index}', '{weaponProperty.Name}', '{string.Join(" ", weaponProperty.Description)}', '{weaponProperty.Source}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading languages from API");
            Console.ForegroundColor = ForegroundColor;

            var languages = await _apiHelper.LoadData<Language>("/api/languages");

            if (languages.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var language in languages)
                {
                    Console.WriteLine($"Loading language {language.Index}");

                    await connection.ExecuteAsync(
                        "INSERT INTO Languages " +
                        "([Id], [Index], [Name], [TypicalSpeakers], [Source], [Type], [Script]) VALUES " +
                        $"('{language.ID}', '{language.Index}', '{language.Name}', '{string.Join(", ", language.TypicalSpeakers)}', '{language.Source}', '{language.Type}', '{language.Script}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading equipment from API");
            Console.ForegroundColor = ForegroundColor;

            var equipment = await _apiHelper.LoadData<Equipment>("/api/equipment");

            if (equipment.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var piece in equipment)
                {
                    Console.WriteLine($"Loading equipment {piece.Index}");

                    // Some pieces of equipment don't have a description
                    var description = string.Empty;

                    if (piece.Description != null && piece.Description.Any())
                    {
                        description = string.Join(" ", piece.Description);
                    }

                    await connection.ExecuteAsync(
                        "INSERT INTO Equipment " +
                        "([Id], [Index], [Name], [EquipmentCategory], [GearCategory], [Description], [Source], [Cost]) VALUES " +
                        $"('{piece.ID}', '{piece.Index}', '{piece.Name}', '{piece.EquipmentCategory}', '{piece.GearCategory}', '{description}', '{piece.Source}', '{piece.CostFromApi.Quantity} {piece.CostFromApi.Unit}')");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLoading proficiencies from API");
            Console.ForegroundColor = ForegroundColor;

            var proficiencies = await _apiHelper.LoadData<Proficiency>("/api/proficiencies");

            if (proficiencies.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                foreach (var proficiency in proficiencies)
                {
                    Console.WriteLine($"Loading proficiency {proficiency.Index}");

                    try
                    {
                        await connection.ExecuteAsync(
                            "INSERT INTO Proficiencies " +
                            "([Id], [Index], [Name], [Type], [Source]) VALUES " +
                            $"('{proficiency.ID}', '{proficiency.Index}', '{proficiency.Name}', '{proficiency.Type}', '{proficiency.Source}')");
                    }
                    catch (SqlException e)
                    {
                        if (e.Message.StartsWith("Violation of PRIMARY KEY",
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Duplicate data in API, ignore
                            var color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Skipping duplicate record {proficiency.Index}");
                            Console.ForegroundColor = color;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private static async Task TruncateAllTheThings()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nTRUNCATING DATABASE");
            Console.ForegroundColor = ForegroundColor;

            await using var connection = new SqlConnection(ConnectionString);

            // I want to run a single command to delete all data from all table
            // Problem 1: I don't want to worry about referential integrity
            // Problem 2: I don't want to cascade deletes
            // Problem 3: Out of the box, sp_MSForEachTable doesn't run in the order it needs
            // Solution: Throw as much ~~human pain and suffering~~ CPU at it as I can until it figures itself out
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    await connection.ExecuteAsync("EXEC sp_MSForEachTable 'DELETE FROM ?'");
                }
                catch
                {
                    Console.WriteLine($"lol u suck ({i})");
                }
            }
        }
    }

    internal class ApiHelper
    {
        public async Task<T[]> LoadData<T>(string rootEndpoint)
        {
            var results = new List<T>();
            using var listHelper = new HttpHelper<NamedAPIResourceList>();
            using var dataHelper = new HttpHelper<T>();

            var endpoints = await listHelper.ReadContent(rootEndpoint);

            foreach (var endpoint in endpoints.Results)
            {
                var dataPoint = await dataHelper.ReadContent(endpoint.URL);

                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(dataPoint);

                    switch (value)
                    {
                        case string _:
                            prop.SetValue(dataPoint, value.ToString()?.Replace("'", ""));
                            break;

                        case string[] values:
                        {
                            for (var i = 0; i < values.Length; i++)
                            {
                                values[i] = values[i].Replace("'", "");
                            }

                            prop.SetValue(dataPoint, values);
                            break;
                        }
                    }
                }

                results.Add(dataPoint);
            }

            return results.ToArray();
        }
    }
}
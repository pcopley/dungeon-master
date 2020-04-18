using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        private static readonly ConsoleColor BackgroundColor = Console.BackgroundColor;

        private static readonly ConsoleColor ForegroundColor = Console.ForegroundColor;

        private static ApiHelper _apiHelper;

        public static async Task Main(string[] args)
        {
            _apiHelper = new ApiHelper();

            await TruncateAllTheThings();

            // Types with no dependencies
            await LoadTypesWithoutDependencies();

            // Types with dependencies, but loading the data first
            await LoadSkills();

            await LoadSpells();

            await LoadFeatures();

            await LoadTraits();

            await LoadClasses();
            await LoadSubclasses();

            await LoadRaces();
            await LoadSubraces();

            await LoadMonsters();

            //// Load the foreign keys for the above types
            //await LoadForeignKeys();

            //// Since we loaded data with no FKs, tighten up
            //// the referential integrity constraints
            //await EnforceForeignKeys();
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

                    // todo classes
                    // todo subclasses

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
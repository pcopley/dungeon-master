using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DM.Data.Utilities.DataLoadUtility.RemoteData;

namespace DM.Data.Utilities.DataLoadUtility
{
    public class Program
    {
        private const string ConnectionString =
            @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DM;Trusted_Connection=True;";

        public static async Task Main(string[] args)
        {
            var abilityScores = (await LoadAbilityScores()).ToArray();

            if (abilityScores.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                await connection.ExecuteAsync("DELETE FROM AbilityScores");

                foreach (var score in abilityScores)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO AbilityScores " +
                        "([Id], [Index], [Name], [FullName], [Description], [Source]) VALUES " +
                        $"('{score.ID}', '{score.Index}', '{score.Name}', '{score.FullName}', '{string.Join(" ", score.Description).Replace("'", "&rsquo;")}', '{score.Source}')");
                }
            }

            var skills = (await LoadSkills()).ToArray();

            if (skills.Any())
            {
                await using var connection = new SqlConnection(ConnectionString);

                await connection.ExecuteAsync("DELETE FROM Skills");

                foreach (var skill in skills)
                {
                    var abilityId = await connection.QueryFirstAsync<string>(
                        "SELECT Id FROM AbilityScores WHERE Name = @Name", new { Name = skill.AbilityScore.Name });

                    await connection.ExecuteAsync(
                        "INSERT INTO Skills " +
                        "([Id], [Index], [Name], [Description], [Source], [AbilityScoreId]) VALUES " +
                        $"('{skill.ID}', '{skill.Index}', '{skill.Name}', '{string.Join(" ", skill.Description).Replace("'", "&rsquo;")}', '{skill.Source}', '{abilityId}')");
                }
            }
        }

        private static async Task<IEnumerable<AbilityScore>> LoadAbilityScores()
        {
            var results = new List<AbilityScore>();
            using var listHelper = new HttpHelper<NamedAPIResourceList>();
            using var httpHelper = new HttpHelper<AbilityScore>();

            var abilityScoreEndpoints = await listHelper.ReadContent("/api/ability-scores");

            foreach (var endpoint in abilityScoreEndpoints.Results)
            {
                var score = await httpHelper.ReadContent(endpoint.URL);

                results.Add(score);
            }

            return results;
        }

        private static IEnumerable<string> LoadRootEndpoints()
        {
            var validEndpoints = new[]
            {
                "/api/ability-scores",

                //"/api/classes",
                //"/api/conditions",
                //"/api/damage-types",
                //"/api/equipment-categories",
                //"/api/equipment",
                //"/api/features",
                //"/api/languages",
                //"/api/magic-schools",
                //"/api/monsters",
                //"/api/proficiencies",
                //"/api/races",
                "/api/skills",

                //"/api/spellcasting",
                //"/api/spells",
                //"/api/starting-equipment",
                //"/api/subclasses",
                //"/api/subraces",
                //"/api/traits",
                //"/api/weapon-properties"
            };

            return validEndpoints.AsEnumerable();

            //using var rootEndpointHelper = new HttpHelper<dynamic>();

            //var rootEndpoints = await rootEndpointHelper.ReadContent("/api");

            //return new List<string>();
        }

        private static async Task<IEnumerable<Skill>> LoadSkills()
        {
            var results = new List<Skill>();
            using var listHelper = new HttpHelper<NamedAPIResourceList>();
            using var httpHelper = new HttpHelper<Skill>();

            var skillEndpoints = await listHelper.ReadContent("/api/skills");

            foreach (var endpoint in skillEndpoints.Results)
            {
                var skill = await httpHelper.ReadContent(endpoint.URL);

                results.Add(skill);
            }

            return results;
        }
    }
}
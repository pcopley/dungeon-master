using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Monster
    {
        public string Alignment { get; set; }

        [JsonProperty("armor_class")]
        public int ArmorClass { get; set; }

        [JsonProperty("challenge_rating")]
        public decimal ChallengeRating { get; set; }

        [JsonProperty("hit_dice")]
        public string HitDice { get; set; }

        public int HitPoints { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Languages { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        public string Subtype { get; set; }

        public string Type { get; set; }
    }
}
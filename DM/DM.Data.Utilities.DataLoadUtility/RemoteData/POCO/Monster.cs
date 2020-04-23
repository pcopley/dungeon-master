using DM.Data.Utilities.DataLoadUtility.RemoteData.Infrastructure;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Monster
    {
        public Action[] Actions { get; set; }

        public string Alignment { get; set; }

        [JsonProperty("armor_class")]
        public int ArmorClass { get; set; }

        [JsonProperty("challenge_rating")]
        public decimal ChallengeRating { get; set; }

        //[JsonProperty("condition_immunities")]
        //public ReferenceData[] ConditionImmunities { get; set; }

        //[JsonProperty("damage_immunities")]
        //public ReferenceData[] DamageImmunities { get; set; }

        //[JsonProperty("damage_resistances")]
        //public ReferenceData[] DamageResistances { get; set; }

        //[JsonProperty("damage_vulnerabilities")]
        //public ReferenceData[] DamageVulnerabilities { get; set; }

        [JsonProperty("hit_dice")]
        public string HitDice { get; set; }

        public int HitPoints { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Languages { get; set; }

        [JsonProperty("legendary_actions")]
        public Action[] LegendaryActions { get; set; }

        public string Name { get; set; }

        public ReferenceData[] Proficiencies { get; set; }

        public dynamic Senses { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        [JsonProperty("special_abilities")]
        public Ability[] SpecialAbilities { get; set; }

        [JsonProperty("speed")]
        public dynamic Speeds { get; set; }

        public string Subtype { get; set; }

        public string Type { get; set; }
    }
}
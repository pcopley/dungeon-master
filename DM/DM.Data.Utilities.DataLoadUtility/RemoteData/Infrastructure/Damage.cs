using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.Infrastructure
{
    public class Damage
    {
        [JsonProperty("damage_bonus")]
        public int? Bonus { get; set; }

        [JsonProperty("damage_dice")]
        public string DamageDice { get; set; }

        [JsonProperty("damage_type")]
        public ReferenceData Type { get; set; }
    }
}
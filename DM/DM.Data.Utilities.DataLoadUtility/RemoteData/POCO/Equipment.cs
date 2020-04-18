using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Equipment : IRemoteData
    {
        [JsonProperty("cost")]
        public Cost CostFromApi { get; set; }

        [JsonIgnore]
        public string CostFromDatabase { get; set; }

        [JsonProperty("desc")]
        public string[] Description { get; set; }

        [JsonProperty("equipment_category")]
        public string EquipmentCategory { get; set; }

        [JsonProperty("gear_category")]
        public string GearCategory { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        public decimal Weight { get; set; }
    }
}
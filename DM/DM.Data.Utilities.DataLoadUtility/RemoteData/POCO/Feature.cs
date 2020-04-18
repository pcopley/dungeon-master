using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Feature
    {
        public ReferenceData Class { get; set; }

        [JsonProperty("desc")]
        public string[] Description { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        public ReferenceData Subclass { get; set; }
    }
}
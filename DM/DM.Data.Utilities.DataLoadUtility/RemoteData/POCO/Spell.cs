using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Spell
    {
        [JsonProperty("casting_time")]
        public string CastingTime { get; set; }

        [JsonProperty("components")]
        public string[] Components { get; set; }

        public bool Concentration { get; set; }

        [JsonProperty("desc")]
        public string[] Description { get; set; }

        public string Duration { get; set; }

        [JsonIgnore]
        public bool HasMaterialComponent { get; set; }

        [JsonIgnore]
        public bool HasSomaticComponent { get; set; }

        [JsonIgnore]
        public bool HasVerbalComponent { get; set; }

        [JsonProperty("higher_level")]
        public string[] HigherLevel { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        [JsonProperty("ritual")]
        public bool IsRitual { get; set; }

        public int Level { get; set; }

        [JsonProperty("school")]
        public ReferenceData MagicSchool { get; set; }

        [JsonIgnore]
        public string MagicSchoolId { get; set; }

        public string Material { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }
    }
}
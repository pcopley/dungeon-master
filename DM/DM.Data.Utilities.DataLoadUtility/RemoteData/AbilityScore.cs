﻿using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class AbilityScore : IRemoteData
    {
        [JsonProperty("desc")]
        public string[] Description { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }
    }
}
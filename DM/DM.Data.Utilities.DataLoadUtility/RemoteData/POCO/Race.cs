﻿using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Race
    {
        public string Age { get; set; }

        public string Alignment { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        [JsonProperty("language_description")]
        public string LanguageDescription { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        [JsonProperty("size_description")]
        public string SizeDescription { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        public int Speed { get; set; }
    }
}
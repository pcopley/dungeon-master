﻿using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Language : IRemoteData
    {
        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Name { get; set; }

        public string Script { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        public string Type { get; set; }

        [JsonProperty("typical_speakers")]
        public string[] TypicalSpeakers { get; set; }
    }
}
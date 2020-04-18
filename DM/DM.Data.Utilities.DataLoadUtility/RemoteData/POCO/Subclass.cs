using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.POCO
{
    public class Subclass
    {
        public ReferenceData Class { get; set; }

        [JsonProperty("desc")]
        public string[] Description { get; set; }

        [JsonProperty("_id")]
        public string ID { get; set; }

        public string Index { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The http://www.dnd5eapi.co/ source URL
        /// </summary>
        [JsonProperty("url")]
        public string Source { get; set; }

        [JsonProperty("subclass_flavor")]
        public string SubclassFlavor { get; set; }
    }
}
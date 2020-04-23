using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.Infrastructure
{
    public class Ability
    {
        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("dc")]
        public DifficultyClass DifficultyClass { get; set; }

        public string Name { get; set; }
    }
}
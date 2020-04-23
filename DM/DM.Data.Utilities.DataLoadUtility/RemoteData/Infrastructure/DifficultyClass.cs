using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData.Infrastructure
{
    public class DifficultyClass
    {
        [JsonProperty("dc_type")]
        public ReferenceData AbilityScore { get; set; }

        [JsonProperty("success_type")]
        public string SuccessType { get; set; }

        [JsonProperty("dc_value")]
        public int Value { get; set; }
    }
}
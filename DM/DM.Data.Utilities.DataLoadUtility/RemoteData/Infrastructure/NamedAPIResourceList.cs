using System.Collections.Generic;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class NamedAPIResourceList : IRemoteData
    {
        /// <summary>
        /// Total number of resources available from this API
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// A list of NamedAPIResources
        /// </summary>
        public IEnumerable<NamedAPIResource> Results { get; set; }
    }
}
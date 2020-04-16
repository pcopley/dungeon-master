using System.Collections.Generic;

namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class ClassAPIResourceList : IRemoteData
    {
        /// <summary>
        /// Total number of resources available from this API
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// A list of ClassAPIResources
        /// </summary>
        public IEnumerable<ClassAPIResource> Results { get; set; }
    }
}
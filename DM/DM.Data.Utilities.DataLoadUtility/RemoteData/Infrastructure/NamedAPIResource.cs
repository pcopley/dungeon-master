namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class NamedAPIResource : IRemoteData
    {
        /// <summary>
        /// The resource index for shorthand searching
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// The name of the referenced resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL of the referenced resource
        /// </summary>
        public string URL { get; set; }
    }
}
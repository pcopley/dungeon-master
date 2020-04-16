namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class ClassAPIResource : IRemoteData
    {
        /// <summary>
        /// The class of whom the resource belongs to
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// The resource index for shorthand searching
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// The URL of the referenced resource
        /// </summary>
        public string URL { get; set; }
    }
}
namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class RootAPIResource : IRemoteData
    {
        /// <summary>
        /// The name of the resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL of the resource
        /// </summary>
        public string URL { get; set; }
    }
}
namespace DM.Data.Utilities.DataLoadUtility.RemoteData
{
    public class Cost : IRemoteData
    {
        /// <summary>
        /// The numerical amount of coins
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit of coinage
        /// </summary>
        public string Unit { get; set; }
    }
}
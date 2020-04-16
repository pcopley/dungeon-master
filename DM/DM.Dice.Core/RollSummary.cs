namespace DM.Dice.Core
{
    /// <summary>
    /// Object to describe the result of an entire roll, with
    /// all modifiers, [dis]advantage, etc.
    /// </summary>
    public class RollSummary
    {
        /// <summary>
        /// The value of any modifier to the roll
        /// </summary>
        public int Modifier { get; set; }

        /// <summary>
        /// The value, taking into account all dice and any modifier
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Individual dice rolls
        /// </summary>
        public RollResult[] Rolls { get; set; }
    }
}
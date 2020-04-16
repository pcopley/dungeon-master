namespace DM.Dice.Core
{
    /// <summary>
    /// The result of an individual, unaltered dice roll
    /// </summary>
    public class RollResult
    {
        /// <summary>
        /// The type of dice rolled, valid values are: 2, 4, 6, 8, 10, 12, 20, 100
        /// todo investigate using an enum instead
        /// </summary>
        public int DiceType { get; set; }

        /// <summary>
        /// Whether or not to discard the roll, e.g. for [dis]advantage Always false at
        /// instantiation but result compilation can edit this value if necessary
        /// </summary>
        public bool Discard { get; set; }

        /// <summary>
        /// The number that was rolled
        /// </summary>
        public int Result { get; set; }
    }
}
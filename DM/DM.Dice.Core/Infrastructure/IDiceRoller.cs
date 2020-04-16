namespace DM.Dice.Core.Infrastructure
{
    public interface IDiceRoller
    {
        /// <summary>
        /// Roll a custom set of dice
        /// </summary>
        /// <param name="input">
        /// What to roll, e.g.:
        ///  * 3d6
        ///  * 2d8 +5
        ///  * 1d6 -1
        ///  * 2d8 1d4
        ///  * 7d10 1d4 +5
        /// </param>
        /// <returns><see cref="RollSummary"/>; will not return less than 1</returns>
        RollSummary Roll(string input);

        /// <summary>
        /// Rolls d20 with advantage, discarding the lower of two rolls
        /// </summary>
        /// <returns><see cref="RollSummary"/></returns>
        RollSummary RollAdvantage();

        /// <summary>
        /// Rolls d20 with disadvantage, discarding the higher of two rolls
        /// </summary>
        /// <returns><see cref="RollSummary"/></returns>
        RollSummary RollDisadvantage();
    }
}
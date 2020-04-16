using System;

namespace DM.Dice.Core.Infrastructure
{
    [Serializable]
    public class DiceRollException : ArgumentException
    {
        public DiceRollException() : base()
        {
        }

        public DiceRollException(string message) : base(message)
        {
        }

        public DiceRollException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
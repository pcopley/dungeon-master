using System;

namespace DM.Dice.Core.Infrastructure
{
    public class RandomizationProvider : IRandomizationProvider
    {
        private readonly Random _rand;

        public RandomizationProvider()
        {
            _rand = new Random();
        }

        public int Next(int maxValue)
        {
            return _rand.Next(1, maxValue);
        }
    }
}
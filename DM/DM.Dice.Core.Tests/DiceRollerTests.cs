using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DM.Dice.Core.Infrastructure;
using Xunit;

// ReSharper disable IdentifierTypo
namespace DM.Dice.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class DiceRollerTests
    {
        private DiceRoller Roller { get; set; }

        public DiceRollerTests()
        {
            Roller = new DiceRoller(new RandomizationProvider());
        }

        [Fact]
        public void Roll_Doesnt_Parse_Without_Dice()
        {
            var exception = Assert.Throws<DiceRollException>(() => Roller.Roll("throws exception"));

            Assert.Equal("No valid dice rolls detected", exception.Message);
        }

        [Fact]
        public void RollAdvantage_discards_lower_roll()
        {
            var result = Roller.RollAdvantage();

            var discardedValue = result.Rolls.Single(x => x.Discard).Result;
            var otherValue = result.Rolls.Single(x => !x.Discard).Result;

            Assert.True(discardedValue < otherValue);
        }

        [Fact]
        public void RollAdvantage_discards_one_roll()
        {
            var result = Roller.RollAdvantage();

            Assert.Equal(1, result.Rolls.Count(x => x.Discard));
            Assert.Equal(1, result.Rolls.Count(x => !x.Discard));
        }

        [Fact]
        public void RollAdvantage_should_roll_2d20()
        {
            var result = Roller.RollAdvantage();

            Assert.True(result.Rolls.Length == 2);

            foreach (var roll in result.Rolls)
            {
                Assert.True(roll.DiceType == 20);
            }
        }

        [Fact]
        public void RollDisadvantage_discards_higher_roll()
        {
            var result = Roller.RollDisadvantage();

            var discardedValue = result.Rolls.Single(x => x.Discard).Result;
            var otherValue = result.Rolls.Single(x => !x.Discard).Result;

            Assert.True(discardedValue > otherValue);
        }

        [Fact]
        public void RollDisadvantage_discards_one_roll()
        {
            var result = Roller.RollDisadvantage();

            Assert.Equal(1, result.Rolls.Count(x => x.Discard));
            Assert.Equal(1, result.Rolls.Count(x => !x.Discard));
        }
    }
}
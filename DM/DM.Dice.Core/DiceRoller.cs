using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DM.Dice.Core.Infrastructure;

namespace DM.Dice.Core
{
    public class DiceRoller : IDiceRoller
    {
        private readonly IRandomizationProvider _rand;

        public DiceRoller(IRandomizationProvider rand)
        {
            _rand = rand;
        }

        public RollSummary Roll(string input)
        {
            var (requests, modifier) = ParseDiceRollRequest(input);

            var rolls = new List<RollResult>();

            foreach (var request in requests)
            {
                for (var i = 0; i < request.Quantity; i++)
                {
                    rolls.Add(new RollResult
                    {
                        DiceType = request.DiceType,
                        Discard = false,
                        Result = _rand.Next(request.DiceType)
                    });
                }
            }

            return new RollSummary
            {
                Modifier = modifier,
                Result = Math.Max(rolls.Sum(x => x.Result) + modifier, 1),
                Rolls = rolls.ToArray()
            };
        }

        public RollSummary RollAdvantage()
        {
            var summary = Roll("2d20");

            if (summary.Rolls[0].Result > summary.Rolls[1].Result)
            {
                summary.Rolls[1].Discard = true;
            }
            else
            {
                summary.Rolls[0].Discard = true;
            }

            return summary.RecalculateTotal();
        }

        public RollSummary RollDisadvantage()
        {
            var summary = Roll("2d20");

            if (summary.Rolls[0].Result < summary.Rolls[1].Result)
            {
                summary.Rolls[1].Discard = true;
            }
            else
            {
                summary.Rolls[0].Discard = true;
            }

            return summary.RecalculateTotal();
        }

        private static Tuple<RollRequest[], int> ParseDiceRollRequest(string input)
        {
            var modifier = 0;
            var pieces = input.Split(' ');

            if (pieces.All(x => !x.Contains('d')))
            {
                throw new DiceRollException("No valid dice rolls detected");
            }

            var requests = new List<RollRequest>();

            foreach (var piece in pieces)
            {
                if (piece.Contains('d'))
                {
                    // Piece is a die roll request
                    var roll = piece.Split('d');

                    int.TryParse(roll[0], out var quantity);
                    int.TryParse(roll[1], out var diceType);

                    requests.Add(new RollRequest
                    {
                        Quantity = quantity,
                        DiceType = diceType
                    });
                }
                else
                {
                    // Piece is a modifier
                    int.TryParse(piece, out modifier);
                }
            }

            return new Tuple<RollRequest[], int>(requests.ToArray(), modifier);
        }
    }
}
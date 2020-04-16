using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Dice.Core
{
    internal sealed class RollRequest
    {
        public int DiceType { get; set; }

        public int Quantity { get; set; }
    }
}
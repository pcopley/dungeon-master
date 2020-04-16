using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Dice.Core.Infrastructure
{
    public static class RollSummaryExtensions
    {
        public static RollSummary RecalculateTotal(this RollSummary summary)
        {
            summary.Result = summary
                .Rolls
                .Where(x => !x.Discard)
                .Sum(x => x.Result);

            return summary;
        }
    }
}
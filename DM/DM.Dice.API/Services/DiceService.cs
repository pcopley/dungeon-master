using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Dice.Core.Infrastructure;
using DM.Dice.Core.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace DM.Dice.API.Services
{
    public class DiceService : DiceRoller.DiceRollerBase
    {
        private readonly IDiceRoller _diceRoller;

        private readonly ILogger<DiceService> _logger;

        public DiceService(IDiceRoller diceRoller, ILogger<DiceService> logger)
        {
            _diceRoller = diceRoller;
            _logger = logger;

            _logger.LogTrace($"New {nameof(DiceService)} instantiated");
        }

        public override async Task Roll(RollRequest request, IServerStreamWriter<RollResult> responseStream, ServerCallContext context)
        {
            var result = _diceRoller.Roll(request.Request);

            foreach (var roll in result.Rolls)
            {
                await responseStream.WriteAsync(new RollResult
                {
                    Result = roll.Result,
                    Type = roll.DiceType
                });
            }
        }
    }
}
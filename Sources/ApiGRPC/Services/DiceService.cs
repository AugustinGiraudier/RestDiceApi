using Grpc.Core;
using ModelAppLib;
using ApiGRPC.Extentions;
using Microsoft.AspNetCore.Components;

namespace ApiGRPC.Services
{
    public class DiceService : Dices.DicesBase
    {
        private IRandomizer _randomizer;
        private readonly IDataManager _manager;
        private readonly ILogger<DiceService> _logger;


        public DiceService(ILogger<DiceService> logger, IDataManager manager, IRandomizer rd)
        {
            _logger = logger;
            _manager = manager;
            _randomizer = rd;
        }

        // GET ALL
        public async override Task<DicesReply> getDices(Empty request, ServerCallContext context)
        {
            var rep = await _manager.GetAllDices();
            if (rep == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "no Dice found"));
            }
            return rep.ToReply();
        }

        // GET
        public async override Task<DiceReply> getDice(DiceRequest request, ServerCallContext context)
        {
            var rep = await _manager.GetDiceWithId(request.Id);
            if (rep == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Dice not found"));
            }
            return rep.ToReply();
        }

        // DELETE
        public async override Task<DiceReply> deleteDice(DiceRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"delete dice with id={request.Id}");
            var diceToDelete = await _manager.GetDiceWithId(request.Id);

            var resultDelete = await _manager.DeleteDice(diceToDelete);

            if (!resultDelete)
            {
                _logger.LogError($"Unable to find dice with id={request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Unable to delete Dice..."));
            }
            _logger.LogTrace("delete dice success");
            return diceToDelete.ToReply();
        }

        // POST
        public async override Task<DiceReply> addDice(InputDiceRequest request, ServerCallContext context)
        {
            _logger.LogTrace("add dice");
            DiceSideType[] dst = new DiceSideType[request.Types_.Count];
            for(int i= 0; i < request.Types_.Count; i++)
            {
                var diceEntity = await _manager.GetDiceSideWithId(request.Types_[i].ProtoId);
                if(diceEntity == null)
                    throw new RpcException(new Status(StatusCode.NotFound, $"Unable to find side for id={request.Types_[i].ProtoId}..."));
                dst[i] = new DiceSideType((int)request.Types_[i].NbSides, diceEntity);
            }
            var d = new Dice(_randomizer, dst);
            var addedDice = await _manager.AddDice(d);
            if (!addedDice)
            {
                _logger.LogError("Unable to add new Dice");
                throw new RpcException(new Status(StatusCode.NotFound, "Unable to add Dice..."));
            }
            _logger.LogTrace("add dice success");
            return d.ToReply();
        }
    }
}

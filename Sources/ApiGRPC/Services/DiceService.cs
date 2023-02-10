using Grpc.Core;
using ModelAppLib;
using ApiGRPC.Extentions;

namespace ApiGRPC.Services
{
    public class DiceService : Dices.DicesBase
    {
        private readonly IDataManager _manager;
        private readonly ILogger<DiceService> _logger;

        public DiceService(ILogger<DiceService> logger, IDataManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public async override Task<DiceReply> getDices(DiceRequest request, ServerCallContext context)
        {
            var rep = await _manager.GetDiceWithId(request.Id);
            if (rep == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Dice not found"));
            }
            return rep.ToReply();
        }
    }
}

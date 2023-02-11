using Grpc.Core;
using ModelAppLib;
using ApiGRPC.Extentions;

namespace ApiGRPC.Services
{
    public class SideService : Sides.SidesBase
    {
        private readonly IDataManager _manager;
        private readonly ILogger<SideService> _logger;

        public SideService(ILogger<SideService> logger, IDataManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public async override Task<SidesReply> getSides(Empty request, ServerCallContext context)
        {
            _logger.LogTrace("get all sides");
            var rep = await _manager.GetAllSides();
            if (rep == null)
            {
                _logger.LogError("Unable to find any Side");
                throw new RpcException(new Status(StatusCode.NotFound, "No Side found"));
            }
            _logger.LogTrace("get all success");
            return rep.ToReply();
        }

        public async override Task<SideReply> getSide(SideRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"get side with id={request.Id}");
            var rep = await _manager.GetDiceSideWithId(request.Id);
            if (rep == null)
            {
                _logger.LogError($"Unable to find Side with id={request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Side not found"));
            }
            _logger.LogTrace("get side success");
            return rep.ToReply();
        }

        public override async Task<SideReply> deleteSide(SideRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"delete side with id={request.Id}");
            var diceSideToDelete = await _manager.GetDiceSideWithId(request.Id);

            var resultDelete = await _manager.DeleteSide(diceSideToDelete);

            if (!resultDelete)
            {
                _logger.LogError($"Unable to find Side with id={request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Unable to delete Side..."));
            }
            _logger.LogTrace("delete side success");
            return diceSideToDelete.ToReply();
        }
        public async override Task<SideReply> addSide(InputSideRequest request, ServerCallContext context)
        {
            _logger.LogTrace("add side");
            var ds = new DiceSide(request.Image);
            var addedDice = await _manager.AddSide(ds);
            if (!addedDice)
            {
                _logger.LogError("Unable to add new Side");
                throw new RpcException(new Status(StatusCode.NotFound, "Unable to add Side..."));
            }
            _logger.LogTrace("add side success");
            return ds.ToReply();
        }

        //public async override Task<SideReply> updateSide(InputSideRequest request, ServerCallContext context)
        //{
            
        //}
    }
}

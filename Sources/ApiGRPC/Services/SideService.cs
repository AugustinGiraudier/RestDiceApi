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

        public async override Task<SideReply> getSides(SideRequest request, ServerCallContext context)
        {
            var rep = await _manager.GetDiceSideWithId(request.Id);
            if (rep == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Side not found"));
            }
            return rep.ToReply();
        }
    }
}

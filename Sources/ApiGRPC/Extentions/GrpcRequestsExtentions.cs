using ModelAppLib;

namespace ApiGRPC.Extentions
{
    public static class GrpcRequestsExtentions
    {
        // ----------- TO REPLY ----------- //
        public static SideReply ToReply(this DiceSide model)
        {
            var rp = new SideReply();
            rp.Id = model.Id;
            rp.Image = model.Image;
            return rp;
        }

    }
}

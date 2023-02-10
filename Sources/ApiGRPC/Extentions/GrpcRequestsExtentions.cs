using Google.Protobuf.Collections;
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
        public static DiceReply ToReply(this Dice model)
        {
            var rp = new DiceReply
            {
                Id = model.Id
            };
            foreach (var st in model.SideTypes)
            {
                rp.Type.Add(new SideType
                {
                    NbSides = st.NbSide,
                    Proto = st.Prototype.ToReply()
                });
            }
            return rp;
        }

    }
}

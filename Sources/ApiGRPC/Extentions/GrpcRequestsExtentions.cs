using Google.Protobuf.Collections;
using ModelAppLib;

namespace ApiGRPC.Extentions
{
    public static class GrpcRequestsExtentions
    {
        // ----------- TO REPLY ----------- //
        // SIDES //
        public static SideReply ToReply(this DiceSide model)
        {
            var rp = new SideReply();
            rp.Id = model.Id;
            rp.Side = new InputSideRequest { Image = model.Image };
            return rp;
        }
        public static SidesReply ToReply(this IEnumerable<DiceSide> model)
        {
            var rp = new SidesReply();
            foreach(var side in model)
            {
                rp.Sides.Add(side.ToReply());
            }
            return rp;
        }
        // DICES //
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

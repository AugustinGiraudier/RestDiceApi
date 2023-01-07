using System.Collections.Generic;
using ModelAppLib;

namespace ModelAppLib_UnitTests;

public class StubForUT
{
    public static Dice getDiceWithValue()
    {
        return new Dice(new SecureRandomizer(), new DiceSideType(1, new DiceSide("1.png")),
            new DiceSideType(2, new DiceSide("1.png")),
            new DiceSideType(3, new DiceSide("1.png")));
    }

    public static Game getGameWithValue()
    {
        return new Game(new DiceType[]
        {
            new DiceType(1, new Dice(new SecureRandomizer(),new DiceSideType(1, new DiceSide("img1")))),
            new DiceType(2, new Dice(new SecureRandomizer(),new DiceSideType(2, new DiceSide("img2")))),
            new DiceType(3, new Dice(new SecureRandomizer(),new DiceSideType(3, new DiceSide("img3"))))
        });
    }
}
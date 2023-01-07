using ModelAppLib;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StubEntitiesLib")]

namespace EntitiesLib
{
    /// <summary>
    /// Classe contenant les extentions ToModel et ToEntity pour chaque entité du projet
    /// </summary>
    internal static class Extentions
    {


        // --- Dice Side --- //
        
        public static DiceSide ToModel(this DiceSideEntity entity)
        {
            var ds = new DiceSide(entity.Image);
            ds.Id = entity.Id;
            return ds;
        }
        public static DiceSideEntity ToEntity(this DiceSide model)
                => new DiceSideEntity { Image = model.Image, Id = model.Id };

        public static IEnumerable<DiceSide> ToModel(this IEnumerable<DiceSideEntity> entities)
                => entities.Select(e => e.ToModel());

        public static IEnumerable<DiceSideEntity> ToEntity(this IEnumerable<DiceSide> models)
            => models.Select(m => m.ToEntity());


        // --- Dice Side Type --- //

        public static DiceSideType ToModel(this DiceSideTypeEntity entity)
                => new DiceSideType(entity.NbSide, entity.Prototype.ToModel());
        public static DiceSideTypeEntity ToEntity(this DiceSideType model, long DiceId)
                => new DiceSideTypeEntity { NbSide = model.NbSide, Prototype = model.Prototype.ToEntity(), Side_FK=model.Prototype.Id, Dice_FK=DiceId };
        public static IEnumerable<DiceSideType> ToModel(this IEnumerable<DiceSideTypeEntity> entities)
                => entities.Select(e => e.ToModel());
        public static IEnumerable<DiceSideTypeEntity> ToEntity(this IEnumerable<DiceSideType> models, long DiceId)
                => models.Select(m => m.ToEntity(DiceId));


        // --- Dice --- //
        public static Dice ToModel(this DiceEntity entity)
        {
            var d = new Dice(new SecureRandomizer(), entity.Sides.ToModel());
            d.Id = entity.Id;
            return d;
        }
        public static DiceEntity ToEntity(this Dice model, DiceLauncherDbContext context)
        {
            var d = new DiceEntity { Sides = model.SideTypes.ToEntity(model.Id).ToList(), Id = model.Id };
            foreach (var side in d.Sides)
                side.Prototype = context.Sides.Where(s => s.Id == side.Side_FK).First();
            return d;
        }
        public static IEnumerable<Dice> ToModel(this IEnumerable<DiceEntity> entities)
            => entities.Select(e => e.ToModel());
        public static IEnumerable<DiceEntity> ToEntity(this IEnumerable<Dice> models, DiceLauncherDbContext context)
            => models.Select(m => m.ToEntity(context));


        // --- Dice Type --- //
        public static DiceType ToModel(this DiceTypeEntity entity)
                => new DiceType(entity.NbDice, entity.Prototype.ToModel());
        public static DiceTypeEntity ToEntity(this DiceType model, long GameId, DiceLauncherDbContext context)
            => new DiceTypeEntity { NbDice = model.NbDices, Prototype = model.Prototype.ToEntity(context), Dice_FK = model.Prototype.Id, Game_FK= GameId };
        public static IEnumerable<DiceType> ToModel(this IEnumerable<DiceTypeEntity> entities)
            => entities.Select(e => e.ToModel());
        public static IEnumerable<DiceTypeEntity> ToEntity(this IEnumerable<DiceType> models, long GameId, DiceLauncherDbContext context)
            => models.Select(m => m.ToEntity(GameId, context));

        // --- Game --- //
        public static Game ToModel(this GameEntity entity)
        {
            var g = new Game(entity.DiceTypes.ToModel());
            g.Id = entity.Id;
            return g;
        }
        public static GameEntity ToEntity(this Game model, DiceLauncherDbContext context)
        {
            var g = new GameEntity { DiceTypes = model.Dices.ToEntity(model.Id, context).ToList(), Id = model.Id };
            foreach (var dice in g.DiceTypes)
                dice.Prototype = context.Dices.Where(d => d.Id == dice.Dice_FK).First();
            return g;
        }
        public static IEnumerable<Game> ToModel(this IEnumerable<GameEntity> entities)
            => entities.Select(e => e.ToModel());
        public static IEnumerable<GameEntity> ToEntity(this IEnumerable<Game> models, DiceLauncherDbContext context)
            => models.Select(m => m.ToEntity(context));

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelAppLib;

namespace EntitiesLib
{
    /// <summary>
    /// Manager de données utilisant une base de donnée Entity Framework comme support
    /// </summary>
    public class DataBaseLinker : IDataManager
    {
        protected readonly DiceLauncherDbContext context;

        /// <summary>
        /// Create a db linker with existing context. It will be disposed at the end
        /// </summary>
        /// <param name="context"></param>
        public DataBaseLinker(DiceLauncherDbContext context)
        {
            this.context = context;
        }
        public DataBaseLinker()
        {
            this.context = new DiceLauncherDbContext();
        }




        // ===================================================== //
        //      = CREATE =
        // ===================================================== //


        public async Task<bool> AddDice(Dice dice)
        {
            var entity = dice.ToEntity(context);
            await context.Dices.AddAsync(entity);
            await context.SaveChangesAsync();
            dice.Id = entity.Id;
            return true;
        }
        public async Task<bool> AddGame(Game game)
        {
            var entity = game.ToEntity(context);
            await context.Games.AddAsync(entity);
            await context.SaveChangesAsync();
            game.Id = entity.Id;
            return true;
        }
        public async Task<bool> AddSide(DiceSide side)
        {
            var entity = side.ToEntity();
            await context.Sides.AddAsync(entity);
            await context.SaveChangesAsync();
            side.Id = entity.Id;
            return true;
        }


        // ===================================================== //
        //      = REMOVE =
        // ===================================================== //
        

        public async Task<bool> DeleteDice(Dice d)
        {
            context.Dices.Remove(GetDiceEntity(d));
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteGame(Game g)
        {
            context.Games.Remove(GetGameEntity(g));
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteSide(DiceSide ds)
        {
            context.Sides.Remove(GetSideEntity(ds));
            await context.SaveChangesAsync();
            return true;
        }


        // ===================================================== //
        //      = SELECT =
        // ===================================================== //


        public Task<IEnumerable<Dice>> GetAllDices()
        {
            return Task.FromResult(
                    context.Dices.Include(d => d.Sides)
                                .ThenInclude(s => s.Prototype)
                                .ToModel()
                                );
        }
        public Task<IEnumerable<Game>> GetAllGames()
        {
            return Task.FromResult( 
                context.Games.Include(g => g.DiceTypes)
                                .ThenInclude(d => d.Prototype)
                                .ToModel()
                                );
        }
        public Task<IEnumerable<DiceSide>> GetAllSides()
        {
            return Task.FromResult(context.Sides.ToModel());
        }
        public Task<IEnumerable<Dice>> GetSomeDices(int nb, int page)
        {
            return Task.FromResult(
                        context.Dices.Include(d => d.Sides)
                                    .ThenInclude(s => s.Prototype)
                                    .Skip(nb * page)
                                    .Take(nb)
                                    .ToModel()
                                    );
        }
        public Task<IEnumerable<Game>> GetSomeGames(int nb, int page)
        {
            return Task.FromResult(
                        context.Games.Include(g => g.DiceTypes)
                                    .ThenInclude(d => d.Prototype)
                                    .Skip(nb * page)
                                    .Take(nb)
                                    .ToModel()
                                    );
        }
        public Task<IEnumerable<DiceSide>> GetSomeSides(int nb, int page)
        {
            return Task.FromResult(
                context.Sides.Skip(nb * page).Take(nb)
                                .ToModel()
                ) ;
        }


        // ===================================================== //
        //      = COUNT =
        // ===================================================== //


        public Task<int> GetNbDice()
        {
            return Task.FromResult(context.Dices.Count());
        }

        public Task<int> GetNbGame()
        {
            return Task.FromResult(context.Games.Count());
        }

        public Task<int> GetNbSide()
        {
            return Task.FromResult(context.Sides.Count());
        }
        

        // ===================================================== //
        //      = UPDATE =
        // ===================================================== //


        public async Task<bool> AddDiceToGame(Game g, Dice d, int nb = 1)
        {
            CheckNumberIsPositive(nb);

            GameEntity ge = GetGameEntity(g);
            DiceEntity de = GetDiceEntity(d);

            bool flagGameHasDice = false;
            foreach(var diceT in ge.DiceTypes)
            {
                if(diceT.Prototype == de)
                {
                    flagGameHasDice = true;
                    diceT.NbDice += nb;
                    break;
                }
            }

            if (!flagGameHasDice)
                ge.DiceTypes.Add(new DiceTypeEntity { Prototype = de, Dice_FK = de.Id, Game = ge, Game_FK = ge.Id, NbDice = nb });

            await context.SaveChangesAsync();

            g.AddDiceType(new DiceType(nb, d));

            return true;
        }

        public async Task<bool> AddSideToDice(Dice d, DiceSide ds, int nb=1)
        {
            CheckNumberIsPositive(nb);

            DiceEntity de = GetDiceEntity(d);
            DiceSideEntity dse = GetSideEntity(ds);

            bool flagDiceHasSide = false;
            foreach (var sideT in de.Sides)
            {
                if (sideT.Prototype == dse)
                {
                    flagDiceHasSide = true;
                    sideT.NbSide += nb;
                    break;
                }
            }

            if (!flagDiceHasSide)
                de.Sides.Add(new DiceSideTypeEntity { Prototype = dse, Dice_FK = dse.Id, Dice = de, Side_FK = dse.Id, NbSide = nb });

            await context.SaveChangesAsync();

            d.AddSide(new DiceSideType(nb, ds));

            return true;
        }

        public async Task<bool> RemoveDiceFromGame(Game g, Dice d, int nb=1)
        {
            CheckNumberIsPositive(nb);

            GameEntity ge = GetGameEntity(g);
            DiceEntity de = GetDiceEntity(d);

            DiceTypeEntity dteToRemove = null;
            foreach (var diceT in ge.DiceTypes)
            {
                if (diceT.Prototype == de)
                {
                    if(diceT.NbDice > nb)
                        diceT.NbDice -= nb;
                    else
                        dteToRemove = diceT;
                    break;
                }
            }
            if (!ReferenceEquals(dteToRemove,null))
                ge.DiceTypes.Remove(dteToRemove);

            await context.SaveChangesAsync();

            g.RemoveDiceType(new DiceType(nb, d));

            return true;
        }

        public async Task<bool> RemoveSideFromDice(Dice d, DiceSide ds, int nb = 1)
        {
            CheckNumberIsPositive(nb);

            DiceEntity de = GetDiceEntity(d);
            DiceSideEntity dse = GetSideEntity(ds);

            DiceSideTypeEntity dsteToRemove = null;
            foreach (var SideT in de.Sides)
            {
                if (SideT.Prototype == dse)
                {
                    if (SideT.NbSide > nb)
                        SideT.NbSide -= nb;
                    else
                        dsteToRemove = SideT;
                    break;
                }
            }
            if (!ReferenceEquals(dsteToRemove, null))
                de.Sides.Remove(dsteToRemove);

            await context.SaveChangesAsync();

            d.RemoveSideType(new DiceSideType(nb, ds));

            return true;
        }


        // ===================================================== //
        //      = PRIVATE =
        // ===================================================== //

        private static void CheckNumberIsPositive(int nb)
        {
            if (nb <= 0)
                throw new ArgumentOutOfRangeException(nameof(nb), "Le nombre à ajouter ne peut etre null ou négatif...");
        }

        /// <summary>
        /// Retourne l'entité de dé correspondant au dé
        /// </summary>
        /// <param name="d">dé</param>
        /// <returns></returns>
        private DiceEntity GetDiceEntity(Dice d)
        {
            try
            {
                return context.Dices.First(d2 => d2.Id == d.Id);
            }
            catch(InvalidOperationException)
            {
                throw new ArgumentException("le dé n'existe pas dans la base...", nameof(d));
            }
        }

        /// <summary>
        /// Retourne l'entité de face correspondant à la face
        /// </summary>
        /// <param name="ds">face</param>
        /// <returns></returns>
        private DiceSideEntity GetSideEntity(DiceSide ds)
        {
            try
            {
                return context.Sides.First(d2 => d2.Id == ds.Id);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("la face n'existe pas dans la base...", nameof(ds));
            }
        }

        /// <summary>
        /// Retourne l'entité de partie correspondant à la partie
        /// </summary>
        /// <param name="g">partie</param>
        /// <returns></returns>
        private GameEntity GetGameEntity(Game g)
        {
            try
            {
                return context.Games.First(g2 => g2.Id == g.Id);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("la partie n'existe pas dans la base...", nameof(g));
            }
        }
    }
}

using EntitiesLib;
using StubLib;
using System;
using System.Linq;

namespace StubEntitiesLib
{
    /// <summary>
    /// Permet de renseigner des faces dans la base de donnée grace à l'utilisation du stub.
    /// </summary>
    public class StubedDatabaseLinker : DataBaseLinker
    {

        private bool StubDices;

        public StubedDatabaseLinker(DiceLauncherDbContext context, bool addDices = true)
            :base(context)
        {
            StubDices = addDices;
            StubThisLinker();
        }
        
        public StubedDatabaseLinker(bool addDices = true)
            :base()
        {
            this.StubDices = addDices;
            StubThisLinker();
        }

        private void StubThisLinker()
        {
            var stub = new Stub();
            if (!context.Sides.Any())
            {
                var sides = stub.GetAllSides().Result;
                foreach (var side in sides)
                    this.context.Sides.Add(side.ToEntity());
                context.SaveChanges();
            }
            if (StubDices && !context.Dices.Any())
            {
                var dices = stub.GetAllDices().Result;
                foreach (var dice in dices)
                    this.context.Dices.Add(dice.ToEntity(this.context));
                context.SaveChanges();
            }

        }
    }
}




using System.Collections.Generic;

namespace EntitiesLib
{
    /// <summary>
    /// Entité d'une partie
    /// </summary>
    public class GameEntity
    {
        public long Id { get; set; }

        public ICollection<DiceTypeEntity> DiceTypes { get; set; } = new List<DiceTypeEntity>();
    }
}

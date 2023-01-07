using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ModelAppLib
{
    /// <summary>
    /// Classe modélisant une partie avec ses dés et la possibilité de simuler leur lancer
    /// </summary>
    public class Game  : IEquatable<Game>
    {
        public long Id { get; set; }
        private readonly List<DiceType> dices = new List<DiceType>();
        public ReadOnlyCollection<DiceType> Dices => dices.AsReadOnly(); 


        public Game(IEnumerable<DiceType> dices)
        {
            if(dices == null)
                throw new ArgumentNullException(nameof(dices), "la liste de dés ne peut etre null");
            
            foreach (var dice in dices)
            {
                if (dice == null)
                    throw new ArgumentNullException(nameof(dices), "un des dés est null");
                AddDiceType(dice);
            }
        }
        public Game(params DiceType[] dices)
            :this(dices.AsEnumerable())
        {
        }

        /// <summary>
        /// Ajoute un type de dé au jeu
        /// </summary>
        /// <param name="dt">type de dé à ajouter</param>
        /// <returns></returns>
        public bool AddDiceType(DiceType dt)
        {
            if(dt == null)
                throw new ArgumentNullException(nameof(dt), "le type de dé ne peut etre null");
            if (dices.Contains(dt))
            {
                dices.Find(x => x.Equals(dt))?.AddDices(dt.NbDices);
                return false;
            }
            dices.Add(dt);
            return true;
        }

        /// <summary>
        /// Retire un type de dé du jeu
        /// </summary>
        /// <param name="dt">type de dé à retirer</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveDiceType(DiceType dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt), "le type de dé ne peut etre null");


            var theDt = dices.Find(x => x.Prototype.Equals(dt.Prototype));
            if (theDt == null)
                throw new ArgumentException("la partie ne contient pas ce type de dé...", nameof(dt));

            if (theDt.NbDices - dt.NbDices > 0)
                theDt.RemoveDices(dt.NbDices);
            else
                dices.Remove(theDt);
        }

        /// <summary>
        /// Génère un lancé des dés de la partie
        /// </summary>
        /// <returns>la liste des faces résultat</returns>
        public IEnumerable<DiceSide> LaunchDices()
        {
            var ret = new List<DiceSide>();

            foreach (var dice in dices) // chaque type de dé
            {
                Dice d = dice.Prototype;
                for (int i=0; i<dice.NbDices; i++) // chaque dé
                {
                    ret.Add(d.GetRandomSide());
                }
            }

            return ret;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!obj.GetType().Equals(GetType())) return false;
            return Equals(obj as Game);
        }
        
        public bool Equals(Game other)
        {
            return dices.SequenceEqual(other.dices) && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Dices.GetHashCode();
        }
    }
}

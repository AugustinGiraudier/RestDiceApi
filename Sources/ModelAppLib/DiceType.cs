
using System;

namespace ModelAppLib
{
    /// <summary>
    /// Classe modélisant un type de dé avec son prototype de référence ainsi que sa quantité
    /// </summary>
    public class DiceType : IEquatable<DiceType>
    {
        /// <summary>
        /// Nombre de dé de ce type
        /// </summary>
        public int NbDices { get; private set; }

        public  Dice Prototype { get; private set; }

        /// <summary>
        /// Construit un type de dé avec un dé et une quantité
        /// </summary>
        /// <param name="nbDices">Nombre de dé de ce type</param>
        /// <param name="prototype">Dé cible</param>
        public DiceType(int nbDices, Dice prototype)
        {
            if (nbDices <= 0)
                throw new ArgumentOutOfRangeException(nameof(nbDices), "Le nombre de dés doit être suppérieur à 0");
            if(prototype == null)
                throw new ArgumentNullException(nameof(prototype), "Le prototype ne peut être null");
            NbDices = nbDices;
            Prototype = prototype;
        }

        /// <summary>
        /// Ajoute un certain nombre de dé de ce type
        /// </summary>
        /// <param name="nbDicesToAdd">nombre de dés à ajouter</param>
        public void AddDices(int nbDicesToAdd)
        {
            if(nbDicesToAdd <=0)
                throw new ArgumentOutOfRangeException(nameof(nbDicesToAdd), "Le nombre de dés à ajouter doit être suppérieur à 0");
            this.NbDices += nbDicesToAdd;
        }

        /// <summary>
        /// Retire un certain nombre de dé de ce type
        /// </summary>
        /// <param name="nbDicesToRm">nombre de dé à retirer</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveDices(int nbDicesToRm)
        {
            if (nbDicesToRm <= 0)
                throw new ArgumentOutOfRangeException(nameof(nbDicesToRm), "Le nombre de dés à retirer doit être suppérieur à 0");
            this.NbDices -= nbDicesToRm;
        }


        /// <summary>
        /// Egaux si même nombre de dé et même prototype
        /// </summary>
        /// <param name="obj">objet à comparer</param>
        /// <returns>true si égaux false sinon</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            if (!this.GetType().Equals(obj.GetType())) return false;
            return this.Equals(obj as DiceType);
        }
        public bool Equals(DiceType other)
        {
            return this.NbDices == other.NbDices && this.Prototype.Equals(other.Prototype);
        }
        public override int GetHashCode()
        {
            return Prototype.GetHashCode() ^ NbDices;
        }

    }
}

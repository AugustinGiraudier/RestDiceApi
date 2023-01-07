using System;

namespace ModelAppLib
{
    /// <summary>
    /// Classe modélisant un type de face avec son prototype de référence ainsi que sa quantité
    /// </summary>
    public class DiceSideType : IEquatable<DiceSideType>
    {
        /// <summary>
        /// Nombre de face de ce type
        /// </summary>
        public int NbSide { get; private set; }

        public DiceSide Prototype { get; private set; }

        /// <summary>
        /// Construit un type de face avec un prototype et une quantité
        /// </summary>
        /// <param name="nbSide">Nombre d'occurence de ce type de face</param>
        /// <param name="prototype">Type de face</param>
        public DiceSideType(int nbSide, DiceSide prototype)
        {
            if (nbSide <= 0)
                throw new ArgumentOutOfRangeException(nameof(nbSide),"le nombre de face doit être suppérieur à 0");
            if (prototype == null)
                throw new ArgumentNullException(nameof(prototype));
            NbSide = nbSide;
            this.Prototype = prototype;
        }

        /// <summary>
        /// Ajoute des faces à ce type de face
        /// </summary>
        /// <param name="nbToAdd">Nombre de face à ajouter</param>
        public void AddSides(int nbToAdd)
        {
            if (nbToAdd <= 0)
                throw new ArgumentOutOfRangeException(nameof(nbToAdd), "le nombre de face à ajouter doit être suppérieur à 0");
            NbSide += nbToAdd;
        }


        /// <summary>
        /// Retire des faces à ce type de face
        /// </summary>
        /// <param name="nbToRm">Nombre de faces à retirer</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveSides(int nbToRm)
        {
            if (nbToRm <= 0)
                throw new ArgumentOutOfRangeException(nameof(nbToRm), "le nombre de face à retirer doit être suppérieur à 0");
            NbSide -= nbToRm;
        }

        /// <summary>
        /// Egaux si même prototype
        /// </summary>
        /// <param name="obj">objet à comparer</param>
        /// <returns>true si égaux false sinon</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            if (!this.GetType().Equals(obj.GetType())) return false;
            return this.Equals(obj as DiceSideType);
        }
        public bool Equals(DiceSideType other)
        {
            return this.NbSide == other.NbSide && this.Prototype.Equals(other.Prototype);
        }
        public override int GetHashCode()
        {
            return this.Prototype.GetHashCode() ^ NbSide;
        }
    }
}

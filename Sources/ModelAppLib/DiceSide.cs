using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModelAppLib
{
     /// <summary>
    /// Classe modélisant une face de dé
    /// </summary>
    public class DiceSide : IEquatable<DiceSide>
    {

        public long Id { get; set; }
        public String Image { get;}

        /// <summary>
        /// Construit une face de dé
        /// </summary>
        /// <param name="image">url de l'image de la face</param>
        public DiceSide(string image)
        {
            if(image == null)
                throw new ArgumentNullException(nameof(image), "l'image ne peut pas etre null");
            this.Image = image;
        }

        public bool Equals(DiceSide other)
        {
            return this.Image.Equals(other.Image);
        }

        /// <summary>
        /// Egaux si même image
        /// </summary>
        /// <param name="obj">objet à comparer</param>
        /// <returns>true si égaux false sinon</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            if (!this.GetType().Equals(obj.GetType())) return false;
            return this.Equals(obj as DiceSide);
        }

        public override int GetHashCode()
        {
            return Image.GetHashCode();
        }

    }
}

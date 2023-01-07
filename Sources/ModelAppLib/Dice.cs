using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelAppLib
{
    /// <summary>
    /// Classe modélisant un dé
    /// </summary>
    public class Dice : IEquatable<Dice>
    {
        public long Id { get; set; }

        private static ILogger<Dice> logger = LoggerFactory.Create(builder => builder.AddNLog()).CreateLogger<Dice>();
        
        private readonly List<DiceSideType> sidesTypes = new List<DiceSideType>();

        private readonly IRandomizer randomizer;

        public ReadOnlyCollection<DiceSideType> SideTypes => sidesTypes.AsReadOnly();
        

        /// <summary>
        /// Construit un dé avec la liste de ses types de faces
        /// </summary>
        /// <param name="sidesTypes">Liste des types de faces qui sera clonnée</param>
        public Dice(IRandomizer random, IEnumerable<DiceSideType> sidesTypes)
        {
            if(sidesTypes == null)
                throw new ArgumentNullException(nameof(sidesTypes));
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (!sidesTypes.Any())
                throw new ArgumentException("La liste des types de faces ne peut être vide", nameof(sidesTypes));
            
            logger.LogTrace("Dice created");

            randomizer = random;
            
            foreach (var sideType in sidesTypes)
            {
                if (sideType == null)
                    throw new ArgumentNullException(nameof(sidesTypes));
                this.AddSide(sideType);
            }
        }
        
        /// <summary>
        /// Construit un dé avec ses types de faces en parametre
        /// </summary>
        /// <param name="dstypes">types de face du dé</param>
        public Dice(IRandomizer random, params DiceSideType[] dstypes)
            : this(random, dstypes.AsEnumerable())
        {
        }

        /// <summary>
        /// Le nombre total de faces du dé
        /// </summary>
        /// <returns>le nombre total de faces du dé</returns>
        public int GetTotalSides()
        {
            logger.LogTrace("Dice total number of sides asked");
            int ret = 0;
            foreach(DiceSideType dst in sidesTypes)
            {
                ret += dst.NbSide;
            }
            return ret;
        }

        /// <summary>
        /// Permet de tirer une face aléatoire du dé
        /// </summary>
        /// <returns></returns>
        public DiceSide GetRandomSide()
        {
            int index = randomizer.GetRandomInt(0, GetTotalSides());
            return this[index];
        }

        /// <summary>
        /// Retourne la face du dé correspondant à l'index
        /// </summary>
        /// <param name="index">index de la face dans ce dé</param>
        /// <returns>la face pointée par l'index</returns>
        public DiceSide this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "L'index doit être positif");
                }
                if (sidesTypes.Count == 0 || index >= GetTotalSides())
                    return null;

                int idCpt = 0;
                DiceSideType dst;
                int DiceCpt = 0;

                do
                {
                    dst = sidesTypes[idCpt];
                    DiceCpt += dst.NbSide;
                    idCpt++;
                }
                while (DiceCpt <= index);

                return dst.Prototype;
            }
        }

        /// <summary>
        /// Ajoute un type de face au dé (additionne le nombre de face si déja existante)
        /// </summary>
        /// <param name="sideT">Type de face à ajouter</param>
        public void AddSide(DiceSideType sideT)
        {
            if (sideT == null)
                throw new ArgumentNullException(nameof(sideT));
            if (sidesTypes.Contains(sideT))
                sidesTypes.Find(x => x.Equals(sideT)).AddSides(sideT.NbSide);
            else
                sidesTypes.Add(sideT);
        }

        public void RemoveSideType(DiceSideType dst)
        {
            if (dst == null)
                throw new ArgumentNullException(nameof(dst), "le type de face ne peut etre null");


            var theDst = sidesTypes.Find(x => x.Prototype.Equals(dst.Prototype));
            if (theDst == null)
                throw new ArgumentException("le dé ne contient pas ce type de face...", nameof(dst));

            if (theDst.NbSide - dst.NbSide > 0)
                theDst.RemoveSides(dst.NbSide);
            else
                sidesTypes.Remove(theDst);
        }

        /// <summary>
        /// Egaux si mêmes types de faces
        /// </summary>
        /// <param name="obj">objet à comparer</param>
        /// <returns>true si égaux false sinon</returns>
        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            if (!this.GetType().Equals(obj.GetType())) return false;
            return this.Equals(obj as Dice);
        }

        public bool Equals(Dice other)
        {
            return this.SideTypes.SequenceEqual(other.SideTypes);
        }
        public override int GetHashCode()
        {
            int code = 1;
            foreach (DiceSideType dt in SideTypes)
                code ^= dt.GetHashCode();
            return code;
        }
    }
}


namespace ModelAppLib
{
    /// <summary>
    /// Interface gérant la récupération des données aléatoires utiles au projet
    /// </summary>
    public interface IRandomizer
    {
        public int GetRandomInt(int min, int max);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelAppLib
{
    /// <summary>
    /// Interface concernant la gestion des données du projet
    /// Elle implémente les méthodes CRUD pour le projet DiceLauncher
    /// </summary>
    public interface IDataManager
    {

        // ===================================================== //
        //      = SELECT =
        // ===================================================== //


        /// <summary>
        /// Permet de récupérer tous les dés
        /// </summary>
        /// <returns>IEnumerablee de tous les dés</returns>
        public Task<IEnumerable<Dice>> GetAllDices();

        /// <summary>
        /// Permet de récupérer toutes les faces
        /// </summary>
        /// <returns>IEnumerablee de toutes les faces</returns>
        public Task<IEnumerable<DiceSide>> GetAllSides();

        /// <summary>
        /// Permet de récupérer toutes les parties
        /// </summary>
        /// <returns>IEnumerablee des parties</returns>
        public Task<IEnumerable<Game>> GetAllGames();

        /// <summary>
        /// Permet de récupérer certains dés
        /// </summary>
        /// <param name="nb">Nombre de dés à récupérer</param>
        /// <param name="page">Numéro de la page (nombre de séquence de 'nb' éléments à skip)</param>
        /// <returns></returns>
        public Task<IEnumerable<Dice>> GetSomeDices(int nb, int page);

        /// <summary>
        /// Permet de récupérer certaines faces de dé
        /// </summary>
        /// <param name="nb">Nombre de faces à récupérer</param>
        /// <param name="page">Numéro de la page (nombre de séquence de 'nb' éléments à skip)</param>
        /// <returns></returns>
        public Task<IEnumerable<DiceSide>> GetSomeSides(int nb, int page);

        /// <summary>
        /// Permet de récupérer certaines parties
        /// </summary>
        /// <param name="nb">Nombre de parties à récupérer</param>
        /// <param name="page">Numéro de la page (nombre de séquence de 'nb' éléments à skip)</param>
        /// <returns></returns>
        public Task<IEnumerable<Game>> GetSomeGames(int nb, int page);


        // ===================================================== //
        //      = CREATE =
        // ===================================================== //


        /// <summary>
        /// Ajoute une partie à la base de données
        /// </summary>
        /// <param name="game">la partie</param>
        /// <returns></returns>
        public Task<bool> AddGame(Game game);

        /// <summary>
        /// Ajoute un dé à la base de données
        /// </summary>
        /// <param name="dice">le dé</param>
        /// <returns></returns>
        public Task<bool> AddDice(Dice dice);

        /// <summary>
        /// Ajoute une face à la base de données
        /// </summary>
        /// <param name="side">la face</param>
        /// <returns></returns>
        public Task<bool> AddSide(DiceSide side);


        // ===================================================== //
        //      = COUNT =
        // ===================================================== //


        /// <summary>
        /// Retourne le nombre de dés de la base
        /// </summary>
        /// <returns></returns>
        public Task<int> GetNbDice();

        /// <summary>
        /// Retourne le nombre de faces de dés de la base
        /// </summary>
        /// <returns></returns>
        public Task<int> GetNbSide();

        /// <summary>
        /// Retourne le nombre de parties de la base
        /// </summary>
        /// <returns></returns>
        public Task<int> GetNbGame();


        // ===================================================== //
        //      = REMOVE =
        // ===================================================== //


        /// <summary>
        /// Supprime le dé passé en paramètre de la base
        /// </summary>
        /// <param name="d">dé à supprimer</param>
        /// <returns></returns>
        public Task<bool> DeleteDice(Dice d);

        /// <summary>
        /// Supprime la face de la base
        /// </summary>
        /// <param name="ds">face à supprimer</param>
        /// <returns></returns>
        public Task<bool> DeleteSide(DiceSide ds);

        /// <summary>
        /// Supprime une partie de la base
        /// </summary>
        /// <param name="g">partie à supprimer</param>
        /// <returns></returns>
        public Task<bool> DeleteGame(Game g);

        // ===================================================== //
        //      = UPDATE =
        // ===================================================== //

        /// <summary>
        /// Ajoute des dés à une partie
        /// </summary>
        /// <param name="g">Partie</param>
        /// <param name="d">dé</param>
        /// <param name="nb">nombre de dé de ce type à ajouter</param>
        /// <returns></returns>
        public Task<bool> AddDiceToGame(Game g, Dice d, int nb = 1);

        /// <summary>
        /// Ajoute des faces à un dé
        /// </summary>
        /// <param name="d">dé</param>
        /// <param name="ds">face</param>
        /// <param name="nb">nombre de face de ce type à ajouter</param>
        /// <returns></returns>
        public Task<bool> AddSideToDice(Dice d, DiceSide ds, int nb = 1);

        /// <summary>
        /// Supprime des dés d'une partie
        /// </summary>
        /// <param name="g">Partie</param>
        /// <param name="d">Dé</param>
        /// <param name="nb">nombre de dé de ce type à supprimer</param>
        /// <returns></returns>
        public Task<bool> RemoveDiceFromGame(Game g, Dice d, int nb = 1);
        public Task<bool> RemoveSideFromDice(Dice d, DiceSide ds, int nb = 1);

    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelAppLib
{
    /// <summary>
    /// Facade du model du projet DiceLauncher
    /// Il rend accessible les actiions utilisateurs à effectuer sur le modèle
    /// </summary>
    public class ModelManager
    {
        private readonly IDataManager dataManager;

        public ModelManager(IDataManager dManager)
        {
            if (dManager == null)
                throw new ArgumentNullException(nameof(dManager), "le gestionnaire de données ne peut etre null");
            
            this.dataManager = dManager;
        }
        
        /// <summary>
        /// Ajoute un dé au stockage
        /// </summary>
        /// <param name="d">dé à ajouter</param>
        public async Task<bool> AddDice(Dice d) 
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d), "le dé ne peut etre null");
            return await dataManager.AddDice(d);
        }
        /// <summary>
        /// Ajoute une partie au stockage
        /// </summary>
        /// <param name="g">partie à ajouter</param>
        public async Task<bool> AddGame(Game g)
        {
            if(g == null)
                throw new ArgumentNullException(nameof(g), "la partie ne peut etre null");
            return await dataManager.AddGame(g);
        }

        /// <summary>
        /// Ajoute des dés à une partie
        /// </summary>
        /// <param name="g">partie</param>
        /// <param name="d">dé</param>
        /// <param name="nbToAdd">nombre à ajouter</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> AddDiceToGame(Game g, Dice d, int nbToAdd=1)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g), "la partie ne peut etre null");
            if (d == null)
                throw new ArgumentNullException(nameof(d), "le dé ne peut etre null");

            return await dataManager.AddDiceToGame(g, d, nbToAdd);
        }

        /// <summary>
        /// Ajoute des faces à un dé
        /// </summary>
        /// <param name="d">dé</param>
        /// <param name="ds">face</param>
        /// <param name="nbToAdd">nombre à ajouter</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> AddSideToDice(Dice d, DiceSide ds, int nbToAdd = 1)
        {
            if (ds == null)
                throw new ArgumentNullException(nameof(ds), "la face ne peut etre null");
            if (d == null)
                throw new ArgumentNullException(nameof(d), "le dé ne peut etre null");

            return await dataManager.AddSideToDice(d, ds, nbToAdd);
        }

        /// <summary>
        /// Retire des dés à une partie
        /// </summary>
        /// <param name="g">partie</param>
        /// <param name="d">dé</param>
        /// <param name="nbToRm">nombre à retirer</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> RemoveDiceFromGame(Game g, Dice d, int nbToRm = 1)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g), "la partie ne peut etre null");
            if (d == null)
                throw new ArgumentNullException(nameof(d), "le dé ne peut etre null");
            return await dataManager.RemoveDiceFromGame(g, d, nbToRm);
        }

        /// <summary>
        /// Retire des faces à un dé
        /// </summary>
        /// <param name="d">dé</param>
        /// <param name="ds">face</param>
        /// <param name="nbToRm">nombre à retirer</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> RemoveSideFromDice(Dice d, DiceSide ds, int nbToRm = 1)
        {
            if (ds == null)
                throw new ArgumentNullException(nameof(ds), "la face ne peut etre null");
            if (d == null)
                throw new ArgumentNullException(nameof(d), "le dé ne peut etre null");
            return await dataManager.RemoveSideFromDice(d, ds, nbToRm);
        }

        /// <summary>
        /// Retourne le nombre de dés du stockage
        /// </summary>
        /// <returns>le nombre de dés</returns>
        public async Task<int> GetDiceCount() { return await dataManager.GetNbDice(); }

        /// <summary>
        /// Retourne le nombre de faces de dé du stockage
        /// </summary>
        /// <returns>le nombre de faces</returns>
        public async Task<int> GetSideCount() { return await dataManager.GetNbSide(); }

        /// <summary>
        /// Retourne le nombre de parties du stockage
        /// </summary>
        /// <returns>nombre de parties</returns>
        public async Task<int> GetGameCount() { return await dataManager.GetNbGame(); }

        /// <summary>
        /// Retourne la IEnumerablee de toutes les faces de dés
        /// </summary>
        /// <returns>IEnumerablee des faces</returns>
        public async Task<IEnumerable<DiceSide>> GetAllSides() { return await dataManager.GetAllSides(); }
        
        /// <summary>
        /// Retourne la IEnumerablee de tous les dés
        /// </summary>
        /// <returns>IEnumerablee des dés</returns>
        public async Task<IEnumerable<Dice>> GetAllDices() { return await dataManager.GetAllDices(); }

        /// <summary>
        /// Retourne la IEnumerablee de toutes les parties
        /// </summary>
        /// <returns>IEnumerablee des parties</returns>
        public async Task<IEnumerable<Game>> GetAllGames() { return await dataManager.GetAllGames(); }

        /// <summary>
        /// Récupère un nombre de face avec un offset
        /// </summary>
        /// <param name="nb">nombre de face</param>
        /// <param name="page">offset (commence à 0)</param>
        /// <returns></returns>
        public async Task<IEnumerable<DiceSide>> GetSomeSides(int nb, int page) { return await dataManager.GetSomeSides(nb, page); }

        /// <summary>
        /// Récupère un nombre de dé avec un offset
        /// </summary>
        /// <param name="nb">nombre de dé</param>
        /// <param name="page">offset (commence à 0)</param>
        /// <returns></returns>
        public async Task<IEnumerable<Dice>> GetSomeDices(int nb, int page) { return await dataManager.GetSomeDices(nb, page); }

        /// <summary>
        /// Récupère un nombre de partie avec un offset
        /// </summary>
        /// <param name="nb">nombre de partie</param>
        /// <param name="page">offset (commence à 0)</param>
        /// <returns></returns>
        public async Task<IEnumerable<Game>> GetSomeGames(int nb, int page) { return await dataManager.GetSomeGames(nb, page); }

        /// <summary>
        /// Supprime un dé
        /// </summary>
        /// <param name="d">le dé</param>
        /// <returns></returns>
        public async Task<bool> RemoveDice(Dice d) { return await dataManager.DeleteDice(d); }

        /// <summary>
        /// Supprime une partie
        /// </summary>
        /// <param name="g">la partie</param>
        /// <returns></returns>
        public async Task<bool> RemoveGame(Game g) { return await dataManager.DeleteGame(g); }

    }
}

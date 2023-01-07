using ModelAppLib;
using NLog;
using StubEntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelApp
{
    /// <summary>
    /// Classe Constituant le test fonctionnel principal de l'application
    /// </summary>
    static class Program
    {

        private static string _step = "";

        static async Task Main(string[] args)
        {

            InitLogger();

            try
            {
                INFO("Lancement du programme");

                STEP("Création du linker");
                var linker = new StubedDatabaseLinker();
                TEST(!ReferenceEquals(linker, null));

                STEP("Création du manager");
                ModelManager manager = new ModelManager(linker);
                TEST(!ReferenceEquals(manager, null));

                STEP("Répuération des faces de la base");
                var sides = (await manager.GetAllSides()).ToList();
                TEST(sides.Any());

                STEP("Création de la liste des types de faces");
                var sideTypes = new List<DiceSideType>
                {
                    new DiceSideType(1, sides[0]),
                    new DiceSideType(1, sides[1]),
                    new DiceSideType(1, sides[5]),
                    new DiceSideType(1, sides[3]),
                    new DiceSideType(1, sides[2]),
                    new DiceSideType(1, sides[4]),
                    new DiceSideType(2, sides[6])
                };
                TEST(true);

                STEP("Création du dé");
                var d = new Dice(new SecureRandomizer(), sideTypes);
                TEST(!ReferenceEquals(d, null));

                STEP("Ajout du dé à la base");
                await manager.AddDice(d);
                TEST((await manager.GetAllDices()).Contains(d));

                const int NB_DICES = 4;

                STEP($"Création du type de dé (la partie contiendra {NB_DICES} fois ce dé)");
                var dt = new DiceType(NB_DICES, d);
                TEST(dt.NbDices == NB_DICES && dt.Prototype.Equals(d));

                STEP("Création de la partie");
                var g = new Game(dt);
                TEST(!ReferenceEquals(g, null));

                STEP("Ajout de la partie à la base");
                await manager.AddGame(g);
                TEST((await manager.GetAllGames()).Contains(g));

                STEP("Lancement des dés de la partie");
                var results = g.LaunchDices();
                TEST(results.Count() == NB_DICES);
                foreach (var res in results)
                    ITEM($"Image résultat : '{res.Image}'");

                STEP("Création de la liste des types de faces pour le second dé");
                var sideTypes2 = new List<DiceSideType>
                {
                    new DiceSideType(3, sides[0]),
                    new DiceSideType(3, sides[5]),
                };
                TEST(true);

                STEP("Création du second dé");
                var d2 = new Dice(new SecureRandomizer(), sideTypes2);
                TEST(!ReferenceEquals(d2, null));

                STEP("Ajout du second dé à la base");
                await manager.AddDice(d2);
                TEST((await manager.GetAllDices()).Contains(d2));

                STEP("Ajout du second dé à la partie déja créée");
                await manager.AddDiceToGame(g, d2);
                TEST(g.Dices.Count == 2);

                STEP("Lancement des dés de la partie");
                var results2 = g.LaunchDices();
                TEST(results2.Count() == NB_DICES + 1);
                foreach (var res in results2)
                    ITEM($"Image résultat : '{res.Image}'");

                STEP("Suppression du premier dé de la partie");
                await manager.RemoveDiceFromGame(g, d, NB_DICES);
                TEST(g.Dices.FirstOrDefault(di => di.Equals(d)) == null);

                STEP("Lancement des dés de la partie");
                var results3 = g.LaunchDices();
                TEST(results3.Count() == 1);
                foreach (var res in results3)
                    ITEM($"Image résultat : '{res.Image}'");

                STEP("Suppression d'un des 2 types de face du dé restant dans la partie");
                await manager.RemoveSideFromDice(d2, sides[5], 3);
                TEST(d2.SideTypes.Count == 1);

                STEP("Lancement des dés de la partie");
                g = (await manager.GetAllGames()).First();
                var results4 = g.LaunchDices();
                TEST(results4.Count() == 1);
                foreach (var res in results4)
                    ITEM($"Image résultat : '{res.Image}'");

                STEP("Suppression de la partie de la base");
                await manager.RemoveGame(g);
                TEST(!((await manager.GetAllGames()).Contains(g)));

                STEP("Suppression du dé de la base");
                await manager.RemoveDice(d);
                TEST(!((await manager.GetAllDices()).Contains(d)));
                
                STEP("Suppression du second dé de la base");
                await manager.RemoveDice(d2);
                TEST(!((await manager.GetAllDices()).Contains(d2)));


                INFO("Fin du programme");
            }
            catch (Exception e)
            {
                FAILURE(e.Message);
            }

        }

        private static void STEP(string step)
        {
            _step = step;
        }

        private static void SUCCESS()
        {
            Console.WriteLine(      $"[+] SUCCESS   |   {_step}");
        }
        private static void FAILURE(string msg = null)
        {
            Console.WriteLine(      $"[X] ERREUR    |   {_step}");
            if (!ReferenceEquals(msg, null))
                Console.WriteLine(  $"[#] EXCEPTION |   {msg}");
            System.Environment.Exit(1);
        }
        private static void INFO(string info)
        {
            Console.WriteLine(      $"[i] INFO      |   {info}");
        }
        private static void ITEM(string item)
        {
            Console.WriteLine(      $"[-] ITEM      |      * {item}");
        }


        private static void TEST(bool testResult)
        {
            if (testResult)
                SUCCESS();
            else
                FAILURE();
        }
        private static void InitLogger()
        {
            LogManager.Setup().LoadConfiguration(builder => {
                builder.ForLogger().FilterMinLevel(LogLevel.Error).WriteToConsole();
            });
        }

    }
}

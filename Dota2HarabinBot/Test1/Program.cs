using Dota2GSI;
using System;
using Dota2GSI.Nodes;

namespace DOTA2GSI_sample
{
    static class Program
    {
        

        static void Main(string[] args)
        {
            var gsl = new GameStateListener(4000);
            gsl.NewGameState += new NewGameStateHandler(OnNewGameState);

            if (!gsl.Start())
            {
                Console.WriteLine("GameStateListener could not start. Try running this program as Administrator.\r\nExiting.");
                Environment.Exit(0);
            }
            Console.WriteLine("Listening for game integration calls...");
        }

        static void OnNewGameState(GameState gs)
        {
            if (gs.Map.GameState == DOTA_GameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS)
            {
                if (gs.Added.Items.CountInventory > gs.Items.CountInventory)
                {
                    Console.WriteLine("You bought an item");
                }

                if (!gs.Map.IsDaytime || gs.Map.IsNightstalker_Night)
                {
                    Console.WriteLine("It is night time");
                }
            }
        }
    }
}

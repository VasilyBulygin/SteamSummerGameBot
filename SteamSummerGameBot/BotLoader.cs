using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SteamSummerGameBot
{
    public static class BotLoader
    {
        public static List<GameBot> LoadBotsFromFile()
        {
            var result = new List<GameBot>();
            if (!File.Exists("accounts.txt")) return result;
            using (var reader = new StreamReader("accounts.txt"))
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var bot = new GameBot();
                        bot.LoadFromString(line);
                        result.Add(bot);
                    }
                }
                catch (Exception)
                {

                }
            }
            return result;
        }

        public static void SaveBotsToFile(List<GameBot> bots)
        {
            if (bots.Count == 0) return;
            var fileString = bots.Aggregate(string.Empty, (current, bot) => current + (bot.SaveToString() + Environment.NewLine));
            File.WriteAllText("accounts.txt", fileString);
        }
    }
}

using CookieClicker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieClicker.Persistence
{
    class DataAccess
    {
        public async Task Save(string path, Game gameState)
        {
            StringBuilder sb = new StringBuilder();
            //write data
            sb.AppendLine(gameState.GetCurrentTime().ToString());
            gameState.SetLastTimeActive(gameState.GetCurrentTime());

            sb.Append(gameState.GetMoney()+" ");
            sb.Append(gameState.GetIncRate() + " ");
            sb.Append(gameState.GetUpgrades() + " ");

            await System.IO.File.WriteAllTextAsync(path, sb.ToString());
        }

        public async Task<Game> Load(string fileName, Game gameState)
        {
            //string[] lastOnline = await File.ReadLinesAsync(fileName);

            string data = await File.ReadAllTextAsync(fileName);

            string[] dataLines = data.Split("\n");

            string[] sliced = dataLines[1].Split(" ");

            DateTime lastActive =  DateTime.Parse(dataLines[0]);

            double money;
            double.TryParse(sliced[0], out money);

            double income;
            double.TryParse(sliced[1], out income);

            int upgrades;
            int.TryParse(sliced[2], out upgrades);

            gameState.SetMoney(money);
            gameState.SetIncome(income);
            gameState.SetUpgrades(upgrades);
            gameState.SetLastTimeActive(lastActive);

            return gameState;
        }
    }
}

using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackhole.Maui.Persistence
{
    public class BlackholeStore : IStore
    {
        /// <summary>
        /// Fájlok lekérdezése.
        /// </summary>
        /// <returns>A fájlok listja.</returns>
        public async Task<IEnumerable<String>> GetFilesAsync()
        {
            return await Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
                .Select(Path.GetFileName)
                .Where(name => name?.EndsWith(".bgf") ?? false)
                .OfType<String>());
        }

        /// <summary>
        /// Módosítás idejének lekérdezése.
        /// </summary>
        /// <param name="name">A fájl neve.</param>
        /// <returns>Az utols módosítás ideje.</returns>
        public async Task<DateTime> GetModifiedTimeAsync(String name)
        {
            var info = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, name));

            return await Task.Run(() => info.LastWriteTime);
        }
        //nemtudom hogy e helyett kell e valami fuggveny aminel nev szerint kerdezzuk le oket
    }
}

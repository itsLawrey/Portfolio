using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Model
{
    public class StoredGameModel
    {
        /// <summary>
        /// Név lekérdezése, vagy beállítása.
        /// </summary>
        public String Name { get; set; } = String.Empty;
        public DateTime Modified { get; set; }
    }
}

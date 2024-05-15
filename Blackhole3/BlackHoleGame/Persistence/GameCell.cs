using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackHoleGame.Model;

namespace BlackHoleGame.Persistence
{
    public enum CellState { None, Blue, Red, Black }


    public class GameCell
    {
        public CellState CellState { get; set; }
        public bool Selected { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public GameCell(int x, int y, CellState cellState)
        {
            X = x;
            Y = y;
            CellState = cellState;
            Selected = false;
        }
    }
}

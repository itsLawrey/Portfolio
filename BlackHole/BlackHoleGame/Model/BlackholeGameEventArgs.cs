using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Model
{
    public class BlackholeGameEventArgs : EventArgs
    {
        private CellState _winner;

        public CellState Winner { get { return _winner; } }


        public BlackholeGameEventArgs(CellState cs)
        {
            _winner = cs;
        }
    }
}

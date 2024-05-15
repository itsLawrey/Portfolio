using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Model
{
    public class BlackholePlayerEventArgs : EventArgs
    {
        private CellState _player;
        public CellState Player { get { return _player; } }

        public BlackholePlayerEventArgs(CellState cs)
        {
            _player = cs;
        }
    }
}

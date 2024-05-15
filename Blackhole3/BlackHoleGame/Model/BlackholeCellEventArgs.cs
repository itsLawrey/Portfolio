using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Model
{
    public class BlackholeCellEventArgs : EventArgs
    {

        private CellState _changedCellState;
        private int _changedRow;
        private int _changedColumn;
        private bool _changedCellSelected;




        public CellState CellState { get { return _changedCellState; } }
        public int Row { get { return _changedRow; } }
        public int Column { get { return _changedColumn; } }
        public bool Selected { get { return _changedCellSelected; } }//ezek csak azert vannak hogy az event adatait kiolvassuk




        public BlackholeCellEventArgs(int row, int col, CellState cellState, bool selected)
        {
            _changedCellState = cellState;
            _changedRow = row;
            _changedColumn = col;
            _changedCellSelected = selected;
        }
    }
}

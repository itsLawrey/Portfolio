using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Persistence
{
    public class BlackholeTable
    {

        #region fields

        private int _tableSize;
        private GameCell[,] _gameTable;



        #endregion

        #region properties
        public int TableSize
        {
            get => _tableSize;
            set
            {
                _tableSize = value;
                _gameTable = new GameCell[value, value];
            }
        }
        public CellState Player { get; set; }

        #endregion

        #region constructors

        public BlackholeTable()
        {
            _gameTable = new GameCell[0,0];
        }
        public BlackholeTable(int size)
        {

            _tableSize = size;
            _gameTable = new GameCell[size, size];

            

        }

        #endregion

        #region public methods

        
        public bool AnySelected()
        {
            foreach (var cell in _gameTable)
            {
                if (cell.Selected) return true;
            }
            return false;
        }
        public GameCell? SelectedCell()
        {
            foreach (var cell in _gameTable)
            {
                if (cell.Selected) return cell;//legelso selected, azaz az egyetlen ilyen :)
            }
            return null;
        }


        public int PlayerShipCount(CellState color)
        {
            int sum = 0;
            foreach (GameCell cell in _gameTable)
            {
                if(cell.CellState == color) sum++;
            }
            return sum;
        }

        public void GenerateCell(int i, int j, CellState cs)
        {
            _gameTable[i, j] = new GameCell(i,j,cs);
        }



        public CellState GetGameCellState(int i, int j)
        {
            return _gameTable[i, j].CellState;
        }
        public bool GetGameCellSelected(int i, int j)
        {
            return _gameTable[i, j].Selected;
        }



        public void UpdateCell(int x, int y, CellState cs)
        {
            _gameTable[x, y].CellState = cs;
            if (cs == CellState.None)
            {
                _gameTable[x, y].Selected = false;
            }
        }

        public void SelectCell(int x, int y, bool selected)
        {
            _gameTable[x, y].Selected = selected;
        }



        

        
        #endregion

        #region private methods

        #endregion
    }
}

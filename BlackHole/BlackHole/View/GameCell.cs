using BlackHoleGame.Model;
using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackHole.View
{
    public partial class GameCell : UserControl
    {

        private bool _selected = false;
        private CellState _cellState;
        private readonly int _x;
        private readonly int _y;

        public CellState CellState { get { return _cellState; } set { SetBackground(value, Selected); _cellState = value; } }
        public bool Selected { get { return _selected; } set { SetBackground(CellState, value); _selected = value; } }//a cellstate onmagat megcsinalja, marmint a kepet
        public GameCell(int x, int y)
        {
            _x = x;
            _y = y;
            InitializeComponent();
        }

        private void SetBackground(CellState cs, bool selected)
        {
            switch (cs)
            {
                case CellState.None:
                    btnGameField.BackgroundImage = null;
                    break;
                case CellState.Blue:
                    btnGameField.BackgroundImage = selected ? Resources.BlueSelected : Resources.Blue;
                    break;
                case CellState.Red:
                    btnGameField.BackgroundImage = selected ? Resources.RedSelected : Resources.Red;
                    break;
                case CellState.Black:
                    btnGameField.BackgroundImage = Resources.Black;
                    break;
                default:
                    btnGameField.BackgroundImage = null;
                    break;
            }
        }

        private void btnGameField_Click(object sender, EventArgs e)
        {
            
            OnCellStateChanged();
            
        }

        public event EventHandler<BlackholeCellEventArgs>? CellChanged;
        //ezt azert valtjuk ki, h a foablakban tudjuk h melyik gomb volt megnyomva,azt ki lehet olvasni az eventargsbol..
        //ebbol a foablak a modellben az azon a koordinatan levo cellat updaetli

        private void OnCellStateChanged()
        {
            CellChanged?.Invoke(this, new BlackholeCellEventArgs(_x, _y, _cellState, _selected));
        }

    }
}

using BlackHoleGame.Persistence;

namespace Blackhole.Maui.ViewModel
{
    public class GameCellButton : ViewModelBase
    {
        private bool _selected = false;
        private CellState _cellState;
        private string _bg = "";

        public string ButtonText { get; set; }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;

                    ButtonText = _selected ? "S" : string.Empty;
                    OnPropertyChanged(nameof(ButtonText));
                }
            }
        }

        public CellState CellState
        {
            get { return _cellState; }
            set
            {
                if (_cellState != value)
                {
                    _cellState = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Bg
        {
            get
            {
                return _bg;
            }
            set
            {
                if (_bg != value)
                {
                    _bg = value;
                    OnPropertyChanged();
                }
            }
        }

        

        public Tuple<int, int> Number => new(X, Y);

        public int Index { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public DelegateCommand? ButtonPressedCommand { get; set; }

        public GameCellButton(int index, int x, int y, CellState cellState, bool selected, DelegateCommand? buttonPressedCommand)
        {
            Selected = selected;
            CellState = cellState;
            X = x;
            Y = y;
            ButtonPressedCommand = buttonPressedCommand;
            Index = index;
            Bg = "White";
            ButtonText = string.Empty;

        }
    }
}

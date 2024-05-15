using BlackHoleGame.Model;
using BlackHoleGame.Persistence;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Blackhole.Maui.ViewModel;

namespace Blackhole.Maui.ViewModel
{
    public class BlackHoleViewModel : ViewModelBase
    {
        private BlackholeGameModel _model; // modell

        public ObservableCollection<GameCellButton> Cells { get; set; }
        public int Size => _model.TableSize;

        public string BottomText { get; set; }

        public CellState Player { get; set; }

        public bool GameOver { get; set; }

        #region Events


        public event EventHandler<NewGameEventArgs>? NewGame;

        //public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? MenuPage;

        public event EventHandler? NewGamePage;
        public event EventHandler? SaveGamePage;
        public event EventHandler? LoadGamePage;

        #endregion

        public DelegateCommand RefreshCommand { get; private set; }


        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand? LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand MenuCommand { get; private set; }

        public DelegateCommand NewGamePageCommand { get; private set; }
        public DelegateCommand SaveGamePageCommand { get; private set; }
        public DelegateCommand LoadGamePageCommand { get; private set; }


        //new stuff
        /// <summary>
        /// Segédproperty a tábla méretezéséhez
        /// </summary>
        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), Size).ToArray());
        }


        /// <summary>
        /// Segédproperty a tábla méretezéséhez
        /// </summary>
        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), Size).ToArray());
        }









        public BlackHoleViewModel(BlackholeGameModel model)
        {
            _model = model;
            _model.CellChanged += new EventHandler<BlackholeCellEventArgs>(Model_CellChanged);
            _model.PlayerChanged += new EventHandler<BlackholePlayerEventArgs>(Model_PlayerChanged);
            _model.GameOver += new EventHandler<BlackholeGameEventArgs>(Model_GameOver);

            NewGameCommand = new DelegateCommand(param => OnNewGame(param??5));
            //LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());


            RefreshCommand = new DelegateCommand(param => OnRefresh());


            MenuCommand = new DelegateCommand(param => OnMenuPage());
            NewGamePageCommand = new DelegateCommand(param => OnNewGamePage());
            SaveGamePageCommand = new DelegateCommand(param => OnSaveGamePage());
            LoadGamePageCommand = new DelegateCommand(param => OnLoadGamePage());

            BottomText = "Game not started";
            GameOver = false;
            OnPropertyChanged(nameof(GameOver));
            OnPropertyChanged(nameof(BottomText));

            Cells = new ObservableCollection<GameCellButton>{new GameCellButton(0, 0, 0, CellState.None, false, null)};
        }

        private void OnRefresh()//temp shit
        {
            GenerateGameCells(Size);
            OnPropertyChanged(nameof(GameTableRows));
            OnPropertyChanged(nameof(GameTableColumns));
        }

        private void OnLoadGamePage()
        {
            LoadGamePage?.Invoke(this, EventArgs.Empty);//jelezzuk es kene reagalni az appshellben 
        }

        private void OnSaveGamePage()
        {
            SaveGamePage?.Invoke(this, EventArgs.Empty);//jelezzuk es kene reagalni az appshellben 
        }

        private void OnNewGamePage()
        {
            NewGamePage?.Invoke(this, EventArgs.Empty);//jelezzuk es kene reagalni az appshellben 
        }

        private void OnNewGame(object gameType)
        {
            int size = Convert.ToInt32(gameType);
            NewGame?.Invoke(this, new NewGameEventArgs(size));
            _model.NewGame(size);
            GenerateGameCells(size);


            OnPropertyChanged(nameof(GameTableRows));
            OnPropertyChanged(nameof(GameTableColumns));

        }

        private void Model_GameOver(object? sender, BlackholeGameEventArgs e)
        {
            //DeclareWinner(e.Winner);

            GameOver = true;
            OnPropertyChanged(nameof(GameOver));
        }

        private void Model_PlayerChanged(object? sender, BlackholePlayerEventArgs e)
        {
            Player = e.Player;
            BottomText = e.Player.ToString();

            OnPropertyChanged(nameof(BottomText));
            OnPropertyChanged(nameof(Player));

            //RefreshTable();
            //OnPropertyChanged(nameof(GameTableRows));
            //OnPropertyChanged(nameof(GameTableColumns));
        }

        private void Model_CellChanged(object? sender, BlackholeCellEventArgs e)
        {
            if (Cells[e.Row * _model.TableSize + e.Column].Selected != e.Selected)
            {
                Cells[e.Row * _model.TableSize + e.Column].Selected = e.Selected;
                //OnPropertyChanged(nameof(Cells));
            }

            if (Cells[e.Row * _model.TableSize + e.Column].CellState != e.CellState)
            {
                Cells[e.Row * _model.TableSize + e.Column].CellState = e.CellState;
                //OnPropertyChanged(nameof(Cells));
            }
            RefreshTable();

        }

        public void GenerateGameCells(int size)
        {
            Cells = new ObservableCollection<GameCellButton>();
            for (Int32 i = 0; i < size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < size; j++)
                {
                    Cells.Add(new GameCellButton(i * size + j, i, j, CellState.None, false, new DelegateCommand(param => CellClicked(param))));//a cellclicked az o buttonpressed commandja, cska kivancsi vagyok ez igy mukodik e
                }
            }

            RefreshTable();
        }

        private void RefreshTable()
        {
            foreach (var cell in Cells)
            {

                cell.CellState = _model.Table.GetGameCellState(cell.X, cell.Y);
                cell.Selected = _model.Table.GetGameCellSelected(cell.X, cell.Y);
                switch (cell.CellState)
                {
                    case CellState.None:
                        cell.Bg = "White";
                        break;
                    case CellState.Blue:
                        cell.Bg = "Blue";
                        break;
                    case CellState.Red:
                        cell.Bg= "Red";
                        break;
                    case CellState.Black:
                        cell.Bg = "Black";
                        break;
                    default:
                        cell.Bg = "White";
                        break;
                }


            }

            OnPropertyChanged(nameof(Size));//xd
            OnPropertyChanged(nameof(Cells));
        }

        private void CellClicked(object? param)
        {
            
            if (param != null)
            {
                Tuple<int, int> coords = (Tuple<int, int>)param;
                _model.HandleGameCellClicked(coords.Item1, coords.Item2);
            }
            RefreshTable();
            
        }

        //private async void DeclareWinner(CellState winner)
        //{

        //    győzelemtől függő üzenet megjelenítése
        //    await DisplayAlert("Blackhole game", $"Congratulations, {e.Winner} won!", "OK");

        //    BottomText = "GAME OVER!";

        //    OnPropertyChanged(nameof(BottomText));

        //}

        #region Event methods


        //private void OnLoadGame()
        //{
        //    //LoadGame?.Invoke(this, EventArgs.Empty);//ures metodus


        //    //GenerateGameCells(_model.TableSize);
            

        //    //OnPropertyChanged(nameof(GameTableRows));
        //    //OnPropertyChanged(nameof(GameTableColumns));
        //    //cells 
            

        //}

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnMenuPage()
        {
            MenuPage?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        


    }
}

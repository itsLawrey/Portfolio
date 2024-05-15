using BlackHoleGame.Model;
using BlackHoleGame.Persistence;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using ViewModel;

namespace BHWPF.ViewModel
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


        public event EventHandler? LoadGame;


        public event EventHandler? SaveGame;


        public event EventHandler? ExitGame;

        #endregion

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }
        public BlackHoleViewModel(BlackholeGameModel model)
        {
            _model = model;
            _model.CellChanged += new EventHandler<BlackholeCellEventArgs>(Model_CellChanged);
            _model.PlayerChanged += new EventHandler<BlackholePlayerEventArgs>(Model_PlayerChanged);
            _model.GameOver += new EventHandler<BlackholeGameEventArgs>(Model_GameOver);

            NewGameCommand = new DelegateCommand(param => OnNewGame(param??5));
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            BottomText = "Game not started";
            GameOver = false;
            OnPropertyChanged(nameof(GameOver));
            OnPropertyChanged(nameof(BottomText));

            Cells = new ObservableCollection<GameCellButton>{new GameCellButton(0, 0, 0, CellState.None, false, null)};
        }

        private void OnNewGame(object gameType)
        {
            int size = Convert.ToInt32(gameType);
            NewGame?.Invoke(this, new NewGameEventArgs(size));
            GenerateGameCells(size);
        }

        private void Model_GameOver(object? sender, BlackholeGameEventArgs e)
        {
            DeclareWinner(e.Winner);

            GameOver = true;
            OnPropertyChanged(nameof(GameOver));
        }

        private void Model_PlayerChanged(object? sender, BlackholePlayerEventArgs e)
        {
            Player = e.Player;
            BottomText = e.Player.ToString();

            OnPropertyChanged(nameof(BottomText));
            OnPropertyChanged(nameof(Player));
        }

        private void Model_CellChanged(object? sender, BlackholeCellEventArgs e)
        {
            if (Cells[e.Row * _model.TableSize + e.Column].Selected != e.Selected)
            {
                Cells[e.Row * _model.TableSize + e.Column].Selected = e.Selected;
                OnPropertyChanged(nameof(Cells));
            }

            if (Cells[e.Row * _model.TableSize + e.Column].CellState != e.CellState)
            {
                Cells[e.Row * _model.TableSize + e.Column].CellState = e.CellState;
                OnPropertyChanged(nameof(Cells));
            }

        }

        public void GenerateGameCells(int size)
        {
            Cells = new ObservableCollection<GameCellButton>();
            for (Int32 i = 0; i < size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < size; j++)
                {
                    Cells.Add(new GameCellButton(i * size + j, i, j, CellState.None, false, new DelegateCommand(param => CellClicked(param))));
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
        }

        private void DeclareWinner(CellState winner)
        {
            MessageBox.Show($"{winner} player won!\nYou can start a new game, save or load from the menu.", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);

            BottomText = "GAME OVER!";

            OnPropertyChanged(nameof(BottomText));

        }

        #region Event methods


        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);

        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion


    }
}

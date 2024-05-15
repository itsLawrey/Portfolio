using BHWPF.View;
using BHWPF.ViewModel;
using BlackHoleGame.Model;
using BlackHoleGame.Persistence;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using ViewModel;

namespace BHWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private BlackholeGameModel _model = null!;
        private BlackHoleViewModel _viewModel = null!;
        private GameWindow _view = null!;
        private bool _gameSaved = true;
        
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new BlackholeGameModel(new BlackholeFileDataAccess());
            //_model.GameOver += new EventHandler<BlackholeGameEventArgs>(Model_GameOver);
            

            // nézemodell létrehozása
            _viewModel = new BlackHoleViewModel(_model);
            _viewModel.NewGame += new EventHandler<NewGameEventArgs>(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new GameWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            await SaveGame();
        }

        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            if (!_gameSaved)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save the current game?", "Save current?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await SaveGame();
                }
            }
            await LoadGame();
        }

        private async void View_Closing(object? sender, CancelEventArgs e)
        {
            if (!_gameSaved)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save before quitting?\nAll progress will be lost.", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await SaveGame();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; // Cancel the closing event to prevent the window from closing
                }
            }
        }

        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
           _view.Close();
        }

        private void ViewModel_NewGame(object? sender, NewGameEventArgs e)
        {
            _model.NewGame(e.Size);
            _gameSaved= false;
        }

        private async Task SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Blackhole Game";
            saveFileDialog.Filter = "Blackhole Game File|*.bgf";

            if (saveFileDialog.ShowDialog() == true) // 'true' means the user clicked 'OK'
            {
                try
                {
                    // Game saving logic
                    await _model.SaveGameAsync(saveFileDialog.FileName);
                    _gameSaved = true;
                    MessageBox.Show("Game successfully saved.", "Success!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (BlackholeDataException)
                {
                    MessageBox.Show("Unable to save." + Environment.NewLine + "Inaccessible path.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task LoadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Blackhole Game";
            openFileDialog.Filter = "Blackhole Game File|*.bgf";

            if (openFileDialog.ShowDialog() == true) // 'true' means the user clicked 'OK'
            {
                try
                {
                    // Game loading logic
                    await _model.LoadGameAsync(openFileDialog.FileName);
                    _gameSaved = false;
                    _viewModel.GenerateGameCells(_model.TableSize);
                }
                catch (BlackholeDataException)
                {
                    MessageBox.Show("Unable to load." + Environment.NewLine + "Invalid path or non-existent file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

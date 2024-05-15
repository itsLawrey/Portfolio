
using Blackhole.Maui.View;
using Blackhole.Maui.ViewModel;
using BlackHoleGame.Model;
using BlackHoleGame.Persistence;

namespace Blackhole.Maui
{
    public partial class AppShell : Shell
    {
        #region Fields

        private IBlackholeDataAccess _blackholeDataAccess;
        private readonly BlackholeGameModel _blackholeGameModel;
        private readonly BlackHoleViewModel _blackholeViewModel;


        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        #endregion

        #region Application methods

        public AppShell(IStore blackholeStore,
            IBlackholeDataAccess blackholeDataAccess,
            BlackholeGameModel blackholeGameModel,
            BlackHoleViewModel blackholeViewModel)
        {
            InitializeComponent();

            // játék összeállítása
            _store = blackholeStore;
            _blackholeDataAccess = blackholeDataAccess;
            _blackholeGameModel = blackholeGameModel;
            _blackholeViewModel = blackholeViewModel;

            _blackholeGameModel.GameOver += BlackholeGameModel_GameOver;


            _blackholeViewModel.NewGame += BlackholeViewModel_NewGame;


            _blackholeViewModel.NewGamePage += BlackholeViewModel_NewGamePage;
            _blackholeViewModel.LoadGamePage += BlackholeViewModel_LoadGamePage;
            _blackholeViewModel.SaveGamePage += BlackholeViewModel_SaveGamePage;
            _blackholeViewModel.MenuPage += BlackholeViewModel_Menu;


            

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        



        #endregion

        #region Model event handlers

        /// <summary>
        ///     Játék végének eseménykezelője.
        /// </summary>
        private async void BlackholeGameModel_GameOver(object? sender, BlackholeGameEventArgs e)
        {
            

            if (e.Winner != CellState.None)
            {
                // győzelemtől függő üzenet megjelenítése
                await DisplayAlert("Blackhole game",$"Congratulations, {e.Winner} won!","OK");
            }
        }

        #endregion

        #region ViewModel event handlers

        private async void BlackholeViewModel_SaveGamePage(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        private async void BlackholeViewModel_LoadGamePage(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        /// <summary>
        ///     Új játék indításának eseménykezelője.
        /// </summary>
        private async void BlackholeViewModel_NewGamePage(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewGamePage
            {
                BindingContext = _blackholeViewModel//eddig gamemodel volt
            }) ; // átnavigálunk a lapra
        }

        private async void BlackholeViewModel_NewGame(object? sender, NewGameEventArgs e)
        {

            await Navigation.PopToRootAsync();

            //_blackholeGameModel.NewGame(e.Size);
        }

        

        

        private async void BlackholeViewModel_Menu(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage
            {
                BindingContext = _blackholeViewModel
            }); // átnavigálunk a menu lapra
        }


        /// <summary>
        ///     Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _blackholeGameModel.LoadGameAsync(e.Name);

                // sikeres betöltés
                await Navigation.PopAsync(); // visszanavigálunk a játék táblára.
                _blackholeViewModel.RefreshCommand.Execute(null);
                await DisplayAlert("Blackhole Game", "Successfully loaded.", "OK");

            }
            catch
            {
                await DisplayAlert("Blackhole game", "Unsuccessful loading.", "OK");
            }
        }

        /// <summary>
        ///     Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk
            

            try
            {
                // elmentjük a játékot
                await _blackholeGameModel.SaveGameAsync(e.Name);
                await DisplayAlert("Blackhole Game", "Successfully saved.", "OK");
            }
            catch
            {
                await DisplayAlert("Blackhole game", "Unsuccessful saving.", "OK");
            }
        }
        

        #endregion
    }
}

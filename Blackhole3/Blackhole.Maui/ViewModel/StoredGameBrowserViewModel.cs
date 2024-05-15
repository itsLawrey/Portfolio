using BlackHoleGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackhole.Maui.ViewModel
{
    public class StoredGameBrowserViewModel
    {
        private StoredGameBrowserModel _model;

        /// <summary>
        /// Betöltés eseménye.
        /// </summary>
        public event EventHandler<StoredGameEventArgs>? GameLoading;

        /// <summary>
        /// Mentés eseménye.
        /// </summary>
        public event EventHandler<StoredGameEventArgs>? GameSaving;

        /// <summary>
        /// Új játék parancsa.
        /// </summary>
        public DelegateCommand NewSaveCommand { get; private set; }

        /// <summary>
        /// Tárolt játékok gyűjteménye.
        /// </summary>
        public ObservableCollection<StoredGameViewModel> StoredGames { get; private set; }

        /// <summary>
        /// Tárolt játékkezelő nézetmodelljének példányosítása.
        /// </summary>
        /// <param name="model">A modell.</param>
        public StoredGameBrowserViewModel(StoredGameBrowserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            _model.StoreChanged += new EventHandler(Model_StoreChanged);

            NewSaveCommand = new DelegateCommand(param =>
            {
                string? fileName = Path.GetFileNameWithoutExtension(param?.ToString()?.Trim());
                if (!String.IsNullOrEmpty(fileName))
                {
                    fileName += ".bgf";
                    OnGameSaving(fileName);
                }
            });
            StoredGames = new ObservableCollection<StoredGameViewModel>();
            UpdateStoredGames();
        }

        /// <summary>
        /// Tárolt játékok frissítése.
        /// </summary>
        private void UpdateStoredGames()
        {
            StoredGames.Clear();

            foreach (StoredGameModel item in _model.StoredGames)
            {
                StoredGames.Add(new StoredGameViewModel
                {
                    Name = item.Name,
                    Modified = item.Modified,
                    LoadGameCommand = new DelegateCommand(param => OnGameLoading(param?.ToString() ?? "")),
                    SaveGameCommand = new DelegateCommand(param => OnGameSaving(param?.ToString() ?? ""))
                });
            }
        }

        private void Model_StoreChanged(object? sender, EventArgs e)
        {
            UpdateStoredGames();
        }

        private void OnGameLoading(String name)
        {
            GameLoading?.Invoke(this, new StoredGameEventArgs { Name = name });
        }

        private void OnGameSaving(String name)
        {
            GameSaving?.Invoke(this, new StoredGameEventArgs { Name = name });
        }
    }
}

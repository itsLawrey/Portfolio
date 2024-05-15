using Blackhole.Maui.Persistence;
using Blackhole.Maui.ViewModel;
using BlackHoleGame.Model;
using BlackHoleGame.Persistence;

namespace Blackhole.Maui
{
    public partial class App : Application
    {
        /// <summary>
        /// Erre az útvonalra mentjük a félbehagyott játékokat
        /// </summary>
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly IBlackholeDataAccess _blackholeDataAccess;
        private readonly BlackholeGameModel _blackholeGameModel;
        private readonly IStore _blackholeStore;
        private readonly BlackHoleViewModel _blackholeViewModel;

        public App()
        {
            InitializeComponent();

            _blackholeStore = new BlackholeStore();
            _blackholeDataAccess = new BlackholeFileDataAccess(FileSystem.AppDataDirectory);//nincs ilyen lehetosegunk a file cuccunkban

            _blackholeGameModel = new BlackholeGameModel(_blackholeDataAccess);
            _blackholeViewModel = new BlackHoleViewModel(_blackholeGameModel);

            _appShell = new AppShell(_blackholeStore, _blackholeDataAccess, _blackholeGameModel, _blackholeViewModel)
            {
                BindingContext = _blackholeViewModel
            };
            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            // az alkalmazás indításakor    NEKUNK MIERT NEM KELL NEWGAME ALKALMAZAS INDITASAKOR? JA H STARTBOL NE LEGYEN SEMMI GOT IT
            //window.Created += (s, e) =>
            //{
            //    // új játékot indítunk
            //    //_blackholeGameModel.NewGame();
            //    //ott all es majd betoltjuk
            //};

            // amikor az alkalmazás fókuszba kerül
            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                    return;

                Task.Run(async () =>
                {
                    // betöltjük a felfüggesztett játékot, amennyiben van
                    try
                    {
                        await _blackholeGameModel.LoadGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            // amikor az alkalmazás fókuszt veszt
            window.Deactivated += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // elmentjük a jelenleg folyó játékot
                        await _blackholeGameModel.SaveGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            return window;
        }
    }
}

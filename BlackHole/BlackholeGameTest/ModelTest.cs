using BlackHoleGame.Persistence;
using BlackHoleGame.Model;
using System.IO;

namespace BlackholeGameTest
{
    [TestClass]
    public class ModelTest
    {
        BlackholeGameModel _model = null!;

        [TestInitialize]
        public void InitGameTest()
        {
            _model = new BlackholeGameModel(new BlackholeFileDataAccess());
        }


        

        [TestMethod]
        public async Task SaveGameTest()
        {
            _model.NewGame(5);//elokeszites
            _model.SelectCell(1, 1, true);
            _model.ChangePlayer();
            //a fajl elso sora az kell legyen hogy "Red 11"
            await _model.SaveGameAsync("testOutput.bgf");//cselekmeny
            using StreamReader f = new StreamReader("testOutput.bgf");
            Assert.AreEqual("Red 11", f.ReadLine());//ellenorzes
            Assert.AreEqual("Red None None None Red ", f.ReadLine());
        }

        [TestMethod]
        public async Task SaveGameFailed()
        {
            await Assert.ThrowsExceptionAsync<BlackholeDataException>(async () => await _model.SaveGameAsync("B:\\34ht0g793wefNONEXISTINGFOLDER\\savedgame.bgf"));
        }
        
        [TestMethod]
        public async Task LoadGameTest()
        {
            await _model.LoadGameAsync("Data\\Reference\\testInput.bgf");//egy 9x9es input file, eleve meghatarozott

            Assert.AreEqual(_model.Table.GetGameCellState(_model.TableSize - 1, _model.TableSize - 1), CellState.None);

            Assert.AreEqual(9, _model.TableSize);

        }
        
        [TestMethod]
        public async Task LoadGameFailed()
        {
            BlackholeGameModel model = new (null);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await model.LoadGameAsync("Data\\Reference\\testInput.bgf"));
        }
        
        [TestMethod]
        public async Task LoadGameInvalidFile()
        {
            await Assert.ThrowsExceptionAsync<BlackholeDataException>(async () => await _model.LoadGameAsync("Data\\Reference\\testInputInvalid.bgf"));
        }

        [TestMethod]
        public async Task SaveAndLoadGameTest()
        {
            //elokeszites
            _model.NewGame(5);
            _model.SelectCell(1, 1, true);
            _model.ChangePlayer();

            //cselekmeny
            await _model.SaveGameAsync("testOutput.bgf");
            await _model.LoadGameAsync("testOutput.bgf");

            //ellenorzes: megnezzuk, hogy tenyleg az a file kerult betoltesre, amit lementettunk
            Assert.IsTrue(_model.Table.AnySelected());
            Assert.IsTrue(_model.Table.SelectedCell()?.CellState == CellState.Red);
            Assert.IsTrue(_model.Table.Player == CellState.Red);
            Assert.AreEqual(5, _model.TableSize);
        }

        [TestMethod]
        public void NewGameTest()
        {


            _model.NewGame(5);//cselekmeny

            Assert.IsTrue(_model.Table.Player == CellState.Blue);//kek kezd
            Assert.IsTrue(_model.TableSize == 5);

            Assert.IsTrue(_model.Table.GetGameCellState(4, 4) == CellState.Blue);//a legutolso cella kek hajos kell legyen egy friss asztalon
            Assert.IsTrue(_model.Table.GetGameCellState(0, 0) == CellState.Red);

            Assert.AreEqual(4, _model.Table.PlayerShipCount(CellState.Blue));
            Assert.AreEqual(_model.Table.PlayerShipCount(CellState.Blue), _model.Table.PlayerShipCount(CellState.Red));


        }
        
        [TestMethod]
        public void ChangePlayerTest()
        {
            bool playerchanged = false;//elokeszites
            _model.PlayerChanged += (sender, args) => playerchanged = true;//hozzaadunk egy event handlert a modellhez
            _model.NewGame(5);
            Assert.IsTrue(playerchanged);
            Assert.IsTrue(_model.Table.Player == CellState.Blue);

            playerchanged = false;
            _model.ChangePlayer();
            Assert.IsTrue(playerchanged);
            Assert.IsTrue(_model.Table.Player == CellState.Red);


        }
        
        [TestMethod]
        public void SelectCellTest()
        {
            bool cellChanged = false;//elokeszites
            _model.CellChanged += (sender, args) => cellChanged = true;//hozzaadunk egy event handlert a modellhez
            _model.NewGame(5);
            _model.SelectCell(1, 1, true);
            Assert.IsTrue(cellChanged);//kuldott eventet
            Assert.IsTrue(_model.Table.AnySelected());//ki lett valasztva a tablan valami

            Assert.IsTrue(_model.Table.SelectedCell()?.CellState == _model.Table.GetGameCellState(1, 1));//az lett a kivalasztott amit kijeloltunk

            Assert.IsTrue(_model.Table.SelectedCell()?.X == 1 && _model.Table.SelectedCell()?.Y == 1);//az lett kivalasztva, amit kivalasztottunk

            Assert.IsTrue(_model.Table.GetGameCellSelected(1, 1));//ez a fg visszaadja hogy az adott koordinataju cella ki van e valasztva(azt varjuk h igen)



        }
        
        [TestMethod]
        public void UpdateCellTest()
        {
            bool cellChanged = false;//elokeszites
            _model.CellChanged += (sender, args) => cellChanged = true;//hozzaadunk egy event handlert a modellhez
            _model.NewGame(5);
            _model.UpdateCell(1, 1, CellState.Blue);
            Assert.IsTrue(cellChanged);//kuldott eventet
            Assert.IsTrue(_model.Table.GetGameCellState(1, 1) == CellState.Blue);//atvaltozott e arra amit mondtunk




        }
        
        [TestMethod]
        public void HandleGameCellClickedTest()
        {
            _model.NewGame(5);//szimulalunk egy scenariot
            _model.UpdateCell(3,1, CellState.None);
            _model.UpdateCell(1, 3, CellState.None);

            _model.UpdateCell(2, 1, CellState.Blue);
            _model.UpdateCell(2, 3, CellState.Red);//szimulaltunk minden jatekostol 1 lepest, a kovetkezo a kek, aki bele fog lepni a fekete lyukba

            _model.SelectCell(2, 1, true);
            

            _model.HandleGameCellClicked(2,2);//tehat belement a lyukba egy kek, most ellenorizzuk

            //onnan elugrott e?
            //kevesebb hajo maradt e

            Assert.IsTrue(_model.Table.GetGameCellState(2, 1) == CellState.None);
            Assert.IsTrue(_model.Table.PlayerShipCount(CellState.Blue) < _model.Table.PlayerShipCount(CellState.Red));






            
        }
    }
}
using BlackHoleGame.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Model
{
    public class BlackholeGameModel
    {
        #region Fields

        private IBlackholeDataAccess? _dataAccess;
        private BlackholeTable _table;
        private int _tableSize;
        private CellState _player;



        #endregion

        #region Properties
        public BlackholeTable Table { get { return _table; } }

        public int TableSize { get { return _tableSize; } }

        #endregion

        #region Events
        public event EventHandler<BlackholeCellEventArgs>? CellChanged;
        public event EventHandler<BlackholeGameEventArgs>? GameOver;
        public event EventHandler<BlackholePlayerEventArgs>? PlayerChanged;
        #endregion

        #region Constructor
        public BlackholeGameModel(IBlackholeDataAccess? dataAccess)
        {
            _dataAccess = dataAccess;
            _table = new BlackholeTable();
            _tableSize = 0;//default ertek
            _player = CellState.Blue;//mindig a kek kezd
        }
        #endregion

        #region Public game methods

        public void UpdateCell(int x, int y, CellState cs)
        {
            _table.UpdateCell(x, y, cs);
            OnCellChanged(x, y, cs, _table.GetGameCellSelected(x, y));
        }

        public void SelectCell(int x, int y, bool selected)
        {
            _table.SelectCell(x, y, selected);
            OnCellChanged(x, y, _table.GetGameCellState(x, y), selected);
        }



        public void NewGame(int size)
        {
            _tableSize = size;
            _table = new BlackholeTable(_tableSize);
            GenerateTableCells();

            _player = CellState.Blue;
            _table.Player = _player;
            OnPlayerChanged(_player);
            

        }



        public async Task LoadGameAsync(string path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided");
            }
            _table = await _dataAccess. LoadAsync(path);
            _player = _table.Player;
            _tableSize = _table.TableSize;
            OnPlayerChanged(_player);

        }
        public async Task SaveGameAsync(string path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }

            await _dataAccess.SaveAsync(path, _table);
        }
        public void HandleGameCellClicked(int x, int y)
        {
            //megkapja h melyik gombra kattintottak
            if (GetWinner() != CellState.None)//ezzel blokkoljuk a klikkelest
            {
                return;
            }


            if (_table.AnySelected())//ilyenkor lepes uzemmod
            {
                bool clickedCellSelected = _table.GetGameCellSelected(x, y);
                if (clickedCellSelected)//ha pont arra nyomtunk, ami ki volt mar jelolve
                {
                    _table.SelectCell(x, y, false);
                    OnCellChanged(x, y, _table.GetGameCellState(x, y), _table.GetGameCellSelected(x, y));
                }
                else
                {
                    var selectedCell = _table.SelectedCell();
                    if (x < selectedCell?.X)//felfele eset
                    {
                        if (y == selectedCell?.Y)
                        {
                            if (!ShipPathUp(selectedCell))
                            {
                                return;//can't step up
                            }
                        }
                        else
                        {
                            return; //can't step
                        }
                    }
                    else if (x > selectedCell?.X)//lefele eset
                    {
                        if (y == selectedCell?.Y)
                        {
                            if (!ShipPathDown(selectedCell))
                            {
                                return;//can't step down
                            }
                        }
                        else
                        {
                            return; //can't step
                        }
                    }
                    else if (y > selectedCell?.Y)//jobbra eset
                    {
                        if (!ShipPathRight(selectedCell))
                        {
                            return;//can't step right
                        }

                    }
                    else if (y < selectedCell?.Y)//balra eset
                    {
                        if (!ShipPathLeft(selectedCell))
                        {
                            return;//can't step left
                        }

                    }
                    ChangePlayer();

                }

            }
            else//nincs semmi kijelolve, ilyenkor ha hajo szabad jelolni
            {
                CellState clickedCellState = _table.GetGameCellState(x, y);

                if (clickedCellState == _player)//a jatekos csak a sajat hajoira nyomhat
                {
                    _table.SelectCell(x, y, true);
                    OnCellChanged(x, y, _table.GetGameCellState(x, y), _table.GetGameCellSelected(x, y));
                }

            }

            var winner = GetWinner();
            if (winner != CellState.None)
            {
                OnGameOver(winner);
            }

        }
        public void ChangePlayer()
        {
            if (_player == CellState.Red)
            {
                _player = CellState.Blue;

            }
            else
            {
                _player = CellState.Red;
            }
            _table.Player = _player;
            OnPlayerChanged(_player);

        }

        #endregion

        #region Private game methods



        private CellState GetWinner()
        {
            if (_table.PlayerShipCount(CellState.Blue) <= (_tableSize - 1) / 2)
            {
                return CellState.Blue;
            }
            if (_table.PlayerShipCount(CellState.Red) <= (_tableSize - 1) / 2)
            {
                return CellState.Red;
            }
            return CellState.None;
        }
        private void UpdateCellWithEvent(int x, int y, CellState cs)
        {
            _table.UpdateCell(x, y, cs);
            OnCellChanged(x, y, cs);
        }


        private bool ShipPathDown(GameCell fieldWithShip)
        {
            if (_table.GetGameCellState(fieldWithShip.X + 1, fieldWithShip.Y) != CellState.None)
            {
                if (_table.GetGameCellState(fieldWithShip.X + 1, fieldWithShip.Y) == CellState.Black)//ha ez a fekete lyuk?
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//beleugrunk a fekete lyukba
                    return true;
                }
                return false;//ha ele se tudunk lepni akkor nem lepunk
            }
            if (fieldWithShip.X == TableSize - 2)//a tablan annyira lent vagyunk, hogy csak egyet lephetunk meg lefele
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(TableSize - 1, fieldWithShip.Y, _player);//ide ugrottunk


                return true;
            }
            for (int i = fieldWithShip.X + 2; i <= TableSize - 1; i++)// amikor random melyen vagyunk
            {
                var roadAhead = _table.GetGameCellState(i, fieldWithShip.Y);
                if (roadAhead != CellState.None)//ha nem ures akkor ele kell lepni
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//mindenkeppen ellepunk vele VALAHOVA

                    if (roadAhead != CellState.Black)//ha nem a lyuk, akkor ele kell lepni, akarmi is az (az elozo utasitas mar levette a hajot onnan, es ha a fekete lyuk volt akkor sose jelenik meg mashol)
                    {
                        UpdateCellWithEvent(i - 1, fieldWithShip.Y, _player);

                    }

                    return true;
                }
            }
            if (_table.GetGameCellState(TableSize - 1, fieldWithShip.Y) == CellState.None)//ha eljutottunk a ciklus vegere es meg az is ures, akkor odalepunk
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(TableSize - 1, fieldWithShip.Y, _player);//ide megyunk
                return true;
            }
            return false;
        }
        private bool ShipPathLeft(GameCell fieldWithShip)
        {
            if (_table.GetGameCellState(fieldWithShip.X, fieldWithShip.Y - 1) != CellState.None)
            {
                if (_table.GetGameCellState(fieldWithShip.X, fieldWithShip.Y - 1) == CellState.Black)//ha ez a fekete lyuk?
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//beleugrunk a fekete lyukba
                    return true;
                }
                return false;//ha ele se tudunk lepni akkor nem lepunk
            }
            if (fieldWithShip.Y == 1)//a tablan annyira balra vagyunk, hogy csak egyet lephetunk meg balra
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(fieldWithShip.X, 0, _player);//ide ugrottunk


                return true;
            }
            for (int i = fieldWithShip.Y - 2; i >= 0; i--)// amikor random melyen vagyunk
            {
                var roadAhead = _table.GetGameCellState(fieldWithShip.X, i);
                if (roadAhead != CellState.None)//ha nem ures akkor ele kell lepni
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//mindenkeppen ellepunk vele VALAHOVA

                    if (roadAhead != CellState.Black)//ha nem a lyuk, akkor ele kell lepni, akarmi is az (az elozo utasitas mar levette a hajot onnan, es ha a fekete lyuk volt akkor sose jelenik meg mashol)
                    {
                        UpdateCellWithEvent(fieldWithShip.X, i + 1, _player);

                    }

                    return true;
                }
            }
            if (_table.GetGameCellState(fieldWithShip.X, 0) == CellState.None)//ha eljutottunk a ciklus vegere es meg az is ures, akkor odalepunk
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(fieldWithShip.X, 0, _player);//ide megyunk
                return true;
            }
            return false;
        }
        private bool ShipPathUp(GameCell fieldWithShip)
        {
            if (_table.GetGameCellState(fieldWithShip.X - 1, fieldWithShip.Y) != CellState.None)//ha kozvetlenul mellette van valami
            {
                if (_table.GetGameCellState(fieldWithShip.X - 1, fieldWithShip.Y) == CellState.Black)//ha ez a fekete lyuk?
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//beleugrunk a fekete lyukba
                    return true;
                }
                return false;//ha ele se tudunk lepni akkor nem lepunk
            }
            if (fieldWithShip.X == 1)//a tablan annyira fent vagyunk, hogy csak egyet lephetunk meg felfele
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(0, fieldWithShip.Y, _player);//ide ugrottunk


                return true;
            }
            for (int i = fieldWithShip.X - 2; i >= 0; i--)// amikor random melyen vagyunk
            {
                var roadAhead = _table.GetGameCellState(i, fieldWithShip.Y);
                if (roadAhead != CellState.None)//ha nem ures akkor ele kell lepni
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//mindenkeppen ellepunk vele VALAHOVA

                    if (roadAhead != CellState.Black)//ha nem a lyuk, akkor ele kell lepni, akarmi is az (az elozo utasitas mar levette a hajot onnan, es ha a fekete lyuk volt akkor sose jelenik meg mashol)
                    {
                        UpdateCellWithEvent(i + 1, fieldWithShip.Y, _player);

                    }

                    return true;
                }
            }
            if (_table.GetGameCellState(0, fieldWithShip.Y) == CellState.None)//ha eljutottunk a ciklus vegere es meg az is ures, akkor odalepunk
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(0, fieldWithShip.Y, _player);//ide megyunk
                return true;
            }
            return false;

        }
        private bool ShipPathRight(GameCell fieldWithShip)
        {
            if (_table.GetGameCellState(fieldWithShip.X, fieldWithShip.Y + 1) != CellState.None)
            {
                if (_table.GetGameCellState(fieldWithShip.X, fieldWithShip.Y + 1) == CellState.Black)//ha ez a fekete lyuk?
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//beleugrunk a fekete lyukba
                    return true;
                }
                return false;//ha ele se tudunk lepni akkor nem lepunk
            }
            if (fieldWithShip.Y == TableSize - 2)//a tablan annyira lent vagyunk, hogy csak egyet lephetunk meg lefele
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(fieldWithShip.X, TableSize - 1, _player);//ide ugrottunk


                return true;
            }
            for (int i = fieldWithShip.Y + 2; i <= TableSize - 1; i++)// amikor random melyen vagyunk
            {
                var roadAhead = _table.GetGameCellState(fieldWithShip.X, i);
                if (roadAhead != CellState.None)//ha nem ures akkor ele kell lepni
                {
                    UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//mindenkeppen ellepunk vele VALAHOVA

                    if (roadAhead != CellState.Black)//ha nem a lyuk, akkor ele kell lepni, akarmi is az (az elozo utasitas mar levette a hajot onnan, es ha a fekete lyuk volt akkor sose jelenik meg mashol)
                    {
                        UpdateCellWithEvent(fieldWithShip.X, i - 1, _player);

                    }

                    return true;
                }
            }
            if (_table.GetGameCellState(fieldWithShip.X, TableSize - 1) == CellState.None)//ha eljutottunk a ciklus vegere es meg az is ures, akkor odalepunk
            {
                UpdateCellWithEvent(fieldWithShip.X, fieldWithShip.Y, CellState.None);//innen
                UpdateCellWithEvent(fieldWithShip.X, TableSize - 1, _player);//ide megyunk
                return true;
            }
            return false;
        }


        private void GenerateTableCells()
        {
            for (int i = 0; i < _tableSize; i++)
            {
                for (int j = 0; j < _tableSize; j++)
                {
                    if (i < _tableSize / 2)
                    {
                        if (i == j || i + j == _tableSize - 1)
                        {
                            _table.GenerateCell(i, j, CellState.Red);
                        }
                        else
                        {
                            _table.GenerateCell(i, j, CellState.None);
                        }
                    }
                    else if (i > _tableSize / 2)
                    {
                        if (i == j || i + j == _tableSize - 1)
                        {
                            _table.GenerateCell(i, j, CellState.Blue);
                        }
                        else
                        {
                            _table.GenerateCell(i, j, CellState.None);
                        }
                    }
                    else
                    {
                        if (i == j)
                        {
                            _table.GenerateCell(i, j, CellState.Black);
                        }
                        else
                        {
                            _table.GenerateCell(i, j, CellState.None);
                        }

                    }
                }
            }
        }




        private void OnCellChanged(int i, int j, CellState state, bool sel)
        {
            CellChanged?.Invoke(this, new BlackholeCellEventArgs(i, j, state, sel));
        }
        private void OnCellChanged(int i, int j, CellState state) //amikor nem akarom kiirni mindenhova hogy false
        {
            OnCellChanged(i, j, state, false);
        }
        private void OnGameOver(CellState state)
        {
            GameOver?.Invoke(this, new BlackholeGameEventArgs(state));
        }
        private void OnPlayerChanged(CellState player)
        {
            PlayerChanged?.Invoke(this, new BlackholePlayerEventArgs(player));
        }
        #endregion


        


    }
}

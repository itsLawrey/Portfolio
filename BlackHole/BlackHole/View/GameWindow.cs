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
    public partial class GameWindow : Form
    {

        #region Fields
        private BlackholeGameModel _model = null!;
        private IBlackholeDataAccess _dataAccess = null!;
        private GameCell[,] _gameCells = null!;
        private bool _gameSaved = false;

        #endregion

        #region Constructors
        public GameWindow()
        {

            InitializeComponent();

            _dataAccess = new BlackholeFileDataAccess();

            _model = new BlackholeGameModel(_dataAccess);
            _model.CellChanged += new EventHandler<BlackholeCellEventArgs>(Game_CellChanged);
            _model.GameOver += new EventHandler<BlackholeGameEventArgs>(Game_Over);//valaki nyer csak akkor van vege a jateknak
            _model.PlayerChanged += new EventHandler<BlackholePlayerEventArgs>(Player_Changed);



        }

        #endregion
 
        #region Private methods

        


        private void Cell_Clicked(object? sender, BlackholeCellEventArgs e)
        {
            _model.HandleGameCellClicked(e.Row,e.Column);

        }
        private void Player_Changed(object? sender, BlackholePlayerEventArgs e)
        {
            switch (e.Player)
            {
                case CellState.Blue:
                    toolStripStatusLabel.Text = "Current player: " + e.Player.ToString();
                    toolStripStatusLabel.ForeColor = Color.Blue;
                    break;
                case CellState.Red:
                    toolStripStatusLabel.Text = "Current player: " + e.Player.ToString();
                    toolStripStatusLabel.ForeColor = Color.Red;
                    break;
                default:
                    toolStripStatusLabel.Text = string.Empty;
                    break;
            }
        }
        private void Game_Over(object? sender, BlackholeGameEventArgs e)
        {
            var winner = e.Winner;
            switch (winner)
            {
                case CellState.None:
                    break;
                case CellState.Blue:
                case CellState.Red:
                    DeclareWinner(winner);
                    break;
                case CellState.Black:
                    break;
                default:
                    break;
            }
            

        }
        private void Game_CellChanged(object? sender, BlackholeCellEventArgs e)//az eventben torteno valtozasokat megjelenitjuk a viewban, amiket a modell kuld
        {
            _gameCells[e.Row, e.Column].Selected = e.Selected;
            _gameCells[e.Row, e.Column].CellState = e.CellState;
            _gameSaved = false;

        }



        private void DeclareWinner(CellState winner)
        {
            MessageBox.Show($"{winner} player won!\nYou can start a new game, save or load from the menu.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            
            toolStripStatusLabel.Text = "GAME OVER!";
            toolStripStatusLabel.ForeColor = Color.DarkViolet;

        }

        private void InitGameTable()
        {
            int tableSize = _model.TableSize;
            _gameCells = new GameCell[tableSize, tableSize];
            gamePanel.Controls.Clear();
            gamePanel.RowStyles.Clear();
            gamePanel.ColumnStyles.Clear();
            gamePanel.RowCount = tableSize;
            gamePanel.ColumnCount = tableSize;
            float cellSize = (float)gamePanel.ClientSize.Width / tableSize;

            for (int i = 0; i < tableSize; i++)
            {
                gamePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSize));
            }

            for (int i = 0; i < tableSize; i++)
            {
                gamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSize));
            }


            var selectedCell = _model.Table.SelectedCell();//ha van kivalasztott cella a modellben (betolteskor)


            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < tableSize; j++)
                {
                    var cell = new GameCell(i, j);

                    cell.CellChanged += Cell_Clicked;

                    cell.CellState = _model.Table.GetGameCellState(i, j);

                    cell.Dock = DockStyle.Fill;

                    if (selectedCell?.X == i && selectedCell?.Y == j)//akkor amikor oda erunk az inicializalasban, azt startbol kivalasztjuk
                    {
                        cell.Selected = true;
                    }
                    _gameCells[i, j] = cell;//belerakja a modellt(abbol az allapotokat) a kepernyon megjeleno matrixba
                    gamePanel.Controls.Add(cell, j, i);

                }
            }
        }

        private void CreateGame(int size)
        {
            _model.NewGame(size);
            InitGameTable();
            _gameSaved = false;
            labelWelcome.Visible = false;//eltuntetjuk az initial uzenete



        }

        private async Task loadGame()
        {


            if (openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);
                    _gameSaved = false;
                    InitGameTable();
                }
                catch (BlackholeDataException)
                {
                    MessageBox.Show("Unable to load." + Environment.NewLine + "Invalid path or unexisting file.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        private async Task saveGame()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék lementese
                    await _model.SaveGameAsync(saveFileDialog.FileName);
                    _gameSaved = true;
                    MessageBox.Show("Game successfully saved.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                catch (BlackholeDataException)
                {
                    MessageBox.Show("Unable to save." + Environment.NewLine + "Inaccessible path.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGame(5);
        }
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGame(7);
        }
        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGame(9);
        }
        private async void loadGameMenuItem_Click(object sender, EventArgs e)
        {
            labelWelcome.Visible = false;
            //ask if curent needs to be saved
            if (!_gameSaved)
            {
                if ((MessageBox.Show("Do you want to save current game?", "Save current?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    await saveGame();
                }
            }
            await loadGame();
        }
        private async void saveGameMenuItem_Click(object sender, EventArgs e)
        {
            await saveGame();
        }
        private async void exitGameMenuItem_Click(object sender, EventArgs e)
        {
            if (!_gameSaved)
            {
                if (MessageBox.Show("Do you want to save before quitting?\nAll progress will be lost.", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await saveGame();
                }
            }
            Application.Exit();

        }
        #endregion
    }
}

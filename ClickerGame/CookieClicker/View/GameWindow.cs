using CookieClicker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookieClicker.View
{
    public partial class GameWindow : Form
    {
        Game gameState = new Game();
        public GameWindow()
        {
            InitializeComponent();

            InitializeDisplay();



        }

        private void buttonCookie_Click(object sender, EventArgs e)
        {
            gameState.IncrementMoney(1);
            UpdateDisplay();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            UpdateDisplay();
        }

        private void LetUpgrade()
        {
            if (gameState.GetMoney() < 5)
            {
                buttonUpgrade.Enabled = false;
            }
            else
            {
                buttonUpgrade.Enabled = true;
            }
        }

        private void InitializeDisplay()
        {
            UpdateDisplay();

            timer1.Interval = (int)gameState.GetTimer().Interval;
            timer1.Enabled = gameState.GetTimer().Enabled;

            buttonUpgrade.Enabled = false;
        }

        private void UpdateDisplay()
        {
            labelMoney.Text = GoodFormat(gameState.GetMoney());
            labelUpgrade.Text = gameState.GetUpgrades().ToString();
            labelIncRate.Text = "Income: " + GoodFormat(gameState.GetIncRate()) + "cps";


            toolStripCurrentTimeLabel.Text = $"Current time: {gameState.GetCurrentTime().ToString()}";
            toolStripOnlineStatusLabel.Text = gameState.GetLastTime() != null ? $"Last save: {gameState.GetLastTime().ToString()}" : $"Last save: -";
            
            
            LetUpgrade();
        }

        private void buttonUpgrade_Click(object sender, EventArgs e)
        {
            gameState.Upgrade();
            UpdateDisplay();
        }

        private string GoodFormat(double value)
        {
            double local_value = Math.Truncate(value * 100) / 100;
            return string.Format("{0:N2}", local_value);
        }

        private void labelUpgrade_Click(object sender, EventArgs e)
        {

        }

        private async void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék lementese
                    await gameState.Save(saveFileDialog1.FileName);
                    MessageBox.Show("Game successfully saved.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                catch (Exception err)
                {
                    MessageBox.Show("Unable to save." + Environment.NewLine + $"Error:{err.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private async void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    await gameState.Load(openFileDialog1.FileName);
                    MessageBox.Show($"Game successfully loaded.\nYou made {gameState.CalculateOfflineEarnings()} cookies while offline.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    InitializeDisplay();
                }
            }
            catch (Exception err)
            {

                MessageBox.Show("Unable to load." + Environment.NewLine + $"Error:{err.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
    }
}

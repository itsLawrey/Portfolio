using JobNet.Model;
using System.Timers;

namespace JobNet
{
    public partial class Form1 : Form
    {
        Game gameState = new Game();
        public Form1()
        {
            InitializeComponent();

            InitializeDisplay();



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}

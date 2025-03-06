using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JobNet.Model
{
    class Game
    {
        private double _money;
        private System.Timers.Timer _timer;
        private double _incrate;
        private int _upgrades;
        public Game()
        {
            _money = 0;
            _incrate = 0;
            _upgrades = 0;
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnSecPassed);
            _timer.Interval = 1000;
            _timer.Enabled = true;
        }

        private void OnSecPassed(object? sender, ElapsedEventArgs e)
        {
            //TODO: refactor
            IncrementMoney(_incrate);
        }

        public double GetMoney()
        {
            double money = _money;
            return money;
        }
        public System.Timers.Timer GetTimer()
        {
            return _timer;
        }
        public void IncrementMoney(double amount)
        {
            _money+=amount;
        }

        public void Upgrade()
        {
            _upgrades++;
            _incrate += 0.02;
            _money -= 5;//kesobb upgrade cost parameter
        }
        public int GetUpgrades()
        {
            return _upgrades;
        }

        public double GetIncRate()
        {
            return _incrate;
        }
    }
}



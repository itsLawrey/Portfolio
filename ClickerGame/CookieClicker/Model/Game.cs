using CookieClicker.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CookieClicker.Model
{
    class Game
    {
        #region adattagok

        
        private System.Timers.Timer _timer;
        private DateTime _currentTime;
        private DateTime? _lastActiveTime = null;

        private double _money;
        private double _income;
        private int _upgrades;


        private DataAccess _dataaccess;

        #endregion

        public Game()
        {
            _money = 0;
            _income = 0;
            _upgrades = 0;
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnSecPassed);
            _timer.Interval = 1000;
            _timer.Enabled = true;
            _dataaccess = new DataAccess();
            _currentTime = DateTime.Now;
        }

        public Game(double money, double income, int upgrades)
        {
            _money = money;
            _income = income;
            _upgrades = upgrades;
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnSecPassed);
            _timer.Interval = 1000;
            _timer.Enabled = true;
            _currentTime = DateTime.Now;
        }

        private void OnSecPassed(object? sender, ElapsedEventArgs e)
        {
            //TODO: refactor
            IncrementMoney(_income);
            _currentTime = DateTime.Now;
        }
        #region getterek setterek

        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        public void SetLastTimeActive(DateTime lastActive)
        {
            _lastActiveTime = lastActive;
        }

        public DateTime? GetLastTime()
        {
            return _lastActiveTime;
        }
        public int GetUpgrades()
        {
            return _upgrades;
        }
        public void SetUpgrades(int upgrades)
        {
            _upgrades = upgrades;
        }

        public void SetIncome(double inc)
        {
            _income = inc;
        }

        public void SetMoney(double money)
        {
            _money = money;
        }

        public double GetIncRate()
        {
            return _income;
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
        #endregion



        public void IncrementMoney(double amount)
        {
            _money += amount;
        }

        public void Upgrade()
        {
            _upgrades++;
            _income += 0.02;
            _money -= 5;//kesobb upgrade cost parameter
        }
        

        public async Task Save(string fileName)
        {
            await _dataaccess.Save(fileName, this);
        }

        public async Task Load(string fileName)
        {
            await _dataaccess.Load(fileName, this);
        }

        public double CalculateOfflineEarnings()
        {
            TimeSpan delta = _currentTime.Subtract((DateTime)_lastActiveTime);
            if (delta!=null)
            {
                double earnings = delta.TotalSeconds * _income;
                IncrementMoney(earnings);
                return earnings;
            }
            return 0;
        }

        
    }
}



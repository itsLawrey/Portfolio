using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackhole.Maui.ViewModel
{
    public class StoredGameViewModel : ViewModelBase
    {
        private String _name = String.Empty;
        private DateTime _modified;
        /// <summary>
        /// Név lekérdezése.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Modified
        {
            get { return _modified; }
            set
            {
                if (_modified != value)
                {
                    _modified = value;
                    OnPropertyChanged();
                }
            }
        }



        /// <summary>
        /// Betöltés parancsa.
        /// </summary>
        public DelegateCommand? LoadGameCommand { get; set; }

        /// <summary>
        /// Mentés parancsa.
        /// </summary>
        public DelegateCommand? SaveGameCommand { get; set; }
    }
}

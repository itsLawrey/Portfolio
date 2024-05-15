using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackhole.Maui.ViewModel
{
    public class StoredGameEventArgs : EventArgs
    {
        public String Name { get; set; } = String.Empty;
    }
}

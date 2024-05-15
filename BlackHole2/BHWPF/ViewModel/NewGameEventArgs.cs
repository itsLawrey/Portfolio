using System;

namespace ViewModel
{
    public class NewGameEventArgs : EventArgs
    {
        public int Size { get; set; }
        public NewGameEventArgs(int size)
        {
            Size = size;
        }
    }
}

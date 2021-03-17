using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab
{

    public class DoorEventArgs : EventArgs
    {
        public bool DoorState { set; get; } //If true door is open, if false door is closed
    }

    public interface IDoor
    {
        event EventHandler<DoorEventArgs> DoorEvent; //Event triggers when value is changed

        void LockDoor();
        void UnlockDoor();
        void DoorOpen();
        void DoorClosed();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab
{
   public class Door : IDoor
    {
        public event EventHandler<DoorEventArgs> DoorEvent;

        private DoorEventArgs doorEventState = new DoorEventArgs
        {
            DoorState = false //Door is closed
        };

        public bool DoorState { get; private set; } //Open = true/Closed = false
        public bool LockState { get; private set; } //Unlocked = true/Locked = false


        public Door()
        {
            DoorState = false;
            LockState = true;
        
        }

        public void LockDoor()
        { }
        public void UnlockDoor()
        { }
        public void DoorOpen()
        { }
        public void DoorClosed()
        { }

    }//class
}//namespace

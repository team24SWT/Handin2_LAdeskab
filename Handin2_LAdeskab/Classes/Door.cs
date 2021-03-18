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
            DoorState = false;  //Closed
            LockState = true;   //Unlocked
        
        }

        //Lock the door
        public void LockDoor()
        {
            if (DoorState || !LockState)
            {
                return; // Returns if the door is open or locked
            }
            LockState = false;
        }
        //Unlock the door
        public void UnlockDoor()
        {
            if (DoorState || LockState)
            {
                return; // Returns if the door is open or unlocked
            }
            LockState = true;
        }
        //Open the door
        public void DoorOpen()
        {
            if (DoorState || !LockState)
            {
                return; // Returns if the door is open or locked
            }
            //Sets new states
            doorEventState.DoorState = true;
            DoorState = true;
            //Event invoke
            DoorEvent?.Invoke(this, doorEventState);
        }
        //CLose the door
        public void DoorClosed()
        {
            if (!DoorState || !LockState)
            {
                return; // Returns if the door is closed or locked
            }
            //Sets new states
            doorEventState.DoorState = false;
            DoorState = false;
            //Event invoke
            DoorEvent?.Invoke(this, doorEventState);
        }

    }//class
}//namespace

using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RFIDEventArgs> RFIDEvent;

        public void OnRfidRead(int id)
        {
            RFIDEvent?.Invoke(this, new RFIDEventArgs() { RFID = id }); //Event call when function is called
        }
    }//class
}//namespace

using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab
{
    public class RFIDEventArgs : EventArgs
    {
       public int RFID { set; get; } //RFID id
    }
   public interface IRFIDReader
    {
        event EventHandler<RFIDEventArgs> RFIDEvent; //Event triggers when value is changed
    }
}

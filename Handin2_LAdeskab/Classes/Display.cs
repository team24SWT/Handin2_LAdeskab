using Handin2_LAdeskab.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab.Classes
{
    public class Display : IDisplay
    {

        private IPrintToDisplay printer;


        public Display(IPrintToDisplay printmsg)
        {
            printer = printmsg;
        }

        public void PhoneConnected()
        {
            printer.PrintToConsole("\n Mobile phone is connected \n");
        }

        public void FejlInPhoneConnection()
        {
            printer.PrintToConsole("\n Fejl is detected, try igen \n");
        }

        public void RFIDMath()
        {
            printer.PrintToConsole("\n RFID tag is readed\n");
        }
         public
        public void RFIDNotMatch()
        {
            printer.PrintToConsole("\n RFID tag is fejled \n");
        }

        public void ChargingInProgress()
        {
            printer.PrintToConsole("\n Mobile is charging \n");
        }
        public void NotCharging()
        {
            printer.PrintToConsole("\n Mobile is not charging \n");
        }
        //public void CurentZero()
        //{
        //    printer.PrintToConsole("Not connection found");
        //}
        public void ChargingComplet()
        {
            printer.PrintToConsole("\n Mobile is full charged \n");
        }

        public void ChargingErorMsg()
        {
            printer.PrintToConsole("\n ERROR! Charging stoppet \n");
        }

        public void StatusDoorLocked()
        {
            printer.PrintToConsole("\n Door is looked\n");
        }

        public void StatusDoorUnLocked()
        {
            printer.PrintToConsole("\n Door i unloked\n");
        }
    }
}
}

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

        public void phoneConnect()
        {
            printer.PrintToConsole("\n Connect your phone! \n");
        }

        public void FejlInPhoneConnection()
        {
            printer.PrintToConsole("\n An error has been detected, try again \n");
        }

        public void RFIDMatch()
        {
            printer.PrintToConsole("\n Read your RFID tag \n");
        }
         
        public void RFIDNotMatch()
        {
            printer.PrintToConsole("\n RFID tag did not match. \n");
        }

        public void ChargingInProgress()
        {
            printer.PrintToConsole("\n The phone charging is in progress \n");
        }
        public void NotCharging()
        {
            printer.PrintToConsole("\n The phone is not charging \n");
        }
        //public void CurentZero()
        //{
        //    printer.PrintToConsole("Not connection found");
        //}
        public void ChargingComplet()
        {
            printer.PrintToConsole("\n The phone is fully charged \n");
        }

        public void ChargingErorMsg()
        {
            printer.PrintToConsole("\n ERROR! Charging has stopped! \n");
        }

        public void StatusDoorLocked()
        {
            printer.PrintToConsole("\n The door is locked and phone is charging. Use your RFID to unlock. \n");
        }

        public void StatusDoorUnLocked()
        {
            printer.PrintToConsole("\n The door is unlocked \n");
        }

    }
}

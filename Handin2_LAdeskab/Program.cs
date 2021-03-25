using System;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Classes;
using Ladeskab;
using UsbSimulator;

class Program
    {
        static void Main(string[] args)
        {
        // Assemble your system here from all the classes
        Door _door = new Door();
        RFIDReader _rfid = new RFIDReader();
        PrintToDisplay _printToDisplay = new PrintToDisplay();
        Display _display = new Display(_printToDisplay);
        LogDisplay _logDisplay = new LogDisplay();
        Log _log = new Log(_logDisplay);
        UsbCharger _charger = new UsbCharger();
        ChargeControl _chargeControl = new ChargeControl(_display, _charger);
        StationControl _stationControl = new StationControl(_charger, _door, _display, _rfid, _log);

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine(" -----------------------------------------\n\n "+
                    " INDTAST FOR AT FORTAGE EN HANDLING\n " +
                    "  E (Exit)\n " +
                    "  O (Open)\n " +
                    "  C (Closed)\n " +
                    "  R (Read RFID)\n\n " +
                    "-----------------------------------------");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                    case 'e':
                        finish = true;
                        break;

                    case 'O':
                    case 'o':
                        _door.DoorOpen();
                        break;

                    case 'C':
                    case 'c':
                        _door.DoorClosed();
                        break;

                    case 'R':
                    case 'r':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        _rfid.OnRfidRead(id);
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }


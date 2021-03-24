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
        UsbChargerSimulator _charger = new UsbChargerSimulator();
        StationControl stationControl = new StationControl(_charger, _door, _display, _rfid, _log);

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        _door.DoorOpen();
                        break;

                    case 'C':
                        _door.DoorClosed();
                        break;

                    case 'R':
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


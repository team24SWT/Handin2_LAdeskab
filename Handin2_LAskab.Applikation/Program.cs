using System;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Classes;
using Ladeskab;
using UsbSimulator;

class Program
{
    public static void  Main(string[] args)
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
        StationControl _stationControl = new StationControl(_chargeControl, _door, _display, _rfid, _log);

        bool finish = false;
        string input;
        System.Console.WriteLine(" -----------------------------------------\n\n " +
            " INDTAST FOR AT FORTAGE EN HANDLING\n " +
            "  E (Exit)\n " +
            "  O (Open)\n " +
            "  C (Closed)\n " +
            "  R (Read RFID)\n " +
            "  G (Guide)\n\n" +
            "-----------------------------------------");
        
        
        do
        {
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

                case 'G':
                case 'g':
                    System.Console.WriteLine(" -----------------------------------------\n\n " +
                        " 1. Open the door of the Ladeskab\n " +
                        " 2. Insert your phone into the Ladeskab\n " +
                        " 3. Connect your phone to the USB charger inside the Ladeskab\n " +
                        " 4. CLose the door of the Ladeskab\n " +
                        " 5. Read your RFID tag on the RFID-Reader\n " +
                        " !!Congratulations! Your phone is now charging!!\n\n" +
                        " Note: To remove your phone from the Ladeskab read your RFID tag on the RFID Reader and open the door to the Ladeskab\n\n" +
                        "-----------------------------------------");
                    break;

                default:
                    break;
            }


        } while (!finish); //test af push
    }
}


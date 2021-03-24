using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbSimulator;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Interfaces;


namespace Ladeskab
{
    public class StationControl
    {

        #region ENUM
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        #endregion


        #region MEMBER VARIABLER
        // Her mangler flere member variable
        private IUsbCharger _charger;
        private IDoor _door;
        private IDisplay _display;
        private IRFIDReader _RFIDReader;
        private int _oldId;
        private ILog _log;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil
        #endregion


        //Constructor
        public StationControl(IUsbCharger charger, IDoor door, IDisplay display, IRFIDReader RFIDReader, ILog log) //constructor
        {
            _charger = charger;
            _door = door;
            _display = display;
            _RFIDReader = RFIDReader;
            _log = log;

            _door.DoorEvent += DoorEventHandler;
          
        }

        #region PROPERTIES
        //Properties
        public LadeskabState _state { get; set; }

        #endregion

        #region EVENT HANDLERS

        //RFID EVENT HANDLER
        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.Connected)
                    {

                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                        _log.LockerLocklog(_oldId);
                        _display.StatusDoorLocked();
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.FejlInPhoneConnection();
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        _log.LockerUnlockLog(_oldId);

                        _display.ChargingComplet();
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.RFIDNotMatch();
                    }

                    break;
            }
        }//end RFID detected

        //DoorHandler 
        private void DoorEventHandler(object sender, DoorEventArgs e)
        {
            switch(_state)
            {
                case LadeskabState.Available:
                    if (e.DoorState==true) //If locker is available it can be opened
                    {
                        _state = LadeskabState.DoorOpen;

                        _display.phoneConnect();
                    }
                    else
                    {
                        throw new Exception("ERROR IN 'AVAILABLE'");
                    }
                    break;
                case LadeskabState.Locked: //if locker is locked it cannot be opened
                    {
                        throw new Exception("ERROR IN 'LOCKED' DOOR IS LOCKED");
                    }
                case LadeskabState.DoorOpen: //If locker is closed it can be opened and is thus available
                    if (e.DoorState==false)
                    {
                        _state = LadeskabState.Available;
                        _display.RFIDMatch();
                    }
                    else
                    {
                        throw new Exception("ERROR IN 'DOOROPEN'");
                    }
                    break;
                default:
                    Console.WriteLine("DEFAULT STATE");
                    break;


            }

        }//end Door handler
        #endregion
    }
}

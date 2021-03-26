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
        private IChargeControl _charger;
        private IDoor _door;
        private IDisplay _display;
        private IRFIDReader _RFIDReader;
        private int _oldId;
        private ILog _log;

        #endregion


        //Constructor
        public StationControl(IChargeControl charger, IDoor door, IDisplay display, IRFIDReader RFIDReader, ILog log) //constructor
        {
            _door = door;
            _charger = charger;
            _display = display;
            _RFIDReader = RFIDReader;
            _log = log;

            _RFIDReader.RFIDEvent += RfidDetected;
            _door.DoorEvent += DoorEventHandler;
          
        }

        #region PROPERTIES
        //Properties
        private LadeskabState _state { get; set; }

        #endregion

        #region EVENT HANDLERS

        //RFID EVENT HANDLER
        private void RfidDetected(object sender, RFIDEventArgs e)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.connected())
                    {

                        _door.LockDoor();
                        _charger.startCharging();
                        _oldId = e.RFID;
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
                    throw new System.Exception("ERROR! DOOR IS OPEN!!!!");

                case LadeskabState.Locked:
                    // Check for correct ID
                    CheckId(_oldId, e.RFID);

                    break;
            }
        }//end RFID detected

        private void CheckId(int _oldId, int id)
        {
            if (id == _oldId)
            {
                _charger.stopCharging();
                _door.UnlockDoor();
                _log.LockerUnlockLog(id);

                _display.StatusDoorUnLocked();
                _state = LadeskabState.Available;
            }
            else
            {
                _display.RFIDNotMatch();
            }
        }
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

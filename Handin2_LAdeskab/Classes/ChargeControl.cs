using Handin2_LAdeskab.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UsbSimulator;

namespace Handin2_LAdeskab.Classes
{
    public class ChargeControl : IChargeControl
    {
        private IUsbCharger _Charger;
        private IDisplay _Display;

        private enum state
        {
            notCharging, 
            fullyCharge,
            charging,
            chargingError
        };

        // States
        private state _State = state.notCharging;
        private state _prevState = state.notCharging;
        public int chargerState = -1;

        private double _current = 0.0;
        public double readCurrent = -1.0;

        // Charging currents
        private const double minCurrent = 5.0;
        private const double maxCurrent = 500.0;
        private const double notChargingCurrent = 0.0;

        // Constructor
        public ChargeControl(IDisplay display, IUsbCharger usbCharger)
        {
            _Display = display;
            _Charger = usbCharger;

            this._Charger.CurrentValueEvent += _Charger_CurrentValueEvent;

        }

        public void startCharging()
        {
            _State = state.charging;
            _Charger.StartCharge();
        }

        public void stopCharging()
        {
            _State = state.notCharging;
            _Charger.StopCharge();
        }

        public bool connected()
        {
            return _Charger.Connected;
        }

        private void _Charger_CurrentValueEvent(object sender, CurrentEventArgs e)
        {
            _current = e.Current;
            readCurrent = _current;
          
            checkChargerState();
            sendToDisplay();
            
        }

        private void checkChargerState()
        {
            if (_current > maxCurrent)
            {
                _Charger.StopCharge();
                _State = state.chargingError;
            }
            else if (_current > minCurrent && _current <= maxCurrent)
            {
                _State = state.charging;
            }
            else if (_current > notChargingCurrent && _current <= minCurrent)
            {
                _State = state.fullyCharge;
            }
            else
            {
                if (_State == state.chargingError)
                {
                    return;
                }
                // Only goes to this state if the state is not in chargingError
                _State = state.notCharging;
            }

            chargerState = (int)_State;
        }

        // Method to update the display
        private void sendToDisplay()
        {
            if (_prevState == _State)
            {
                
                if (_State == state.charging)
                {
                    var chargingProgress = (((500 - (int)readCurrent) * 100) / 495);
                    Console.Write("\r Progress: {0} / 100 %        Press (R) to read RFID and cancel the the charging process!", chargingProgress);
                }
                return;
            }

            switch (_State)
            {
                case state.notCharging:
                    _Display.NotCharging();
                    _prevState = state.notCharging;
                    break;
                case state.fullyCharge:
                    _Display.ChargingComplet();
                    _prevState = state.fullyCharge;
                    break;
                case state.charging:
                    _Display.ChargingInProgress();
                    _prevState = state.charging;
                    break;
                case state.chargingError:
                    _Display.ChargingErorMsg();
                    _prevState = state.chargingError;
                    break;
                default:
                    break;
            }
        }

    }
}

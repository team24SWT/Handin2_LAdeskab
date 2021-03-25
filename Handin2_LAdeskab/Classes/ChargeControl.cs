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

        private state _State = state.notCharging;
        private state _prevState = state.notCharging;

        

    }
}

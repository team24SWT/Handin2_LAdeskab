using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab.Interfaces
{
    public interface IDisplay
    {
        void phoneConnect();
        void FejlInPhoneConnection();
        void RFIDMatch();
        void RFIDNotMatch();
        void ChargingInProgress();
        void NotCharging();
        void ChargingComplet();
        void ChargingErorMsg();
        void StatusDoorLocked();
        void StatusDoorUnLocked();

    }
}

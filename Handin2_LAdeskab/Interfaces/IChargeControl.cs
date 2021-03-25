using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab.Interfaces
{
    public interface IChargeControl
    {
        void startCharging();
        void stopCharging();
        bool connected();

    }
}

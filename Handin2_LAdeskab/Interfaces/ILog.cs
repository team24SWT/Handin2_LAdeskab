using System;
using System.Collections.Generic;
using System.Text;

namespace Handin2_LAdeskab.Interfaces
{
    public interface ILog
    {
        void LockerUnlockLog(int id); //Log for when locker is unlocked
        void LockerLocklog(int id);   //Log for when locker in locked
    }
}

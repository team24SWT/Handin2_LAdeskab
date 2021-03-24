using System;
using System.Collections.Generic;
using System.Text;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Interfaces;

namespace Handin2_LAdeskab.Classes
{
    public class Log : ILog
    {

        private ILogDisplay _log;

        public Log(ILogDisplay log)
        {
            _log = log;
        }

        public void LockerUnlockLog(int id)
        {
            _log.PrintToLog(DateTime.Now + ": Skab låst op med RFID: " + id);
        }
        public void LockerLocklog(int id)
        {
            _log.PrintToLog(DateTime.Now + ": Skab låst med RFID: " + id);
        }
    }
}

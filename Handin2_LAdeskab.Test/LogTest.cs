using System;
using System.Collections.Generic;
using System.Text;
using Handin2_LAdeskab;
using System.Xml.Linq;
using NUnit.Framework;
using NSubstitute;
using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;

namespace Handin2_LAdeskab.Test
{
    class LogTest
    {
        private Log uut;
        private ILogDisplay output;

 
        [SetUp]
        public void setup()
        {
           
            output = Substitute.For<ILogDisplay>();
            uut = new Log(output);
        }

        [TestCase(1)]
        //Test of: Log when the door is unlocked
        public void Log_UnlockedDoor(int x)
        {
            uut.LockerUnlockLog(x);
            output.Received().PrintToLog(Arg.Is<string>(str => str.Contains(": Skab låst op med RFID: " + x)));
        }

        [TestCase(1)]
        //Test of: Log when the door is locked
        public void Log_DoorLocked(int x)
        {
            uut.LockerLocklog(x);
            output.Received().PrintToLog(Arg.Is<string>(str => str.Contains(": Skab låst med RFID: " + x)));
        }


    }
}

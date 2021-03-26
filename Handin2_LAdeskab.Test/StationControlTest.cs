using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;
using NSubstitute;
using Ladeskab;

namespace Handin2_LAdeskab.Test
{
    [TestFixture]
    class StationControlTest
    {
        private StationControl uut;
        private IDisplay DisplayFake;
        private ILog logFake;
        private IDoor DoorFake;
        private IChargeControl ChargeFake;
        private IRFIDReader RFIDfake;

        [SetUp]
        public void Setup()
        {

            DisplayFake = Substitute.For<IDisplay>();
            logFake = Substitute.For<ILog>();
            DoorFake = Substitute.For<IDoor>();
            ChargeFake = Substitute.For<IChargeControl>();
            RFIDfake = Substitute.For<IRFIDReader>();
            uut = new StationControl(ChargeFake, DoorFake, DisplayFake, RFIDfake, logFake);


        }

        #region DoorEventH

        [Test]
        //Test of: When door events the door is open
        public void DoorEventHandler_DoorOpen_StateChange()
        {

            uut._state = StationControl.LadeskabState.Available;


            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });

            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.DoorOpen));

        }

        [Test]
        public void DoorEventHandler_DoorOpen_Display()
        {
            uut._state = StationControl.LadeskabState.Available;

            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });

            DisplayFake.Received(1).phoneConnect();
            DisplayFake.Received(0).RFIDMatch();


        }

        [Test]
        public void DoorEventHandler_DoorStateDoorOpen_Throws()
        {

            uut._state = StationControl.LadeskabState.DoorOpen;
            void throwingFunc()
            {

                DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });
            }

            Assert.Throws(typeof(System.Exception), throwingFunc);
        }

        [Test]
        public void DoorEventHandler_DoorOpenStateLocked_Throws()
        {

            uut._state = StationControl.LadeskabState.Locked;


            void throwingFunc()
            {
                // Raise event in fake
                DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });
            }


            Assert.Throws(typeof(System.Exception), throwingFunc);
        }


        [Test]
        // Test of: when the door events the door is closed
        public void DoorEventHandler_DoorCloseStateAvailable_Throws()
        {

            uut._state = StationControl.LadeskabState.Available;

            void throwingFunc()
            {
                // Raise event in fake
                DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });
            }


            Assert.Throws(typeof(System.Exception), throwingFunc);
        }

        [Test]
        public void DoorEventHandler_DoorCloseStateDoorOpen_stateChangesAvailable()
        {

            uut._state = StationControl.LadeskabState.DoorOpen;


            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });


            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [Test]
        public void DoorEventHandler_DoorCloseStateDoorOpen_Display()
        {

            uut._state = StationControl.LadeskabState.DoorOpen;


            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });


            DisplayFake.Received(0).phoneConnect();
            DisplayFake.Received(1).RFIDMatch();
        }

        [Test]
        public void DoorEventHandler_DoorCloseStateLocked_Throws()
        {

            uut._state = StationControl.LadeskabState.Locked;

            void throwingFunc()
            {

                DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });
            }
            // Assert
            Assert.Throws(typeof(System.Exception), throwingFunc);
        }

        #endregion

        #region RFID - Detect

        [Test]
        //Test of: rfid events is received state=Available
        public void RFIDDetected_StateAvailableChargerConnected_DoorCalledOnce()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });


            DoorFake.Received(1).LockDoor();
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_ChargerCalled()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });


            ChargeFake.Received(1).startCharging();
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_LogDoorLockedCalled()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 10 });


            logFake.Received(1).LockerLocklog(10);
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_DisplayCalled()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });


            DisplayFake.Received(1).StatusDoorLocked();
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_StateChanges()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });


            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerNotConnected_DisplayCalled()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(false);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });

            DisplayFake.Received(1).FejlInPhoneConnection();
        }


        [Test]
        //Tests of: RFID events is received state Door Open
        public void RFIDDetected_StateDoorOpen_Throws()
        {

            uut._state = StationControl.LadeskabState.DoorOpen;
            ChargeFake.connected().Returns(false);

            void throwingFunc()
            {

                RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });
            }

            Assert.Throws(typeof(System.Exception), throwingFunc);
        }


        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        //Test of: simulate an entire open amd close cycle
        public void RFIDDetected_FullCycleSim_DisplayCalls(int id1, int id2, int res)
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id1 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id2 });


            DisplayFake.Received(res).StatusDoorUnLocked();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RFIDDetected_FullCycle_CallsCharger(int id1, int id2, int res)
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id1 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id2 });


            ChargeFake.Received(res).stopCharging();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RFIDDetected_FullCycle_CallsDoor(int id1, int id2, int res)
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id1 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id2 });

            // Assert
            DoorFake.Received(res).UnlockDoor();
        }

        [TestCase(12, 12, 1)]
        [TestCase(12, 13, 0)]
        public void RFIDDetected_FullCycle_CallsLogfile(int id1, int id2, int res)
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id1 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = id2 });


            logFake.Received(res).LockerUnlockLog(id2);
        }

        [Test]
        public void RFIDDetected_FullCycleSim_RFIDMatchStateChangesBack()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });


            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }
        [Test]
        public void RFIDetected_FullCycleSimNo_RFIDMatchStateChangesBack()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });


            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }
        [Test]
        public void RFIDDetected_FullCycleSimNo_RFIDMatch_DisplayCalled()
        {

            uut._state = StationControl.LadeskabState.Available;
            ChargeFake.connected().Returns(true);


            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });


            DisplayFake.Received(1).RFIDNotMatch();
        }

        #endregion

    }
}

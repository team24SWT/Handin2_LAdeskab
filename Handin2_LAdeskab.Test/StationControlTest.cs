using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Handin2_LAdeskab;
using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;
using NSubstitute;
using Ladeskab;
using NSubstitute.ExceptionExtensions;

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

            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });
            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });

            Assert.That(uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        #endregion

        #region DoorEventBlackBoxTests

        [Test]
        public void DoorEventHandler_DoorOpen_Display()
        {
            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });

            DisplayFake.Received(1).phoneConnect();
            DisplayFake.Received(0).RFIDMatch();
        }

        [Test]
        public void DoorEventHandler_DoorCloseStateDoorOpen_Display()
        {

            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = true });
            DoorFake.DoorEvent += Raise.EventWith(new DoorEventArgs() { DoorState = false });

            DisplayFake.Received(1).RFIDMatch();
        }
        #endregion

        #region RFID - Detect

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


        #endregion

        #region RFID BlackBoxTest
                
        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_ChargerCalled()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });

            ChargeFake.Received(1).startCharging();
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_LogDoorLockedCalled()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 10 });

            logFake.Received(1).LockerLocklog(10);
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerConnected_DisplayCalled()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });

            DisplayFake.Received(1).StatusDoorLocked();
        }

        [Test]
        public void RFIDDetected_StateAvailableChargerNotConnected_DisplayCalled()
        {
            ChargeFake.connected().Returns(false);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 1 });

            DisplayFake.Received(1).FejlInPhoneConnection();
        }

        [Test]
        //Test of: simulate an entire open and close cycle
        public void RFIDDetected_FullCycleSim_DisplayCalls_succes()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });

            DisplayFake.Received(1).StatusDoorUnLocked();
        }

        [Test]
        //Test of: simulate an entire open and close cycle
        public void RFIDDetected_FullCycleSim_DisplayCalls_fail()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });

            DisplayFake.Received(0).StatusDoorUnLocked();
        }

        [Test]
        public void RFIDDetected_FullCycle_CallsCharger_success()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });

            ChargeFake.Received(1).stopCharging();
        }

        [Test]
        public void RFIDDetected_FullCycle_CallsCharger_fail()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });

            ChargeFake.Received(0).stopCharging();
        }

        [Test]
        public void RFIDDetected_FullCycle_CallsDoor_success()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });

            // Assert
            DoorFake.Received(1).UnlockDoor();
        }

        [Test]
        public void RFIDDetected_FullCycle_CallsDoor_fail()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });

            // Assert
            DoorFake.Received(0).UnlockDoor();
        }

        [Test]
        public void RFIDDetected_FullCycle_CallsLogfile_CorrectID()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });

            logFake.Received(1).LockerUnlockLog(12);
        }

        [Test]
        public void RFIDDetected_FullCycle_DoesNotCallLog_WrongID()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });

            logFake.Received(0).LockerUnlockLog(13);
        }

        [Test]
        public void RFIDDetected_FullCycleSimNo_RFIDMatch_DisplayCalled()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 12 });
            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 13 });

            DisplayFake.Received(1).RFIDNotMatch();
        }
        #endregion

        #region RFID BVA & EP
        [TestCase(1)]
        [TestCase(20)]
        [TestCase(213)]
        [TestCase(5832)]
        [TestCase(9999)]
        //Test of: rfid events is received state=Available
        public void RFIDDetected_StateAvailableChargerConnected_DoorCalledOnce(int detectedRFID)
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = detectedRFID });

            DoorFake.Received(1).LockDoor();
        }

        [TestCase(-5674)]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void RFIDDetected_RFID_IsBelowLimit(int detectedRFID)
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = detectedRFID });

            DoorFake.Received(0).LockDoor();
        }

        [Test]
        public void RFIDDetected_RFID_IsAtMaxLimit()
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = 10000 });

            DoorFake.Received(1).LockDoor();
        }

        [TestCase(10001)]
        [TestCase(10231)]
        [TestCase(23123)]
        [TestCase(1645888)]
        public void RFIDDetected_RFID_IsAboveGivenLimit(int detectedRFID)
        {
            ChargeFake.connected().Returns(true);

            RFIDfake.RFIDEvent += Raise.EventWith(new RFIDEventArgs() { RFID = detectedRFID });

            DoorFake.Received(0).LockDoor();
        }
        #endregion
    }
}

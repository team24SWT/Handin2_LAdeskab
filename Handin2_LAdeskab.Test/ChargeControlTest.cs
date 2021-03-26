using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UsbSimulator;

namespace Handin2_LAdeskab.Test
{
    [TestFixture]
    public class ChargeControlTest
    {
        private ChargeControl uut;
        private IDisplay testDisplay;
        private IUsbCharger testCharger;

        [SetUp]
        public void Setup()
        {
            testDisplay = Substitute.For<IDisplay>();
            testCharger = Substitute.For<IUsbCharger>();
            uut = new ChargeControl(testDisplay, testCharger);
        }

        #region connected()
        [Test]
        public void ConnectedReturnsTrue()
        {
            testCharger.Connected.Returns(true);

            Assert.IsTrue(uut.connected());
        }

        [Test]
        public void ConnectedReturnsFalse()
        {
            testCharger.Connected.Returns(false);

            Assert.IsFalse(uut.connected());
        }
        #endregion

        #region startcharging

        [Test]
        public void StartChargingCalledOnce()
        {
            uut.startCharging();

            testCharger.Received(1).StartCharge();
        }
        #endregion

        #region stopcharging

        [Test]
        public void StopChargingCalledOnce()
        {
            uut.stopCharging();

            testCharger.Received(1).StopCharge();
        }
        #endregion

        #region _Charger_CurrentValueEvent()

        [Test]
        public void CurrentValueEventReceived()
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 200.0 });

            Assert.That(uut.readCurrent, Is.EqualTo(200.0));
  
        }
        #endregion

        #region checkCurrentTest

        [TestCase(0.0, 0)]
        [TestCase(0.1, 1)]
        [TestCase(5.0, 1)]
        [TestCase(5.1, 2)]
        [TestCase(500.0, 2)]
        [TestCase(500.1, 3)]
        public void CheckCurrentTest_ChargerStateChanges(double currentAdded, int state)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });

            Assert.That(uut.chargerState, Is.EqualTo(state));
        }

        [Test]
        public void StopChargeCalledWhenCurrentOverMaxLimit()
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 550.0 });

            testCharger.Received(1).StopCharge();
        }
        #endregion

        #region sendToDisplay()

        [Test]
        public void DisplayToScreen_StateFromChargingToNotCharging()
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 200.0 });

            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = 0.0 });

            Assert.That(uut.chargerState, Is.EqualTo((int)0));
        }

        
        [TestCase(100, 1, 0, 0)]
        [TestCase(4, 0, 1, 0)]
        [TestCase(600, 0, 0, 1)]
        public void ChangeStateIsCalledWhenChangingCurrent(double currentChange, int a, int b, int c)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentChange });

            //Not charging is not checked as the default state is "notCharging" and is not called
            testDisplay.Received(a).ChargingInProgress();
            testDisplay.Received(b).ChargingComplet();
            testDisplay.Received(c).ChargingErorMsg();

        }

        [TestCase(7, 100, 7, 0, 0)]
        [TestCase(10, 3, 0, 10, 0)]
        [TestCase(20, 600, 0, 0, 20)]
        public void ChangeStateIsCalledOnceForEveryAmountOfEvents(int eventAmount, double currentChange, int a, int b, int c)
        {

            for (int i = 0; i < eventAmount; i++)
            {
                testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentChange });
            }

            testDisplay.Received(a / eventAmount).ChargingInProgress();
            testDisplay.Received(b / eventAmount).ChargingComplet();
            testDisplay.Received(c / eventAmount).ChargingErorMsg();

        }



        #endregion
    }
}

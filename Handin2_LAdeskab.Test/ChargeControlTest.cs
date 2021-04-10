using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UsbSimulator;
using System.Xml.Linq;

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

        [TestCase(200.0)]
        public void CurrentValueEventReceived(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });

            Assert.That(uut.readCurrent, Is.EqualTo(currentAdded));
  
        }
        #endregion

        #region checkCurrentTest

        [TestCase(0.0)]
        public void CheckCurrentTest_NothingDisplayedWhenNoCurrentIsAdded(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });
            testDisplay.Received(0);
        }
        [TestCase(0.1)]
        [TestCase(5.0)]
        public void CheckCurrentTest_ChargerStateChangesWhenCurrentFullyCharged(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });
            testDisplay.Received(1).ChargingComplet();
        }

        [TestCase(5.1)]
        [TestCase(60.0)]
        [TestCase(500.0)]
        public void CheckCurrentTest_ChargerStateChangesWhenCurrentCharging(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });
            testDisplay.Received(1).ChargingInProgress();
        }

        [TestCase(500.1)]
        public void CheckCurrentTest_ChargerStateChangesWhenCurrentOverload(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });
            testDisplay.Received(1).ChargingErorMsg();
            
        }

        [TestCase(550.0)]
        public void StopChargeCalledWhenCurrentOverMaxLimit(double currentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded });
            testCharger.Received(1).StopCharge();
        }
        #endregion

        #region sendToDisplay()

        [TestCase(200.0, 0.0)]
        public void DisplayToScreen_StateFromChargingToNotCharging(double currentAdded, double nextCurrentAdded)
        {
            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = currentAdded});
            testDisplay.Received(1).ChargingInProgress();

            testCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() { Current = nextCurrentAdded });
            testDisplay.Received(1).NotCharging();
        }
 
        #endregion
    }
}

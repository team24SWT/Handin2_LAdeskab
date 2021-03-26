using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
namespace Handin2_LAdeskab.Test
{
    [TestFixture]
    class DisplayTest
    {
        private Display uut;
        private IPrintToDisplay printer;

        [SetUp]
        public void Setup()
        {
            printer = Substitute.For<IPrintToDisplay>();
            uut = new Display(printer);
        }

        [Test]
        public void Print_phoneConnec()
        {
            uut.phoneConnect();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n Connect your phone! \n")));
        }

        [Test]
        public void Print_FejlInPhoneConnection()
        {
            uut.FejlInPhoneConnection();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n An error has been detected, try again \n")));
        }

        [Test]
        public void Print_RFIDMatch()
        {
            uut.RFIDMatch();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n Read your RFID tag \n")));
        }

        [Test]
        public void Print_RFIDNotMatch()
        {
            uut.RFIDNotMatch();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n RFID tag did not match. \n")));
        }

        [Test]
        public void Print_ChargingInProgress()
        {
            uut.ChargingInProgress();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n The phone charging is in progress \n")));
        }

        [Test]
        public void Print_NotCharging()
        {
            uut.NotCharging();

               printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n The phone is not charging \n")));

        }

        [Test]
        public void Print_ChargingComplet()
        {
            uut.ChargingComplet();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n The phone is fully charged \n")));
        }

        [Test]
        public void Print_ChargingErorMsg()
        {
            uut.ChargingErorMsg();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n ERROR! Charging has stopped! \n")));
        }

        [Test]
        public void Print_StatusDoorLocked()
        {
            uut.StatusDoorLocked();

                printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n The door is locked and phone is charging. Use your RFID to unlock. \n")));
        }

        [Test]
        public void Print_StatusDoorUnLocked()
        {
            uut.StatusDoorUnLocked();

            printer.Received().PrintToConsole(Arg.Is<string>(str => str.Contains("\n The door is unlocked \n")));
        }
    }

    
}

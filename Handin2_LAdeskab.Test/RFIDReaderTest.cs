using System;
using System.Collections.Generic;
using System.Text;
using Handin2_LAdeskab;
using System.Xml.Linq;
using NUnit.Framework;
using Handin2_LAdeskab.Classes;
using Handin2_LAdeskab.Interfaces;

namespace Handin2_LAdeskab.Test
{

    [TestFixture]
    public class RFIDReaderTest
    {
        private RFIDReader uut_;
        private RFIDEventArgs receivedEventArgs_;

        [SetUp]
        public void SetUp()
        {
            receivedEventArgs_ = null;
            uut_ = new RFIDReader();
            uut_.OnRfidRead(1111);
            uut_.RFIDEvent += (o, args) =>
             {
                 receivedEventArgs_ = args;
             };
        }

        [Test]

        public void RFIDReaderNewValue_EventFired()
        {
            uut_.OnRfidRead(1112);
            Assert.That(receivedEventArgs_, Is.Not.Null);
        }

        [Test]

        public void RFIDReaderNewValue_CorrectRFIDReader()
        {
            uut_.OnRfidRead(1112);
            Assert.That(receivedEventArgs_.RFID, Is.EqualTo(1112));
        }


        [TestCase (1111)]
        [TestCase(0)]
        [TestCase(-1113)]

        public void RFIDSend(int id) 
        {
            int idInUse = 0;//Arrange!
            uut_.RFIDEvent += (sender, args) => idInUse = args.RFID;//Act!!
            uut_.OnRfidRead(id);
            Assert.That(idInUse, Is.EqualTo(id));//Assert!!!

        }        

    }//RFIDReaderTest

}//namespace

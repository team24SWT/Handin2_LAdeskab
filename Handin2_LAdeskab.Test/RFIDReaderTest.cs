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

        [SetUp]
        public void SetUp()
        {
            uut_ = new RFIDReader();
        }


        #region Testing

        [Test]

        public void RFIDNoMatch() //Does not match with previously written RFID
        {

        }

        #endregion

        #region AAA

        [TestCase (1111)]
        [TestCase(0)]
        [TestCase(-1111)]

        public void RFIDSend(int id) //Does not match with previously written RFID
        {
            int idInUse = 0;//ARRANGE!
            uut_.RFIDEvent += (sender, args) => idInUse = args.RFID;//ACT!!
            uut_.OnRfidRead(idInUse);
            Assert.That(idInUse, Is.EqualTo(id));//ASSERT!!!

        }

        #endregion

        #region Test that I couldn't make

        //Hvordan ved vi at den er modtaget

        //[Test] CANNOT SET INT TO NULL

        //public void RFIDIsEmpty() //Nothing has been written
        //{
        //    uut_.OnRfidRead(0);
        //}

        //[Test] CANNOT SET INT TO STRING

        //public void RFIDNotInt() //The RFID is written in char or special symbols
        //{
        //    uut_.OnRfidRead("asdadasds");
        //}

        #endregion


    }//RFIDReaderTest

}//namespace

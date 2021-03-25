using NUnit.Framework;

namespace Handin2_LAdeskab.Test
{
    public class DoorTests
    {

        private Door uut;
        [SetUp]
        public void Setup()
        {
            uut = new Door();
        }
        #region close Door
        [Test]
        // test of: close a open door
        public void Door_Test1_CloseDoor()
        {
            uut.DoorClosed();
            uut.DoorOpen();
            Assert.IsTrue(uut.DoorState);
        }

        [Test]
        public void Door_Test1_CloseDoor_event()
        {
            //Door open so we can closed it later
            bool notified = false;
            uut.DoorOpen();

            uut.DoorEvent += (sender, args) => notified = true;
            uut.DoorClosed();

            Assert.IsTrue(notified);
        }

        [Test]
        //test of: close a closed door
        public void Door_Test_CloseAclosedDoor()
        {
            uut.DoorClosed();
            uut.DoorClosed();

            Assert.That(uut.DoorState, Is.False);
        }

        [Test]
        //test of: lock a closed door
        public void Door_Test_LockClosedDoor()
        {
            uut.LockDoor();

            Assert.IsFalse(uut.LockState);
        }

        [Test]
        //Test of: unlock a locked door

        public void Door_Test_unlockClosedDoor()
        {
            uut.LockDoor();
            uut.UnlockDoor();

            Assert.That(uut.LockState, Is.True);
        }

        [Test]
        //Test of: Unlock a unlocked door

        public void Door_Test_unlockAunclockedDoor()
        {
            uut.UnlockDoor();
            uut.UnlockDoor();

            Assert.That(uut.LockState, Is.True);
        }

        #endregion'

        #region Door open
        [Test]
        // Test of: open the door
        public void Door_Test_OpenDoor()
        {
          
            uut.DoorOpen();

            
            Assert.IsTrue(uut.DoorState);
        }

        [Test]
        public void Door_Test_OpenDoor_EventN()
        {
           
            bool notified = false;

            
            uut.DoorEvent += (sender, args) => notified = true;
            uut.DoorOpen();

            
            Assert.IsTrue(notified);
        }

        
        [Test]
        // Test of: open the door when its already open
        public void DoorTest_OpenDoor_DoorAlreadyOpen()
        {

            uut.DoorOpen();
            uut.DoorOpen();

            Assert.IsTrue(uut.DoorState);
        }

     
        [Test]
        //Test of: lock the door when the door is open
        // With this test it's deemed not necessary to test the unlock function 
        // when opened, if it can't lock.
        public void DoorTest_LockOpenDoor_DoorIsNotLocked()
        {
            uut.DoorOpen();
            uut.LockDoor();

           
            Assert.IsTrue(uut.DoorState);
        }

       
        [Test]
        //Test of: try to Open a locked door
        public void DoorTest_OpenLockedDoor_DoorNotOpen()
        {

            uut.DoorClosed();
            uut.LockDoor();
            uut.DoorOpen();
         

            
            Assert.IsFalse(uut.DoorState);
        }
#endregion
    }


}
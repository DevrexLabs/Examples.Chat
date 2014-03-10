using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OrigoDB.Examples.Chat;

namespace OrigoDB.Examples.Chat.Test
{
    
    
    /// <summary>
    ///This is a test class for ChatModelTest and is intended
    ///to contain all ChatModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChatModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ChatModel Constructor
        ///</summary>
        [TestMethod()]
        public void ChatModelConstructorTest()
        {
            ChatModel target = new ChatModel();
            var chatrooms = target.GetChatRooms();
            Assert.AreEqual(0, chatrooms.Count);
        }

        /// <summary>
        ///A test for AddRoom
        ///</summary>
        [TestMethod()]
        public void AddRoomTest()
        {
            ChatModel target = new ChatModel(); 
            
            Guid id = Guid.NewGuid();
            string name = "New Room";
            DateTime at = DateTime.Now;
            target.CreateRoom(id, name, at);

            var rooms = target.GetChatRooms();
            Assert.AreEqual(1, rooms.Count);
            Assert.AreEqual(at, rooms[0].Created);
            Assert.AreEqual(name,rooms[0].Name);
            Assert.AreEqual(id, rooms[0].Id);
            Assert.AreEqual(0, rooms[0].CurrentUsers);
            Assert.AreEqual(0, rooms[0].TotalMessages);
        }

        /// <summary>
        ///A test for EnterRoom
        ///</summary>
        [TestMethod()]
        public void EnterRoomTest()
        {
            //Arrange
            ChatModel target = new ChatModel();
            var user = new User(Guid.NewGuid(), "user");
            DateTime when = DateTime.Now;
            target.AddUser(user);
            var roomId = Guid.NewGuid();
            target.CreateRoom(roomId, "New room", when);
            
            //Act
            target.EnterRoom(user.Id, roomId, when);

            //Assert
            var rooms = target.GetChatRooms();
            Assert.AreEqual(1, rooms.Count);
            Assert.AreEqual(1, rooms[0].CurrentUsers);
            
            var users = target.GetUsers(roomId);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(user.Id, users[0].Id);
        }


        /// <summary>
        ///A test for LeaveRoom
        ///</summary>
        [TestMethod()]
        public void LeaveRoomTest()
        {
            //Arrange
            ChatModel target = new ChatModel();
            var user = new User(Guid.NewGuid(), "user");
            var roomId = Guid.NewGuid();
            DateTime when = DateTime.Now;
            target.AddUser(user);
            target.CreateRoom(roomId, "Room", when);

            //Act
            target.EnterRoom(user.Id, roomId, when);
            target.LeaveRoom(user.Id, roomId, when);

            //Assert
            Assert.AreEqual(0,target.GetUsers(roomId).Count);
            Assert.AreEqual(0, target.GetChatRooms()[0].CurrentUsers);
        }

        /// <summary>
        ///A test for WriteMessage
        ///</summary>
        [TestMethod()]
        public void WriteMessageTest()
        {
            //Arrange
            ChatModel target = new ChatModel();
            var user = new User(Guid.NewGuid(), "user");
            var roomId = Guid.NewGuid();
            DateTime when = DateTime.Now;
            target.AddUser(user);
            target.CreateRoom(roomId, "New Room", when);
            target.EnterRoom(user.Id, roomId, when);
            
            //Act
            target.WriteMessage(user.Id, roomId, when, "Hello world!");

            //Assert
            var roomView = target.GetChatRooms()[0];
            Assert.AreEqual(1,roomView.TotalMessages);

            var messages = target.GetMessagesFrom(roomId, DateTime.MinValue);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual("Hello world!", messages[0].Message);
            Assert.AreEqual(user.Id, messages[0].UserId);

        }
    }
}

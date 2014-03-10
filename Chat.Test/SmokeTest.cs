using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrigoDB.Core;
using OrigoDB.Core.Proxy;

namespace OrigoDB.Examples.Chat.Test
{
    [TestClass]
    public class SmokeTest
    {

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BasicScenarioUsingProxy()
        {

            if (Directory.Exists("ChatModel"))
            {
                new DirectoryInfo("ChatModel").Delete(recursive: true);
            }

            var engine = Engine.LoadOrCreate<ChatModel>();
            ChatModel chat = engine.GetProxy();

            // Add a user
            var robert = new User(Guid.NewGuid(), "@robertfriberg");
            chat.AddUser(robert);

            var magnus = new User(Guid.NewGuid(), "@dathor");
            chat.AddUser(magnus);

            var uffe = new User(Guid.NewGuid(), "@ulfbjo");
            chat.AddUser(uffe);

            //Create a room
            DateTime timeStamp = DateTime.Now;
            
            var roomId = Guid.NewGuid();
            chat.CreateRoom(roomId, "RealTime Web with OrigoDB and XSockets", timeStamp);

            int messagesAdded = 0;

            Action<User, string> say = (user, message) =>
                {
                    chat.WriteMessage(user.Id, roomId, timeStamp.AddMinutes(++messagesAdded),
                                            message);
                };
            
            // Enter the room, all at the same time
            chat.EnterRoom(robert.Id, roomId, timeStamp);
            chat.EnterRoom(magnus.Id, roomId, timeStamp);
            chat.EnterRoom(uffe.Id, roomId, timeStamp);

            say(robert, "Really nice having you guys here! So what's so hot about real time web?");
            say(magnus, "First of all, we can push data from the server to the clients, removing the need to poll");
            say(robert, "yeah, but polling isn't that bad, well?");
            say(magnus, "If every client polls for updates once per second, that put's unnecessary load on the server");
            say(magnus, "And still, the client will get the update on average a half second late");
            say(robert, "So real time web is for push applications then?");
            say(uffe,   "No, latency over an open tcp channel is really low compared to the overhead of an http request/response");
            say(uffe,   "Request/response benefits just as well as other communication patterns");
            say(robert, "Ok, I think I understand");
            say(robert, "Actually, OrigoDB is about low latency/high throughput too");
            say(robert, "All the data is kept in RAM so there's no need to read from disk");
            say(uffe,   "But what happens when the system shuts down, won't data get lost?");
            say(robert, "Don't worry, every change is recorded in the command journal and replayed when the system starts");
            say(magnus, "I think OrigoDB and XSockets could work well together providing end to end high performance");
            say(robert, "Yeah, you could embed an engine directly in an xsockets server and have all the data in-process!");
            say(uffe,   "We like OrigoDB but we're too lazy to write a bunch of commands and queries");
            say(robert, "Then you will be very pleased with the proxy feature, you will never have to write a command again!");
            engine.Close();

            //Reloads after close
            engine = Engine.LoadOrCreate<ChatModel>();
            chat = engine.GetProxy();
            var rooms = chat.GetChatRooms();
            Assert.AreEqual(1, rooms.Count);
            var roomView = rooms[0];
            Assert.AreEqual(3, roomView.CurrentUsers);
            Assert.AreEqual(messagesAdded, roomView.TotalMessages);

            var messages = chat.GetMessagesFrom(roomId, timeStamp, messagesAdded);
            foreach (var messagePosted in messages)
            {
                //bad, user name could be included in the event or use a local dictionary of userid -> name
                var user = chat.GetUserById(messagePosted.UserId);
                Console.WriteLine("[{0} - {1}] -> {2}", user.Name, messagePosted.At.ToShortTimeString(), messagePosted.Message);
            }
        }
    }
}

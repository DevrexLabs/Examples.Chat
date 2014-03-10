using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class ChatModel : Model
    {
        private readonly Dictionary<Guid, ChatRoom> _chatRooms;
        private readonly Dictionary<Guid, User> _users;

        public ChatModel()
        {
            _chatRooms = new Dictionary<Guid, ChatRoom>();
            _users = new Dictionary<Guid, User>();
        }

        public void EnterRoom(Guid userId, Guid room, DateTime at)
        {
            _chatRooms[room].EnterUser(userId, at);
        }

        public void LeaveRoom(Guid userId, Guid room, DateTime at)
        {
            _chatRooms[room].ExitUser(userId,at);
        }

        public void WriteMessage(Guid userId, Guid room, DateTime at, string message)
        {
            _chatRooms[room].WriteMessage(at,userId,message);
        }

        public MessagePosted[] GetMessagesFrom(Guid room, DateTime from, int numMessages = 10)
        {
            return _chatRooms[room].GetMessages().SkipWhile(m => m.At <= from).Take(numMessages).ToArray();
        }

        public List<User> GetUsers(Guid room)
        {
            return _chatRooms[room].Users().Select(id => _users[id]).ToList();
        }

        public void AddUser(User user)
        {
            _users[user.Id] = user;
        }

        public User GetUserById(Guid userId)
        {
            return _users[userId];
        }

        public void CreateRoom(Guid roomId, string name, DateTime at)
        {
            var room = new ChatRoom(roomId,name, at);
            _chatRooms[room.Id] = room;
        }

        public List<ChatRoomView> GetChatRooms()
        {
            return _chatRooms.Values
                .OrderBy(r => r.Created)
                .Select(r => new ChatRoomView(r)).ToList();
        }
        
    }
}

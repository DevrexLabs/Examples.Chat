using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class ChatRoomView
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly DateTime Created;
        public readonly int TotalMessages;
        public readonly int CurrentUsers;

        public ChatRoomView(ChatRoom room)
        {
            Id = room.Id;
            Name = room.Name;
            Created = room.Created;
            TotalMessages = room.GetMessages().Count();
            CurrentUsers = room.Users().Count();
        }
    }
}

using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class MessagePosted : Event
    {
        public readonly Guid UserId;
        public readonly string Message;
        public readonly Guid Room;

        public MessagePosted(DateTime at, Guid userId, string message, Guid room)
            : base(at)
        {
            UserId = userId;
            Message = message;
            Room = room;
        }
    }
}
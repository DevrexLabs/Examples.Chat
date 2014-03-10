using System;
using System.Collections.Generic;
using System.Linq;

namespace OrigoDB.Examples.Chat
{

    [Serializable]
    public class ChatRoom
    {
        /// <summary>
        /// Events that have happened in this room
        /// </summary>
        private List<Event> _events;

        /// <summary>
        /// Users currently in the room
        /// </summary>
        private List<Guid> _users; 

        public readonly Guid Id;
        public readonly string Name;

        public ChatRoom(Guid id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            _events = new List<Event>();
            _events.Add(new RoomCreated(created));
            _users = new List<Guid>();
        }

        public IEnumerable<Event> AllEvents()
        {
            foreach (var @event in _events)
            {
                yield return @event;
            }
        }

        public IEnumerable<MessagePosted> GetMessages()
        {
            return _events.OfType<MessagePosted>();
        }


        public IEnumerable<Guid> Users()
        {
            return _users;
        }

        public void EnterUser(Guid userId, DateTime at)
        {
           _users.Add(userId); 
            _events.Add(new UserEntered(at, userId));
        }

        public void ExitUser(Guid userId, DateTime at)
        {
            _users.Remove(userId);
            _events.Add(new UserExited(at, userId));
        }

        public void WriteMessage(DateTime at, Guid userId, string message)
        {
            _events.Add(new MessagePosted(at, userId, message, Id));
        }

        public DateTime Created
        {
            get 
            {
                return _events[0].At;
            }
        }
    }
}

using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class RoomCreated : Event
    {
        public RoomCreated(DateTime at) 
            : base(at)
        {
            
        }
    }
}
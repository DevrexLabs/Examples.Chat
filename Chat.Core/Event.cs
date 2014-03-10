using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public abstract class Event
    {
        public readonly DateTime At;

        protected Event(DateTime at)
        {
            At = at;
        }
    }
}

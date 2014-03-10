using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class UserEntered : Event
    {
        public readonly Guid UserId;

        public UserEntered(DateTime at, Guid userId)
            : base(at)
        {
            UserId = userId;
        }
    }
}
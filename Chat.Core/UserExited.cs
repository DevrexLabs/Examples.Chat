using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class UserExited : Event
    {
        public readonly Guid UserId;

        public UserExited(DateTime at, Guid userId)
            : base(at)
        {
            UserId = userId;
        }
    }
}
using System;

namespace OrigoDB.Examples.Chat
{
    [Serializable]
    public class User
    {
        public readonly Guid Id;
        public readonly string Name;

        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

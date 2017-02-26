using System.Collections.Generic;

namespace SafeTalk.Core.Model
{
    public class Chatroom
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
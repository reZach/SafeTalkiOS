using System;

namespace SafeTalk.Core.Model
{
    public class MessagePayload
    {
        public User FromUser { get; set; }
        public string Message { get; set; }
        public Chatroom Chatroom { get; set; }
    }
}

using System;

namespace Chords.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}

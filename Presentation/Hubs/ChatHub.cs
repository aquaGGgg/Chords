using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Chords.Domain.Entities;
using Chords.Infrastructure;
using Chords.Infrastructure.Data;

namespace Chords.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;

        public ChatHub(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string user, string message)
        {
            // 1. Сохраняем сообщение в БД
            var chatMessage = new ChatMessage
            {
                UserName = user,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            // 2. Отправляем всем клиентам событие ReceiveMessage
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

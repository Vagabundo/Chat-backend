using System.Threading.Tasks;

namespace PublicChat.Services
{
    public class ChatGPTService : IAIChatService
    {
        public async Task<string> GetAnswer(string message)
        {
            return "dumb AI answer";
        }

    }
}
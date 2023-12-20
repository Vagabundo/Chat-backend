using System.Threading.Tasks;
using OpenAI_API;

namespace PublicChat.Services
{
    public class ChatGPTService : IAIChatService
    {
        private OpenAIAPI _api;
        public ChatGPTService ()
        {
            _api = new OpenAIAPI(new APIAuthentication("OpenAPI_Key"));
        }
        public async Task<string> GetAnswer(string message)
        {
            var answer = "";
            var chat = _api.Chat.CreateConversation();
            chat.AppendUserInput(message);

            await chat.StreamResponseFromChatbotAsync(res =>
            {
                answer += res;
            });

            return answer;
        }
    }
}
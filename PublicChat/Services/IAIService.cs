using System.Threading.Tasks;

namespace PublicChat.Services
{
    public interface IAIChatService
    {
        Task<string> GetAnswer(string message);
    }
}
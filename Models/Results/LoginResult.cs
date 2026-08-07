using TradeSim.Models.Domain;

namespace TradeSim.Models.Results
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
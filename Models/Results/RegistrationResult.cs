using System.ComponentModel.DataAnnotations;

namespace TradeSim.Models.Results
{
    public class RegistrationResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}

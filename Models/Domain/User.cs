namespace TradeSim.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public decimal Balance { get; set; } = 1000000m;

        public decimal LockedCapital { get; set; } = 0m;

        // TODO: Replace with CompetitionEntries relationship
        public bool IsCompeting { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }
    }
}
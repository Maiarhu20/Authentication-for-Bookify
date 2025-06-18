namespace BookifyAuth.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string MembershipTier { get; set; } = MembershipTiers.Basic;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public static class MembershipTiers
    {
        public const string Basic = "Basic";
        public const string Premium = "Premium";
    }
}

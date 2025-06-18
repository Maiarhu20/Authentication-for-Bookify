using System.ComponentModel.DataAnnotations;

namespace BookifyAuth.DTO
{
    public class MembershipUpgradeRequest
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; } = string.Empty;
        [Required]
        public string NewMembershipTier { get; set; } = string.Empty;
    }
}

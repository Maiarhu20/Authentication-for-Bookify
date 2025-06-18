using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BookifyAuth.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? MembershipTier { get; set; } 

    }
}

using BookifyAuth.DTO;
using BookifyAuth.Models;
using Microsoft.AspNetCore.Identity;

namespace BookifyAuth.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = new();
        private static int _userID = 0;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }


        public ApiResponse<object> RegisterUser(RegisterRequest request)
        {
            var existingUsers = _users.ToList();

            if (existingUsers.Any(u => u.Email == request.Email))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "Email Is Already Exist!" }
                };
            }

            if (existingUsers.Any(u => u.Username == request.Username))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "UserName Is Already Exist!" }
                };
            }

            string membershipTier = string.IsNullOrWhiteSpace(request.MembershipTier)
                ? MembershipTiers.Basic
                : request.MembershipTier.Trim();

            if (!string.Equals(membershipTier, MembershipTiers.Basic, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(membershipTier, MembershipTiers.Premium, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "Invalid membership tier" }
                };
            }

            var user = new User
            {
                UserId = ++_userID,
                Username = request.Username,
                Email = request.Email,
                MembershipTier = membershipTier,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            _users.Add(user);

            return new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    Username = user.Username,
                    Email = user.Email,
                    MembershipTier = user.MembershipTier
                }
            };
        }

        public ApiResponse<object> Login(LoginRequest request)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "Invalid username or password" }
                };
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user,
                user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "Invalid username or password" }
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    UsernameOrEmail = request.UsernameOrEmail,
                    MembershipTier = user.MembershipTier,
                }
            };
        }

        
        public User? GetUserById(int userId)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);

            return user;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public ApiResponse<Object> UpgradeMembershipTier(MembershipUpgradeRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "User not found" }
                };
            }

            if (string.IsNullOrWhiteSpace(request.NewMembershipTier) ||
                (request.NewMembershipTier != MembershipTiers.Basic &&
                 request.NewMembershipTier != MembershipTiers.Premium))
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { "Invalid membership tier. Available options: Basic, Premium" }
                };
            }

            if (user.MembershipTier == request.NewMembershipTier)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Errors = new List<string> { $"User is already at {request.NewMembershipTier} tier" }
                };
            }

            user.MembershipTier = request.NewMembershipTier;

            return new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    Message = $"Membership tier updated to {request.NewMembershipTier} ",
                    MembershipTier = user.MembershipTier,
                }
            };
        }
    }
}

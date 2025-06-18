using BookifyAuth.DTO;
using BookifyAuth.Models;

namespace BookifyAuth.Services
{
    public interface IUserService
    {
       public ApiResponse<object> RegisterUser(RegisterRequest request);
        public ApiResponse<object> Login(LoginRequest request);
        public User? GetUserById(int userId);
        public List<User> GetAllUsers();
        public ApiResponse<Object> UpgradeMembershipTier(MembershipUpgradeRequest request);
    }
}

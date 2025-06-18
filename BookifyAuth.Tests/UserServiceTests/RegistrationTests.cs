using System;
using Xunit;
using BookifyAuth.DTO;
using BookifyAuth.Models;
using BookifyAuth.Services;

namespace BookifyAuth.Tests.UserServiceTests
{
    public class RegistrationTests
    {
        private readonly UserService _userService;

        public RegistrationTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void RegisterUser_WithValidInformation_ShouldSucceed()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            var result = _userService.RegisterUser(registerRequest);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void RegisterUser_WithDuplicateUsername_ShouldFail()
        {
            var registerRequest1 = new RegisterRequest
            {
                Username = "duplicateuser",
                Email = "user1@example.com",
                Password = "Password123!"
            };

            var registerRequest2 = new RegisterRequest
            {
                Username = "duplicateuser",
                Email = "user2@example.com",
                Password = "Password123!"
            };

            _userService.RegisterUser(registerRequest1);
            var result = _userService.RegisterUser(registerRequest2);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("UserName Is Already Exist!", result.Errors);
        }
        
        [Fact]
        public void RegisterUser_WithDuplicateEmail_ShouldFail()
        {
            var uniqueBase = DateTime.Now.Ticks.ToString();

            var registerRequest1 = new RegisterRequest
            {
                Username = "user1" + uniqueBase,
                Email = "duplicate" + uniqueBase + "@example.com",
                Password = "Password123!"
            };

            var registerRequest2 = new RegisterRequest
            {
                Username = "user2" + uniqueBase,
                Email = "duplicate" + uniqueBase + "@example.com", // Same email as registerRequest1
                Password = "Password123!"
            };

            var result1 = _userService.RegisterUser(registerRequest1);
            Assert.True(result1.Success); 

            var result2 = _userService.RegisterUser(registerRequest2);

            Assert.False(result2.Success);
            Assert.NotNull(result2.Errors);
            Assert.Contains("Email Is Already Exist!", result2.Errors);
        }

        [Fact]
        public void RegisterUser_WithInvalidMembershipTier_ShouldFail()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "testuser" + DateTime.Now.Ticks, 
                Email = "test" + DateTime.Now.Ticks + "@example.com", 
                Password = "Password123!",
                MembershipTier = "Gold" // Non-existent tier
            };

            var result = _userService.RegisterUser(registerRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("Invalid membership tier", result.Errors[0]); 
        }

        [Fact]
        public void RegisterUser_WithValidMembershipTier_ShouldSucceed()
        {
            var uniqueBase = DateTime.Now.Ticks.ToString();
            var registerRequest = new RegisterRequest
            {
                Username = "premiumuser" + uniqueBase,
                Email = "premium" + uniqueBase + "@example.com",
                Password = "Password123!",
                MembershipTier = MembershipTiers.Premium
            };

            var result = _userService.RegisterUser(registerRequest);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            var dataDict = (IDictionary<string, object>)result.Data.GetType()
                .GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

            Assert.Equal(MembershipTiers.Premium, dataDict["MembershipTier"].ToString());
        }

        
    }
}

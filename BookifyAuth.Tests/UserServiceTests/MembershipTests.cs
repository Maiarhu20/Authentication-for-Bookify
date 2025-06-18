using System;
using System.Linq;
using Xunit;
using BookifyAuth.DTO;
using BookifyAuth.Models;
using BookifyAuth.Services;

namespace BookifyAuth.Tests.UserServiceTests
{
    public class MembershipTests
    {
        private readonly UserService _userService;

        public MembershipTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void UpgradeMembershipTier_WithValidRequest_ShouldSucceed()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "upgradeuser",
                Email = "upgrade@example.com",
                Password = "Password123!",
                MembershipTier = MembershipTiers.Basic
            };
            _userService.RegisterUser(registerRequest);

            var upgradeRequest = new MembershipUpgradeRequest
            {
                Email = "upgrade@example.com",
                NewMembershipTier = MembershipTiers.Premium
            };

            var result = _userService.UpgradeMembershipTier(upgradeRequest);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            var updatedUser = _userService.GetAllUsers().First(u => u.Email == "upgrade@example.com");
            Assert.Equal(MembershipTiers.Premium, updatedUser.MembershipTier);
        }

        [Fact]
        public void UpgradeMembershipTier_WithNonexistentUser_ShouldFail()
        {
            var upgradeRequest = new MembershipUpgradeRequest
            {
                Email = "nonexistent@example.com",
                NewMembershipTier = MembershipTiers.Premium
            };

            var result = _userService.UpgradeMembershipTier(upgradeRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("User not found", result.Errors);
        }

        [Fact]
        public void UpgradeMembershipTier_WithInvalidTier_ShouldFail()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "invalidtieruser",
                Email = "invalidtier@example.com",
                Password = "Password123!"
            };
            _userService.RegisterUser(registerRequest);

            var upgradeRequest = new MembershipUpgradeRequest
            {
                Email = "invalidtier@example.com",
                NewMembershipTier = "Gold" // Non-existent tier
            };

            var result = _userService.UpgradeMembershipTier(upgradeRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("Invalid membership tier", result.Errors[0]);
        }


        [Fact]
        public void UpgradeMembershipTier_ToSameTier_ShouldFail()
        {
            var uniqueUsername = "sametieruser" + DateTime.Now.Ticks;
            var uniqueEmail = "sametier" + DateTime.Now.Ticks + "@example.com";

            var registerRequest = new RegisterRequest
            {
                Username = uniqueUsername,
                Email = uniqueEmail,
                Password = "Password123!",
                MembershipTier = MembershipTiers.Premium
            };

            var registerResult = _userService.RegisterUser(registerRequest);
            Assert.True(registerResult.Success);

            var upgradeRequest = new MembershipUpgradeRequest
            {
                Email = uniqueEmail,
                NewMembershipTier = MembershipTiers.Premium // Same tier
            };

            var result = _userService.UpgradeMembershipTier(upgradeRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains($"User is already at {MembershipTiers.Premium} tier", result.Errors);
        }
    }
}
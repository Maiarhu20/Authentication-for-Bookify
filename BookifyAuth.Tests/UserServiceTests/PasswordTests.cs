using System;
using Xunit;
using BookifyAuth.DTO;
using BookifyAuth.Models;
using BookifyAuth.Services;
using Microsoft.AspNetCore.Identity;

namespace BookifyAuth.Tests.UserServiceTests
{
    public class PasswordTests
    {
        private readonly UserService _userService;

        public PasswordTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void VerifyPasswordHash_IsStoredSecurely()
        {
            string password = "Password123!";
            var registerRequest = new RegisterRequest
            {
                Username = "hashuser",
                Email = "hash@example.com",
                Password = password
            };

            _userService.RegisterUser(registerRequest);
            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == "hashuser");

            Assert.NotNull(user);
            Assert.NotEqual(password, user.PasswordHash); // Ensure password is not stored in plain text
            Assert.NotEmpty(user.PasswordHash); // Ensure hash is not empty

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            // Check that the original password successfully verifies against the hash
            Assert.Equal(PasswordVerificationResult.Success, verificationResult);
        }
    }
}
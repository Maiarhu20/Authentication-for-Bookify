using System;
using Xunit;
using BookifyAuth.DTO;
using BookifyAuth.Models;
using BookifyAuth.Services;

namespace BookifyAuth.Tests.UserServiceTests
{
    public class LoginTests
    {
        private readonly UserService _userService;

        public LoginTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldSucceed()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "loginuser",
                Email = "login@example.com",
                Password = "Password123!"
            };
            _userService.RegisterUser(registerRequest);

            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = "loginuser",
                Password = "Password123!"
            };

            var result = _userService.Login(loginRequest);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void Login_WithEmailInsteadOfUsername_ShouldSucceed()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "emailloginuser",
                Email = "emaillogin@example.com",
                Password = "Password123!"
            };
            _userService.RegisterUser(registerRequest);

            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = "emaillogin@example.com",
                Password = "Password123!"
            };

            var result = _userService.Login(loginRequest);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void Login_WithInvalidUsername_ShouldFail()
        {
            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = "nonexistentuser",
                Password = "Password123!"
            };

            var result = _userService.Login(loginRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("Invalid username or password", result.Errors);
        }

        [Fact]
        public void Login_WithInvalidPassword_ShouldFail()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "passworduser",
                Email = "password@example.com",
                Password = "Password123!"
            };
            _userService.RegisterUser(registerRequest);

            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = "passworduser",
                Password = "WrongPassword123!"
            };

            var result = _userService.Login(loginRequest);

            Assert.False(result.Success);
            Assert.NotNull(result.Errors);
            Assert.Contains("Invalid username or password", result.Errors);
        }
    }
}
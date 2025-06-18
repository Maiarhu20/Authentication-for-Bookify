using System;
using System.Collections.Generic;
using Xunit;
using BookifyAuth.DTO;
using BookifyAuth.Models;
using BookifyAuth.Services;

namespace BookifyAuth.Tests.UserServiceTests
{
    public class UserRetrievalTests
    {
        private readonly UserService _userService;

        public UserRetrievalTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void GetUserById_WithValidId_ShouldReturnUser()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "iduser",
                Email = "id@example.com",
                Password = "Password123!"
            };
            _userService.RegisterUser(registerRequest);
            var users = _userService.GetAllUsers();
            var userId = users.First(u => u.Username == "iduser").UserId;

            var user = _userService.GetUserById(userId);

            Assert.NotNull(user);
            Assert.Equal("iduser", user.Username);
        }

        [Fact]
        public void GetUserById_WithInvalidId_ShouldReturnNull()
        {
            int nonExistentId = 99999;

            var user = _userService.GetUserById(nonExistentId);

            Assert.Null(user);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnUsersList()
        {
            _userService.RegisterUser(new RegisterRequest
            {
                Username = "user1",
                Email = "user1@example.com",
                Password = "Password123!"
            });

            _userService.RegisterUser(new RegisterRequest
            {
                Username = "user2",
                Email = "user2@example.com",
                Password = "Password123!"
            });

            var users = _userService.GetAllUsers();

            Assert.NotNull(users);
            Assert.IsType<List<User>>(users);
            Assert.Contains(users, u => u.Username == "user1");
            Assert.Contains(users, u => u.Username == "user2");
        }
    }
}
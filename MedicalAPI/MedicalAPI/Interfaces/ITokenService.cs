using MedicalAPI.DTOs;
using System;
using System.Threading.Tasks;

namespace MedicalAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<bool> UserExists(string Username);
        public Task<bool> ValidatePassword(LoginInfo Login);
        public string GenerateAccessToken(string Username);
        public Task<Guid> UpdateRefreshToken(string Username);
        public string GetTokenUsername(string Token);
        public Task<bool> ValidateRefreshToken(string Username, Guid RefreshToken);
        public Task<bool> ValidateRefreshTokenExp(string Username);
        public Task<object> AddUser(LoginInfo Login);
    }
}

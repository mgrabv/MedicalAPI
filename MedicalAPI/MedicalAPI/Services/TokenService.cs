using MedicalAPI.DTOs;
using MedicalAPI.Interfaces;
using MedicalAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAPI.Services
{
    public class TokenService : ITokenService
    {
        private MedicalContext Context;
        private IConfiguration Configuration;

        public TokenService(MedicalContext Context, IConfiguration Configuration)
        {
            this.Context = Context;
            this.Configuration = Configuration;
        }

        public async Task<bool> UserExists(string Username)
        {
            return await Context.Users.AnyAsync(u => u.Username == Username);
        }

        public async Task<bool> ValidatePassword(LoginInfo Login)
        {
            byte[] UserSalt = await GetUserSalt(Login.Username);

            string EnteredPassword = HashPassword(Login.Password, UserSalt);
            string CorrectPassword = await GetUserPassword(Login.Username);

            return EnteredPassword == CorrectPassword;
        }

        public string GenerateAccessToken(string Username)
        {
            Claim[] UserClaims = new[]
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Role, "User")
            };

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));

            SigningCredentials Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken Token = new JwtSecurityToken(
                issuer: Configuration["Issuer"],
                audience: Configuration["Issuer"],
                claims: UserClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: Creds
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<Guid> UpdateRefreshToken(string Username)
        {
            User User = await GetUser(Username);
            Guid RefreshToken = Guid.NewGuid();

            User.RefreshToken = RefreshToken;
            User.RefreshTokenExp = DateTime.Now.AddDays(1);

            await Context.SaveChangesAsync();
            return RefreshToken;
        }

        public string GetTokenUsername(string Token)
        {
            JwtSecurityToken AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);

            return AccessToken.Claims
                .Where(c => c.Type == ClaimTypes.Name)
                .First().Value;
        }

        public async Task<bool> ValidateRefreshToken(string Username, Guid RefreshToken)
        {
            Guid UserRefreshToken = await GetUserRefreshToken(Username);

            return RefreshToken == UserRefreshToken;
        }

        public async Task<bool> ValidateRefreshTokenExp(string Username)
        {
            DateTime RefreshTokenExp = await GetUserRefreshTokenExp(Username);

            return RefreshTokenExp > DateTime.Now;
        }

        public async Task<object> AddUser(LoginInfo Login)
        {
            byte[] Salt = GenerateSalt();
            string HashedPassword = HashPassword(Login.Password, Salt);

            Guid RefreshToken = Guid.NewGuid();
            string AccessToken = GenerateAccessToken(Login.Username);

            User User = new User()
            {
                Username = Login.Username,
                HashedPassword = HashedPassword,
                Salt = Salt,
                RefreshToken = RefreshToken,
                RefreshTokenExp = DateTime.Now.AddDays(1)
            };

            await Context.Users.AddAsync(User);
            await Context.SaveChangesAsync();

            return new { RefreshToken = RefreshToken, AccessToken = AccessToken };
        }

        private static byte[] GenerateSalt()
        {
            byte[] Salt = new byte[128];

            using (var RNG = RandomNumberGenerator.Create())
            {
                RNG.GetBytes(Salt);
            }

            return Salt;
        }

        private static string HashPassword(string Password, byte[] Salt)
        {
            string HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return HashedPassword;
        }

        private async Task<byte[]> GetUserSalt(string Username)
        {
            return await Context.Users
                .Where(u => u.Username == Username)
                .Select(u => u.Salt).FirstAsync();
        }

        private async Task<User> GetUser(string Username)
        {
            return await Context.Users
                .Where(u => u.Username == Username)
                .FirstAsync();
        }

        private async Task<string> GetUserPassword(string Username)
        {
            return await Context.Users
                .Where(u => u.Username == Username)
                .Select(u => u.HashedPassword).FirstAsync();
        }

        private async Task<Guid> GetUserRefreshToken(string Username)
        {
            return await Context.Users
                .Where(u => u.Username == Username)
                .Select(u => u.RefreshToken).FirstAsync();
        }

        private async Task<DateTime> GetUserRefreshTokenExp(string Username)
        {
            return await Context.Users
                .Where(u => u.Username == Username)
                .Select(u => u.RefreshTokenExp).FirstAsync();
        }
    }
}

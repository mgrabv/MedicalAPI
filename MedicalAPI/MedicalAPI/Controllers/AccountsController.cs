using MedicalAPI.DTOs;
using MedicalAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private ITokenService TokenService;

        public AccountsController(ITokenService TokenService)
        {
            this.TokenService = TokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInfo Login)
        {
            if (!await TokenService.UserExists(Login.Username))
            {
                return StatusCode(400, "This User does not exist. Try to register first.");
            }
            if (!await TokenService.ValidatePassword(Login))
            {
                return StatusCode(400, "Incorrect password.");
            }

            return Ok(new
            {
                AccessToken = TokenService.GenerateAccessToken(Login.Username),
                RefreshToken = await TokenService.UpdateRefreshToken(Login.Username)
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginInfo Login)
        {
            if (await TokenService.UserExists(Login.Username))
            {
                return StatusCode(400, "A User with this username already exists.");
            }

            return Ok(await TokenService.AddUser(Login));
        }

        [HttpPost("refresh-tokens")]
        public async Task<IActionResult> RefreshTokens(TokensInfo Tokens)
        {
            string Username = TokenService.GetTokenUsername(Tokens.AccessToken);
            if (!await TokenService.ValidateRefreshToken(Username, Tokens.RefreshToken))
            {
                return StatusCode(400, "Invalid RefreshToken");
            }
            if (!await TokenService.ValidateRefreshTokenExp(Username))
            {
                return StatusCode(400, "RefreshToken Expired.");
            }

            return Ok(new
            {
                AccessToken = TokenService.GenerateAccessToken(Username),
                RefreshToken = await TokenService.UpdateRefreshToken(Username)
            });
        }
    }
}

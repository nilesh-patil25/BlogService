using BlogService.Core.AppSettings;
using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await GetUser(userName, password);

            if (user == null)
            {
                return string.Empty;
            }

            var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsHelper.GetValue(ConfigConstants.JWTSecretKey)));
            var signIn = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                AppSettingsHelper.GetValue(ConfigConstants.JWTIssuer),
                AppSettingsHelper.GetValue(ConfigConstants.JWTAudience),
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AppUser> Register(AppUser user)
        {
            var users = await _userRepository.GetAllAsync(u => u.UserName == user.UserName);
            if (users.Any())
            {
                throw new InvalidOperationException("Username is already in use.");
            }
            using var hmac = new HMACSHA512();

            var registerUser = new AppUser
            {
                UserName = user.UserName,
                Role = user.Role,
                Password = user.Password,  // hmac.ComputeHash(Encoding.UTF8.GetBytes(password)
                // PasswordSalt = hmac.Key
                Token= user.Token
            };

            var registeredUser = await _userRepository.CreateAsync(registerUser);

            return registeredUser;
        }

        private async Task<AppUser> GetUser(string userName, string password)
        {
            return await _userRepository.FindByUserNameAndPasswordAsync(userName, password);
        }

    }
}

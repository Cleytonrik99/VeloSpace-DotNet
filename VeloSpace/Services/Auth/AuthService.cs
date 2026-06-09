using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using VeloSpace.DTOs.Auth;
using VeloSpace.Model.User;
using VeloSpace.Repositories.UsersRepositories;

namespace VeloSpace.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserAccountRepository userAccountRepository,
            IConfiguration configuration)
        {
            _userAccountRepository = userAccountRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            var user = await _userAccountRepository.GetByEmailAsync(loginRequest.Email);

            if (user == null)
                throw new UnauthorizedAccessException("User or Password invalid");

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                loginRequest.HashedPassword,
                user.HashedPassword
            );

            if (!senhaValida)
                throw new UnauthorizedAccessException("User or Password invalid");

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                UserAccountId = user.UserAccountId,
                Email = user.Email,
                Phone = user.Phone,
                UserRoleId = user.UserRoleId
            };
        }

        private string GenerateJwtToken(UserAccount user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim("userId", user.UserAccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
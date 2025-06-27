

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NGOAutomationAPI.Data;
using NGOAutomationAPI.DTOs;
using NGOAutomationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NGOAutomationAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
		{
            _context = context;
			_config = config;
		}

		public async Task<string> Register(UserRegisterDto request)
		{
			if(await _context.Users.AnyAsync(u => u.Username == request.Username))
			{
				return "Username already exists";
			}

			CreatePasswordHash(request.Password, out byte[] hash);

			var user = new User
			{
				Username = request.Username,
				PasswordHash = Convert.ToBase64String(hash),
				Role = "User"
			};

			_context.Users.Add(user);
            await _context.SaveChangesAsync();
			return "User registered successfully";
		}

		public async Task<string> Login(UserLoginDto request)
		{
			var user  = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
			if (user == null || !VerifyPassword(request.Password, user.PasswordHash)) return "Invalid credentials";
			return GenerateToken(user);
		}

		private void CreatePasswordHash(string password, out byte[] hash)
		{
			using var sha256 = SHA256.Create();
			hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		private bool VerifyPassword(string password, string storedHash)
		{
			using var sha256 = SHA256.Create();
			var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hash) == storedHash;
		}

		private string GenerateToken(User user)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}



using Microsoft.EntityFrameworkCore;
using NGOAutomationAPI.Data;
using NGOAutomationAPI.DTOs;
using NGOAutomationAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace NGOAutomationAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration config)
		{
            _context = context;
            _configuration = config;
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
	}
}

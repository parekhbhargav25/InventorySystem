using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using InventorySystem.Data;
using InventorySystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Services;

public interface IAuthService
{
	Task<User?> AuthenticateAsync(string username, string password);
	(byte[] hash, byte[] salt) HashPassword(string password);
	bool VerifyPassword(string password, byte[] hash, byte[] salt);
}

public sealed class AuthService : IAuthService
{
	private readonly InventoryDbContext _db;

	public AuthService(InventoryDbContext db)
	{
		_db = db;
	}

	public async Task<User?> AuthenticateAsync(string username, string password)
	{
		var normalized = username.Trim().ToLowerInvariant();
		var user = await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == normalized);
		if (user == null) return null;
		return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
	}

	public (byte[] hash, byte[] salt) HashPassword(string password)
	{
		using var rng = RandomNumberGenerator.Create();
		var salt = new byte[16];
		rng.GetBytes(salt);
		using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
		var hash = pbkdf2.GetBytes(32);
		return (hash, salt);
	}

	public bool VerifyPassword(string password, byte[] hash, byte[] salt)
	{
		using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
		var computed = pbkdf2.GetBytes(32);
		return CryptographicOperations.FixedTimeEquals(computed, hash);
	}
}




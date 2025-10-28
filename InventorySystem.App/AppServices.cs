using System;
using InventorySystem.Data;
using InventorySystem.Services;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.App;

public static class AppServices
{
	public static InventoryDbContext DbContext { get; private set; } = null!;
	public static IAuthService AuthService { get; private set; } = null!;
	public static IInventoryService InventoryService { get; private set; } = null!;

	public static void Initialize(string? connectionString)
	{
		var conn = string.IsNullOrWhiteSpace(connectionString)
			? "Server=localhost;Database=InventorySystem;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
			: connectionString!;

		var options = InventoryDbContextFactory.CreateSqlServerOptions(conn);
		DbContext = new InventoryDbContext(options);

		var auth = new AuthService(DbContext);
		DbInitializer.EnsureCreatedAndSeed(DbContext, auth.HashPassword);
		AuthService = auth;
		InventoryService = new InventoryService(DbContext);
	}
}




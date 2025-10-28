using System;
using InventorySystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Data;

public sealed class InventoryDbContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Supplier> Suppliers => Set<Supplier>();
	public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();

	public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(b =>
		{
			b.HasIndex(u => u.Username).IsUnique();
			b.Property(u => u.Username).HasMaxLength(100).IsRequired();
		});

		modelBuilder.Entity<Supplier>(b =>
		{
			b.Property(s => s.Name).HasMaxLength(200).IsRequired();
		});

		modelBuilder.Entity<InventoryItem>(b =>
		{
			b.HasIndex(i => i.Sku).IsUnique();
			b.Property(i => i.Sku).HasMaxLength(50).IsRequired();
			b.Property(i => i.Name).HasMaxLength(200).IsRequired();
			b.HasOne(i => i.Supplier)
				.WithMany(s => s.Items)
				.HasForeignKey(i => i.SupplierId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		modelBuilder.Entity<Order>(b =>
		{
			b.Property(o => o.CustomerName).HasMaxLength(200);
		});

		modelBuilder.Entity<OrderItem>(b =>
		{
			b.HasOne(oi => oi.Order)
				.WithMany(o => o.Items)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			b.HasOne(oi => oi.InventoryItem)
				.WithMany(i => i.OrderItems)
				.HasForeignKey(oi => oi.InventoryItemId)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}

public static class InventoryDbContextFactory
{
	public static DbContextOptions<InventoryDbContext> CreateSqlServerOptions(string connectionString)
	{
		var builder = new DbContextOptionsBuilder<InventoryDbContext>();
		builder.UseSqlServer(connectionString);
		return builder.Options;
	}
}

public static class DbInitializer
{
	public static void EnsureCreatedAndSeed(InventoryDbContext context, Func<string, (byte[] hash, byte[] salt)> passwordHasher)
	{
		context.Database.EnsureCreated();

		if (!context.Users.Any())
		{
			var (hash, salt) = passwordHasher("Admin@123");
			context.Users.Add(new User
			{
				Username = "admin",
				PasswordHash = hash,
				PasswordSalt = salt,
				Role = UserRole.Admin
			});
		}

		if (!context.Suppliers.Any())
		{
			var supplier = new Supplier { Name = "Default Supplier" };
			context.Suppliers.Add(supplier);
			context.InventoryItems.Add(new InventoryItem
			{
				Sku = "SKU-001",
				Name = "Sample Item",
				Description = "Initial sample inventory item",
				QuantityOnHand = 100,
				UnitPrice = 9.99m,
				Supplier = supplier
			});
		}

		context.SaveChanges();
	}
}




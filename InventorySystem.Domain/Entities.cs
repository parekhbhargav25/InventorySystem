using System;
using System.Collections.Generic;

namespace InventorySystem.Domain;

public abstract class Entity
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAtUtc { get; set; }
}

public enum UserRole
{
	Admin = 1,
	Clerk = 2
}

public sealed class User : Entity
{
	public string Username { get; set; } = string.Empty;
	public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
	public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
	public UserRole Role { get; set; } = UserRole.Clerk;
}

public sealed class Supplier : Entity
{
	public string Name { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? Address { get; set; }
	public ICollection<InventoryItem> Items { get; set; } = new List<InventoryItem>();
}

public sealed class InventoryItem : Entity
{
	public string Sku { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int QuantityOnHand { get; set; }
	public decimal UnitPrice { get; set; }
	public Guid SupplierId { get; set; }
	public Supplier? Supplier { get; set; }
	public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public sealed class Order : Entity
{
	public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;
	public string? CustomerName { get; set; }
	public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public sealed class OrderItem : Entity
{
	public Guid OrderId { get; set; }
	public Order? Order { get; set; }
	public Guid InventoryItemId { get; set; }
	public InventoryItem? InventoryItem { get; set; }
	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
}




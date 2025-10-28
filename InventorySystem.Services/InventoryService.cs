using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Data;
using InventorySystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Services;

public interface IInventoryService
{
	Task<List<InventoryItem>> GetItemsAsync(string? search = null);
	Task<InventoryItem> AddItemAsync(InventoryItem item);
	Task<InventoryItem?> UpdateItemAsync(InventoryItem item);
	Task<bool> DeleteItemAsync(Guid id);
}

public sealed class InventoryService : IInventoryService
{
	private readonly InventoryDbContext _db;

	public InventoryService(InventoryDbContext db)
	{
		_db = db;
	}

	public Task<List<InventoryItem>> GetItemsAsync(string? search = null)
	{
		var query = _db.InventoryItems.Include(i => i.Supplier).AsQueryable();
		if (!string.IsNullOrWhiteSpace(search))
		{
			var s = search.Trim();
			query = query.Where(i => i.Name.Contains(s) || i.Sku.Contains(s));
		}
		return query.OrderBy(i => i.Name).ToListAsync();
	}

	public async Task<InventoryItem> AddItemAsync(InventoryItem item)
	{
		_db.InventoryItems.Add(item);
		await _db.SaveChangesAsync();
		return item;
	}

	public async Task<InventoryItem?> UpdateItemAsync(InventoryItem item)
	{
		var existing = await _db.InventoryItems.FindAsync(item.Id);
		if (existing == null) return null;
		existing.Sku = item.Sku;
		existing.Name = item.Name;
		existing.Description = item.Description;
		existing.QuantityOnHand = item.QuantityOnHand;
		existing.UnitPrice = item.UnitPrice;
		existing.SupplierId = item.SupplierId;
		existing.UpdatedAtUtc = DateTime.UtcNow;
		await _db.SaveChangesAsync();
		return existing;
	}

	public async Task<bool> DeleteItemAsync(Guid id)
	{
		var existing = await _db.InventoryItems.FindAsync(id);
		if (existing == null) return false;
		_db.InventoryItems.Remove(existing);
		await _db.SaveChangesAsync();
		return true;
	}
}



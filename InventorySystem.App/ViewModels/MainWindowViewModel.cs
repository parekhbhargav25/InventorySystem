using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InventorySystem.Domain;
using InventorySystem.Services;
using InventorySystem.App;

namespace InventorySystem.App.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
	public ObservableCollection<InventoryItem> Items { get; } = new();
	public ObservableCollection<Supplier> Suppliers { get; } = new();

	private InventoryItem _editItem = new InventoryItem();
	public InventoryItem EditItem
	{
		get => _editItem;
		set { _editItem = value; OnPropertyChanged(nameof(EditItem)); }
	}

	public string? SearchText { get; set; }

	public ICommand RefreshCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand NewCommand { get; }
	public ICommand DeleteCommand { get; }

	public MainWindowViewModel()
	{
		RefreshCommand = new AsyncRelayCommand(RefreshAsync);
		SaveCommand = new AsyncRelayCommand(SaveAsync);
		NewCommand = new AsyncRelayCommand(NewAsync);
		DeleteCommand = new AsyncRelayCommand(DeleteAsync);
		_ = RefreshAsync();
	}

	private async Task RefreshAsync()
	{
		Items.Clear();
		var items = await AppServices.InventoryService.GetItemsAsync(SearchText);
		foreach (var i in items) Items.Add(i);

		// Load suppliers once (basic approach for demo)
		if (Suppliers.Count == 0)
		{
			var ctx = AppServices.DbContext;
			foreach (var s in ctx.Suppliers.ToList()) Suppliers.Add(s);
		}
	}

	private Task NewAsync()
	{
		EditItem = new InventoryItem();
		return Task.CompletedTask;
	}

	private async Task SaveAsync()
	{
		if (EditItem.Id == Guid.Empty)
		{
			EditItem.Id = Guid.NewGuid();
			await AppServices.InventoryService.AddItemAsync(EditItem);
		}
		else
		{
			await AppServices.InventoryService.UpdateItemAsync(EditItem);
		}
		await RefreshAsync();
	}

	private async Task DeleteAsync()
	{
		if (EditItem.Id == Guid.Empty) return;
		await AppServices.InventoryService.DeleteItemAsync(EditItem.Id);
		await RefreshAsync();
		await NewAsync();
	}
}

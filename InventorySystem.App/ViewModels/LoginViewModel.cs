using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InventorySystem.App.Views;

namespace InventorySystem.App.ViewModels;

public sealed class LoginViewModel : ViewModelBase
{
    private readonly Func<string> _passwordProvider;

	public string Username { get; set; } = string.Empty;
	public string? ErrorMessage { get; set; }

	public ICommand LoginCommand { get; }
	public event Action? LoggedIn;

    public LoginViewModel(Func<string> passwordProvider)
	{
        _passwordProvider = passwordProvider;
		LoginCommand = new AsyncRelayCommand(LoginAsync);
	}

	private async Task LoginAsync()
	{
		ErrorMessage = null;
        var password = _passwordProvider();
		var user = await AppServices.AuthService.AuthenticateAsync(Username, password);
		if (user == null)
		{
			ErrorMessage = "Invalid credentials";
			OnPropertyChanged(nameof(ErrorMessage));
			return;
		}

        LoggedIn?.Invoke();
	}
}

public sealed class AsyncRelayCommand : ICommand
{
	private readonly Func<Task> _execute;
	private readonly Func<bool>? _canExecute;
	private bool _isExecuting;

	public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
	{
		_execute = execute;
		_canExecute = canExecute;
	}

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		return !_isExecuting && (_canExecute?.Invoke() ?? true);
	}

	public async void Execute(object? parameter)
	{
		if (!CanExecute(parameter)) return;
		try
		{
			_isExecuting = true;
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			await _execute();
		}
		finally
		{
			_isExecuting = false;
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}



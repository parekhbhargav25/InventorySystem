using Avalonia.Controls;
using InventorySystem.App.ViewModels;

namespace InventorySystem.App.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
	{
		InitializeComponent();
        var passwordBox = this.FindControl<TextBox>("PasswordBox");
        var vm = new LoginViewModel(() => passwordBox?.Text ?? string.Empty);
        vm.LoggedIn += () =>
        {
            var main = new MainWindow { DataContext = new MainWindowViewModel() };
            main.Show();
            Close();
        };
        DataContext = vm;
	}
}



using LoginDemoApp.Models;
using LoginDemoApp.ViewModels;
using LoginDemoApp.Views;
namespace LoginDemoApp;

public partial class App : Application
{
	public User LoggedInUser { get; set; }
	public App(IServiceProvider serviceProvider)
	{
		LoggedInUser = null;
		InitializeComponent();

		MainPage = serviceProvider.GetService<LoginView>();
	}
}

using LoginDemoApp.ViewModels;

namespace LoginDemoApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}
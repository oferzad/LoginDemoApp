using LoginDemoApp.Services;
using LoginDemoApp.ViewModels;

namespace LoginDemoApp.Views;

public partial class UploadProfileImageView : ContentPage
{
	public UploadProfileImageView()
	{
		InitializeComponent();
		this.BindingContext = new UploadProfileImageViewModel(new LoginDemoWebAPIProxy());
	}
}
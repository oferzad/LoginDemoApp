using LoginDemoApp.Services;
using LoginDemoApp.ViewModels;

namespace LoginDemoApp.Views;

public partial class UploadProfileImageView : ContentPage
{
	public UploadProfileImageView(UploadProfileImageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}
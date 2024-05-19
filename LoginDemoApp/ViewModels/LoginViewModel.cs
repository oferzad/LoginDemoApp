using LoginDemoApp.Models;
using LoginDemoApp.Services;
using LoginDemoApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginDemoApp.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        private LoginDemoWebAPIProxy service;
        private IServiceProvider serviceProvider;
        public LoginViewModel(LoginDemoWebAPIProxy service, IServiceProvider serviceProvider)
        {
            InServerCall = false;
            this.service = service;
            this.LoginCommand = new Command(OnLogin);
            this.CheckCommand = new Command(OnCheck);
            this.serviceProvider = serviceProvider;
        }

        public ICommand LoginCommand { get; set; }
        private async void OnLogin()
        {
            //Choose the way you want to blobk the page while indicating a server call
            InServerCall = true;
            LoginInfo userInfo = new LoginInfo()
            {
                Email = this.Email,
                Password = this.Password,
            };

            User u = await this.service.LoginAsync(userInfo);
            
            InServerCall = false;

            //Set the application logged in user to be whatever user returned (null or real user)
            ((App)Application.Current).LoggedInUser = u;
            if (u == null)
            {

                await Application.Current.MainPage.DisplayAlert("Login", "Login Faild!", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login", $"Login Succeed! for {u.Name}", "ok");
                Application.Current.MainPage = serviceProvider.GetService<UploadProfileImageView>();
            }
        }

        public ICommand CheckCommand { get; set; }
        private async void OnCheck()
        {
            //Choose the way you want to blobk the page while indicating a server call
            InServerCall = true;
            
            string str = await this.service.CheckAsync();

            InServerCall = false;

            await Application.Current.MainPage.DisplayAlert("Check", str, "ok");
            
        }


        private bool inServerCall;
        public bool InServerCall
        {
            get
            {
                return this.inServerCall;
            }
            set
            {
                this.inServerCall = value;
                OnPropertyChanged("NotInServerCall");
                OnPropertyChanged("InServerCall");
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password= value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                OnPropertyChanged();
            }
        }
        public bool NotInServerCall
        {
            get
            {
                return !this.InServerCall;
            }
        }
    }
}

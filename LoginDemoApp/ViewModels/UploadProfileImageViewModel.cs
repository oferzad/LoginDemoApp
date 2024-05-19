using LoginDemoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginDemoApp.ViewModels
{
    public class UploadProfileImageViewModel: ViewModelBase
    {
        private LoginDemoWebAPIProxy service;
        public UploadProfileImageViewModel(LoginDemoWebAPIProxy service)
        {
            InServerCall = false;
            this.service = service;
            this.UploadCommand = new Command(UploadImage);
            this.PickImageCommand = new Command(OnPickImage);
            this.CaptureImageCommand = new Command(OnCaptureImage);
            App theApp = (App)Application.Current;
            
        }

        ///The following command handle the pick photo button
        FileResult imageFileResult;
        public ICommand PickImageCommand { get; set; }
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "בחר תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
            }
        }

        public ICommand CaptureImageCommand { get; set; }
        public async void OnCaptureImage()
        {
            FileResult result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "צלם תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
            }
        }

        public ICommand UploadCommand { get; set; }
        private async void UploadImage()
        {
            bool success = false;
            if (this.imageFileResult != null)
            {
                InServerCall = true;
                success = await service.UploadProfileImageAsync(this.imageFileResult.FullPath);
                InServerCall = false;
            }

            if (success)
            {
                App theApp = (App)Application.Current;
                ImageUrl = LoginDemoWebAPIProxy.ImagesRoot + theApp.LoggedInUser.Email + ".jpg";
                await Application.Current.MainPage.DisplayAlert("File Upload", "Success!!", "ok");

            }
            else
                await Application.Current.MainPage.DisplayAlert("File Upload", "FAIL!!", "ok");
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

        public bool NotInServerCall
        {
            get
            {
                return !this.InServerCall;
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = value;
                OnPropertyChanged();
            }
        }
    }
}

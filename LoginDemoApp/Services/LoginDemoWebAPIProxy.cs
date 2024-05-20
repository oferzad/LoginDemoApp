using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoginDemoApp.Models;
namespace LoginDemoApp.Services
{
    public class LoginDemoWebAPIProxy
    {
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5171/api/" : "http://localhost:5171/api/";
        public static string ImagesRoot = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5171/" : "http://localhost:5171/";

        public LoginDemoWebAPIProxy()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            this.client = new HttpClient(handler);
            this.baseUrl = BaseAddress;
        }

        public async Task<User> LoginAsync(LoginInfo userInfo)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}login";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(userInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    User result = JsonSerializer.Deserialize<User>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> CheckAsync()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}check";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    return resContent;
                }
                else
                {
                    return "User is not logged in!";
                }
            }
            catch (Exception ex)
            {

                return "FAILED WITH EXCEPTION!";
            }
        }

        public async Task<bool> UploadProfileImageAsync(string filePath)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UploadProfileImage";
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                multipartFormDataContent.Add(fileContent, "file", filePath);
                HttpResponseMessage response = await client.PostAsync(url, multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UploadProfileImageAsync(FileResult file)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}UploadProfileImage";
            try
            {
                byte[] streamBytes;
                //take the file and make it a byte array
                using (var stream = await file.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    streamBytes = memoryStream.ToArray();
                }
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(streamBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                multipartFormDataContent.Add(fileContent, "file", file.FileName);
                HttpResponseMessage response = await client.PostAsync(url, multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}

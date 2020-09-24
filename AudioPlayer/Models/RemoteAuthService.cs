using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AudioPlayer.Models
{
    public class RemoteAuthService : IAuthService, IDisposable
    {
        private const string urlHost = "https://localhost:5001"; //44342
        private readonly HttpClient client = new HttpClient();

        public async Task<string> Register(string login, string password, string licenseKey)
        {
            //using (var client = new HttpClient())
            //{
            try
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("login", login));
                formVariables.Add(new KeyValuePair<string, string>("password", password));
                formVariables.Add(new KeyValuePair<string, string>("licenseKey", licenseKey));
                var formContent = new FormUrlEncodedContent(formVariables);
                var result = await client.PostAsync($"{urlHost}/Main/Register", formContent);
                var message = await result.Content.ReadAsStringAsync();
                if (message == null && result.IsSuccessStatusCode)
                    return null;
                return message;
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("The server is not available", "Timeout exceed");
                return null;
            }

            //}
        }

        public async Task Logout() => await client.GetAsync($"{urlHost}/Main/Logout");

        public async Task<User> Authenticate(string login, string password)
        {
            // using (var client = new HttpClient())
            // {

            client.Timeout = new TimeSpan(0, 0, 0, 4);
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("login", login));
            formVariables.Add(new KeyValuePair<string, string>("password", password));
            var formContent = new FormUrlEncodedContent(formVariables);
            var result = await client.PostAsync($"{urlHost}/Main/Login", formContent);
            var str = await result.Content.ReadAsStringAsync();
            //return new JsonParser.Parse<User>(str);
            return JsonSerializer.Deserialize<User>(str,
                new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            //  }
        }

        public async Task<string> Edit(User user, string password = null)
        {
            //using (var client = new HttpClient())
            // {
            try
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("password", password));
                formVariables.Add(new KeyValuePair<string, string>("MainDirectory", user.MainDirectory));
                formVariables.Add(new KeyValuePair<string, string>("Volume", user.Volume.ToString()));
                formVariables.Add(new KeyValuePair<string, string>("IsSimple", user.IsSimple.ToString()));
                formVariables.Add(new KeyValuePair<string, string>("Login", user.Login));
                // formVariables.Add(new KeyValuePair<string, string>("PasswordHash", ""));
                var formContent = new FormUrlEncodedContent(formVariables);
                var result = await client.PutAsync($"{urlHost}/Main/Edit", formContent);
                var message = await result.Content.ReadAsStringAsync();
                if (message == null && result.IsSuccessStatusCode)
                    return null;
                return message;
                // }
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("The server is not available", "Timeout exceed");
                return null;
            }
        }

        static byte[] Encrypt(string str)
        {
            MD5 hasher = MD5.Create();
            return hasher.ComputeHash(Encoding.Default.GetBytes(str));
        }

        static bool ArrayEquals<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for (var i = 0; i < arr1.Length; i++)
                if (!arr1[i].Equals(arr2[i]))
                    return false;
            return true;
        }

        public static bool PasswordCheck(string checking, byte[] right) => ArrayEquals(Encrypt(checking), right);
        //TODO: implement Disposable pattern
        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
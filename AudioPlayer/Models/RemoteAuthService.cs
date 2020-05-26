using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    class RemoteAuthService : IAuthService
    {
        private const string urlHost = "https://localhost:44342";
        public async Task<string> Register(string login, string password, string licenseKey)
        {
            using (var client = new HttpClient())
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
        }

        public async void Logout()
        {
            using (var client = new HttpClient())
                await client.GetAsync($"{urlHost}/Main/Logout");
        }

        public async Task<User> Authenticate(string login, string password)
        {
            using (var client = new HttpClient())
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("login", login));
                formVariables.Add(new KeyValuePair<string, string>("password", password));
                var formContent = new FormUrlEncodedContent(formVariables);
                var result = await client.PostAsync($"{urlHost}/Main/Login", formContent);
                var str = await result.Content.ReadAsStringAsync();
                //return new JsonParser.Parse<User>(str);
                return JsonSerializer.Deserialize<User>(str, new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            }
        }

        public async Task<string> Edit(User user, string password = null)
        {
            using (var client = new HttpClient())
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
            }
        }
    }
}

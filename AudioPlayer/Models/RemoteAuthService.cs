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
        public async Task<string> Save(string login, string password, string licenseKey)
        {
            using (var client = new HttpClient())
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("login", login));
                formVariables.Add(new KeyValuePair<string, string>("password", password));
                formVariables.Add(new KeyValuePair<string, string>("licenseKey", licenseKey));
                var formContent = new FormUrlEncodedContent(formVariables);
                var result = await client.PostAsync("/Main/Register", formContent);
                var message = await result.Content.ReadAsStringAsync();
                if (message == null && result.IsSuccessStatusCode)
                    return null;
                return message;
            }
        }

        public async Task<User> Authenticate(string login, string password)
        {
            using (var client = new HttpClient())
            {
                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("login", login));
                formVariables.Add(new KeyValuePair<string, string>("password", password));
                var formContent = new FormUrlEncodedContent(formVariables);
                var result = await client.PostAsync("/Main/Login", formContent);
                return new JsonParser().Parse<User>(
                    await result.Content.ReadAsStreamAsync());

                // await result.Content.
                /*.ReadAsStringAsync();
if (message == null && result.IsSuccessStatusCode)
return null;
return message;*/
            }
        }
    }
}

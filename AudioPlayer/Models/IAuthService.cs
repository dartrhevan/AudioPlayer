using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public interface IAuthService
    {
        Task Logout();
        Task<string> Register(string login, string password, string licenseKey);
        Task<User> Authenticate(string login, string password);
        Task<string> Edit(User user, string password = null);

    }
}
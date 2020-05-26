using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public interface IAuthService
    {
        Task<string> Save(string login, string password, string licenseKey);
        Task<User> Authenticate(string login, string password);
    }
}
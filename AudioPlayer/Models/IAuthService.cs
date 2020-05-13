namespace AudioPlayer.Models
{
    public interface IAuthService
    {
        void Save(User user);
        User Authenticate(string login, string password);
    }
}
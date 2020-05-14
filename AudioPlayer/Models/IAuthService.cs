namespace AudioPlayer.Models
{
    public interface IAuthService
    {
        void Save(User user, string dirName);
        User Authenticate(string login, string password, string dirName);
    }
}
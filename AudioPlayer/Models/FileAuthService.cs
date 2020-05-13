using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace AudioPlayer.Models
{
    public class FileAuthService : IAuthService
    {

        private readonly DirectoryInfo dir = new DirectoryInfo(Path.Combine(MyPlayer.MainDirectory.FullName, "Users"));
        private IEnumerable<User> users;

        public User Authenticate(string login, string password)
        {
            if (!Directory.Exists(dir.FullName))
                Directory.CreateDirectory(dir.FullName);
            users = dir.GetFiles().Select(f => Open(f)); //.ToArray();//dir.GetFiles().Select(f => Open(f));

            var curHash = User.Encrypt(password);
            var user = users.FirstOrDefault(s =>
                s.Login == login && ArrayEquals(s.PasswordHash, curHash));
            if (user == null)

                //MessageBox.Show("Fuck!");
                return null;


            MyPlayer.CurrentUser = user;
            return user;
        }

        private static bool ArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for (var i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i]) return false;
            return true;
        }

        public void Save(User user)
        {
            var fileStream = File.Open(Path.Combine(MyPlayer.MainDirectory.FullName, "Users", user.Login), FileMode.Create);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, user);
            fileStream.Close();
        }

        private User Open(FileInfo f)
        {
            var fileStream = File.Open(f.FullName, FileMode.Open);
            var binaryFormatter = new BinaryFormatter();
            var user = binaryFormatter.Deserialize(fileStream) as User;
            fileStream.Close();
            return user;
        }
    }
}
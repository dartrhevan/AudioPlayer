using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AudioPlayer.Models
{
    /// <summary>
    /// User data
    /// </summary>
    [Serializable]
    public class User
    {
        public readonly string Login;
        public readonly byte[] PasswordHash;
        public readonly bool IsExtended;

        public User(string login, byte[] passwordHash, bool isExtended)
        {
            Login = login;
            PasswordHash = passwordHash;
            IsExtended = isExtended;
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null &&
                   Login == user.Login &&
                   PasswordHash == user.PasswordHash &&
                   IsExtended == user.IsExtended;
        }

        public static byte[] Encrypt(string str)
        {
            MD5 hasher = MD5.Create();
            return hasher.ComputeHash(Encoding.Default.GetBytes(str));
            // data;
        }

        public override int GetHashCode()
        {
            var hashCode = -1526497174;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Login);
            hashCode = hashCode * -1521134295 + PasswordHash.GetHashCode();
            hashCode = hashCode * -1521134295 + IsExtended.GetHashCode();
            return hashCode;
        }
    }
}
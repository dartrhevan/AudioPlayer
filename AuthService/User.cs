using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AuthService
{
    public class User
    {
        public const string ExtendedUserRole = "ExtendedUserRole";
        public const string BasicUserRole = "BasicUserRole";
        [Key]
        [Required]
        public string Login { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        public bool IsExtended { get; set; }
        public string MainDirectory { get; set; }
        private double _volume = 0.5;
        public const string LicenseKey = "159";
        public double Volume
        {
            get => _volume;
            set
            {
                if (value > 1 || value < 0)
                    throw new ArgumentException("Volume should be between 0 and 1");
                _volume = value;
            }
        }

        public bool IsSimple { get; set;  }
        //public readonly List<Tuple<int, string>> PlayList = new List<Tuple<int, string>>();
        public User(string login, byte[] passwordHash, bool isExtended, bool isSimple = false, string mainDirectory = null)
        {
            Login = login;
            PasswordHash = passwordHash;
            IsExtended = isExtended;
            MainDirectory = mainDirectory;
            IsSimple = isSimple;
        }

        public User()
        {

        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Login == user.Login &&
                   PasswordHash == user.PasswordHash &&
                   IsExtended == user.IsExtended;
        }

        public static byte[] Encrypt(string str)
        {
            MD5 hasher = MD5.Create();
            return hasher.ComputeHash(Encoding.Default.GetBytes(str));
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
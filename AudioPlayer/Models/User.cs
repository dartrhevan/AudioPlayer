﻿using System;
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
        public readonly string MainDirectory;
        private double _volume;

        public double Volume
        {
            get => _volume;
            set
            {
                if(value > 1 || value < 0)
                    throw new ArgumentException("Volume should be between 0 and 1");
                _volume = value;
            }
        }

        public bool IsSimple { get; set; }

        public bool UseSimple => IsExtended && !IsSimple;
        //public readonly List<Tuple<int, string>> PlayList = new List<Tuple<int, string>>();
        public User(string login, byte[] passwordHash, bool isExtended, string mainDirectory = MyPlayer.DefaultMainDirecrory, bool isSimple = false)
        {
            Login = login;
            PasswordHash = passwordHash;
            IsExtended = isExtended;
            MainDirectory = mainDirectory;
            IsSimple = isSimple;
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
using System;
using System.Security.Cryptography;
using System.Text;

namespace iTravelerServer.Domain.Helpers
{
    public static class HashPasswordHelper
    {
        public static string HashPassword(string password)
        {
            using(var sha256 = SHA256.Create())  
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password)); 
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }

        public static bool MatchPasswordHash(string existedPassword, string achievedPassword)
        {
            if (existedPassword == HashPassword(achievedPassword))
            {
                return true;
            }

            return false;
        }
    }
}
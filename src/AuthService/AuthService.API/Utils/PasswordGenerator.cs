namespace AuthService.API.Utils
{
    using System;
    using System.Linq;

    public class PasswordGenerator
    {
        private static readonly char[] _chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public static string GeneratePassword(int length = 6)
        {
            var prefix = "Hcmiu@";
            var random = new Random();
            var suffix = new string(Enumerable.Repeat(_chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return prefix + suffix;
        }
    }
}

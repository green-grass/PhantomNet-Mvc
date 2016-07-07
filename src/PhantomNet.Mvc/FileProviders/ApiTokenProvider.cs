using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PhantomNet.Mvc.FileProviders
{
    public class ApiTokenProvider : IApiTokenProvider
    {
        public void GenerateToken(string secretKey, string command, double timeOut, out string timeStamp, out string token)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var password = $"{secretKey}{command}";

            timeStamp = DateTime.Now.AddMilliseconds(timeOut).Ticks.ToString();
            token = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(timeStamp),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public bool ValidateToken(string secretKey, string command, string timeStamp, string token)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }
            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                throw new ArgumentNullException(nameof(timeStamp));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            DateTime time;
            try
            {
                time = new DateTime(long.Parse(timeStamp));
            }
            catch
            {
                throw new ArgumentException(nameof(timeStamp));
            }

            if (time < DateTime.Now)
            {
                return false;
            }

            var password = $"{secretKey}{command}";

            return token == Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(timeStamp),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}

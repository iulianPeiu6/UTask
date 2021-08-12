using SHA3.Net;
using System.Text;

namespace UTask.Services.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        public string GetPasswordSHA3Hash(string password)
        {
            using (var shaAlg = Sha3.Sha3256())
            {
                var passwordHashBytes = shaAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Encoding.UTF8.GetString(passwordHashBytes);
            }
        }
    }
}

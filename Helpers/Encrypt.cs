using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Helpers
{
    public class Encrypt
    {
        public static string EncryptPassword(string senha)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(senha);
                System.Security.Cryptography.SHA1CryptoServiceProvider cripto = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cripto.ComputeHash(buffer)).Replace("-", "");
                return hash;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace LoginApp.Models
{
    public class Commons
    {
        public static int Dev_Id;
        public static int CK;


        public static string GetMd5Hash(MD5 mD5Hash, string input)
        {
            byte[] data = mD5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sbuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sbuilder.Append(data[i].ToString("x2"));
            }

            return sbuilder.ToString();
        }
    }
}
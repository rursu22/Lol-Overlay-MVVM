using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string plainText)
        {
            byte[] plainTextAsBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] encryptedPlainText = ProtectedData.Protect(plainTextAsBytes, [], DataProtectionScope.CurrentUser);

            string encryptedTextBase64 = Convert.ToBase64String(encryptedPlainText);

            return encryptedTextBase64;
        }
        public string Decrypt(string encryptedText)
        {
            byte[] encryptedTextAsBytes = Convert.FromBase64String(encryptedText);

            byte[] decryptedTextAsBytes = ProtectedData.Unprotect(encryptedTextAsBytes, [], DataProtectionScope.CurrentUser);

            string plainText = Encoding.UTF8.GetString(decryptedTextAsBytes);

            return plainText;
        }

        
    }
}

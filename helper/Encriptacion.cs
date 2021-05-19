using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace accesosIp.helper
{
    public class Encriptacion
    {
        public static String Encriptar(String clearText)
        {
            String EncryptionKey = "B7O178";
            try
            {
                Byte[] clearBytes = Encoding.Unicode.GetBytes(clearText.Trim().ToUpper());
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new Byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception EX) { }
            return clearText;
        }

        public static String Desencrip(String cipherText)
        {
            String EncryptionKey = "B7O178";
            String StrValor = "";
            if (cipherText != null)
            {
                try
                {

                    StrValor = cipherText.Replace(" ", "+");
                    Byte[] cipherBytes = Convert.FromBase64String(StrValor);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new Byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            StrValor = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
                catch (Exception EX)
                {
                    StrValor = "texto incorrecto";
                }
            }
            return Mayucula(StrValor);
        }
        public static String Mayucula(String Cadena)
        {
            String textoACambiar = Cadena.ToLower();
            String resul = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textoACambiar);
            String resul2 = new CultureInfo("en-US", false).TextInfo.ToTitleCase(textoACambiar);
            return resul2;
        }
    }
}

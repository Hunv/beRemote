using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using beRemote.Core.Common.SimpleSettings;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using beRemote.Core.Common.LogSystem;
using System.IO.Compression;

namespace beRemote.Core.Common.Helper
{
    public static class Helper
    {
        public static int DbVersionRequired = 7; //The DB-Version this client requires
        
        private static IniFile _AppConfig;

        private static SecureString _UserPassword; //Contains the Userpassword

        private static String _ComputerId = "";

        //Required for UnblocKFile
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFile(string name);


        public static void InitiateApplicationConfiguration()
        {
            _AppConfig = new IniFile("config.ini");
        }

        #region Hashing
        /// <summary>
        /// Creates a 3xMD5 Hash
        /// </summary>
        /// <param name="strInput">Original String</param>
        /// <returns>3xMD5 Hash</returns>
        public static string HashString(string strInput)
        {
            MD5 md5Hasher = MD5.Create();
            strInput = BitConverter.ToString(md5Hasher.ComputeHash(Encoding.Default.GetBytes(BitConverter.ToString(md5Hasher.ComputeHash(Encoding.Default.GetBytes(BitConverter.ToString(md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput))))))))).Replace("-", "");
            return (strInput);
        }

        /// <summary>
        /// Creates a MD5Code of a String
        /// </summary>
        /// <param name="strInput">the string the MD5-Hash is based on</param>
        /// <returns>MD5 Hash of given String</returns>
        public static string MD5String(string strInput)
        {
            MD5 md5Hasher = MD5.Create();
            strInput = BitConverter.ToString(md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput))).Replace("-", "");                        
            return (strInput);
        }

        /// <summary>
        /// Gets the MD5 Hash of a file
        /// </summary>
        /// <param name="filename">Path to the file</param>
        /// <returns>MD5 Hash</returns>
        public static string MD5FileHash(string filename)
        {
            //Read File
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(filename);
            //Get MD5 Hash from Bytearray
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            //Convert to string
            string md5hash = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();

            return (md5hash);
        }
        #endregion

        #region Compression

        public static void CompressFile(string sInDir, string sOutFile)
        {            
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
            {
                //string sRelativePath = sInDir.Substring(iDirLen);
                string sPath = Path.GetDirectoryName(sInDir);
                string sFilename = Path.GetFileName(sInDir);
                CompressFile(sPath, sFilename, str);

                //CompressFile(sInDir, sRelativePath, str);

            }             
        }

        public static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            //Compress file name
            char[] chars = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));

            foreach (char c in chars)
                zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

            //Compress file content
            byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
            zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            zipStream.Write(bytes, 0, bytes.Length);
        }

        public static bool DecompressFile(string sDir, GZipStream zipStream)
        {
            //Decompress file name
            byte[] bytes = new byte[sizeof(int)];
            int Readed = zipStream.Read(bytes, 0, sizeof(int));
            if (Readed < sizeof(int))
                return false;

            int iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                char c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }
            string sFileName = sb.ToString();

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            int iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            string sFilePath = Path.Combine(sDir, sFileName);
            string sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!Directory.Exists(sFinalDir))
                Directory.CreateDirectory(sFinalDir);

            using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                outFile.Write(bytes, 0, iFileLen);

            return true;
        }

        public static void CompressDirectory(string sInDir, string sOutFile)
        {
            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    string sRelativePath = sFilePath.Substring(iDirLen);
                    CompressFile(sInDir, sRelativePath, str);
                }
        }

        public static void DecompressToDirectory(string sCompressedFile, string sDir)
        {
            using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
            using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                while (DecompressFile(sDir, zipStream)) ;
        }        
        #endregion
        
        /// <summary>
        /// Creates a new IniFile object that contains the main applications configuration information.
        /// If the object is already created it will return the instance
        /// </summary>
        public static IniFile GetApplicationConfiguration()
        {
            if (_AppConfig == null)
                InitiateApplicationConfiguration();

            return _AppConfig;
        }

        /// <summary>
        /// Gets a unique id for this computer.
        /// This id represents the hardware id of CPU0
        /// IMPORTANT: THIS IS USED MULTIPLE TIMES! UPDATE IT EVERYWHERE !!!
        /// </summary>
        /// <returns>String</returns>
        public static String GetUniqueComputerId()
        {
            Logger.Log(LogEntryType.Info, "Generating unique computer id...", "Helper");
            if (_ComputerId == "")
            {                
                _ComputerId = Security.FingerPrint.Value();
                Logger.Log(LogEntryType.Info, "... id generated (id: " + _ComputerId + ")", "Helper");
            }
            return _ComputerId;
        }

        /// <summary>
        /// Sets the Password for Application-Usage of the User
        /// </summary>
        /// <param name="password">The Password of the User</param>
        public static void SetUserPassword(SecureString password)
        {
            _UserPassword = password;
        }

        /// <summary>
        /// Gets the Userpassword
        /// </summary>
        /// <returns>The Userpassword as SecureString</returns>
        public static SecureString GetUserPassword()
        {
            return (_UserPassword);
        }

        /// <summary>
        /// Does the same as the "unblock"-Button in the properties-dialog of a file
        /// </summary>
        public static bool UnblockFile(string fileName)
        {
            Logger.Log(LogEntryType.Verbose, "Trying to Unblock file " + fileName);
            bool ret = DeleteFile(fileName + ":Zone.Identifier");
            Logger.Log(LogEntryType.Verbose, "   ... Unblock-Result: " + ret);
            return ret;
        }

        #region SecureString-Handling
        /// <summary>
        /// Converts a String to a SecureString
        /// </summary>
        /// <param name="unsecurePassword"></param>
        /// <returns></returns>
        public static SecureString ConvertToSecureString(string unsecurePassword)
        {
            if (unsecurePassword == "")
                return (new SecureString());

            unsafe
            {
                fixed (char* passwordChars = unsecurePassword)
                {
                    var securePassword = new SecureString(passwordChars, unsecurePassword.Length);
                    unsecurePassword = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; //Overwrite content
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }

        /// <summary>
        /// Converts a SecureString to a String
        /// </summary>
        /// <param name="securePassword"></param>
        /// <returns></returns>
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return "";
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static IEnumerable<T> MoveUp<T>(this IEnumerable<T> enumerable, int itemIndex)
        {
            int i = 0;

            IEnumerator<T> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                i++;

                if (itemIndex.Equals(i))
                {
                    T previous = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }

                    yield return previous;

                    break;
                }

                yield return enumerator.Current;
            }

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> MoveDown<T>(this IEnumerable<T> enumerable, int itemIndex)
        {
            int i = 0;

            IEnumerator<T> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                i++;

                if (itemIndex.Equals(i - 1))
                {
                    T previous = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }

                    yield return previous;

                    break;
                }

                yield return enumerator.Current;
            }

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        /// <summary>
        /// Compares two SecureStrings without using Plain-Text
        /// </summary>
        /// <param name="ss1">SecureString 1</param>
        /// <param name="ss2">SecureString 2</param>
        /// <returns>Is equal or not</returns>
        public static bool SecureStringsAreEqual(this SecureString ss1, SecureString ss2)
        {
            var bstr1 = IntPtr.Zero;
            var bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                var length1 = Marshal.ReadInt32(bstr1, -4);
                var length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (var x = 0; x < length1; ++x)
                    {
                        var b1 = Marshal.ReadByte(bstr1, x);
                        var b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }
        #endregion

        #region Code-Hashing
        //Nice Article about how (NOT) to hash: https://crackstation.net/hashing-security.htm#normalhashing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secText">The SecuirtyString that includes the password</param>
        /// <param name="hash1">The salt1-key</param>
        /// <param name="dbguid">the dbguid</param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes(SecureString secText, byte[] hash1, byte[] dbguid, byte[] iv)
        {
            //var key = new byte[dbguid.Length + hash1.Length];
            //dbguid.CopyTo(key, 0);
            //hash1.CopyTo(key, dbguid.Length);

            // Check arguments. 
            if (secText == null || secText.Length <= 0)
                throw new ArgumentNullException("secText");
            //if (key == null || key.Length <= 0)
            //    throw new ArgumentNullException("dbguid");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            byte[] encrypted;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = HashSha256(dbguid, hash1);
                rijAlg.IV = GetFirstBytes(iv, 16);

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(ConvertToUnsecureString(secText));
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        public static SecureString DecryptStringFromBytes(byte[] cipherText, byte[] hash1, byte[] dbguid, byte[] iv)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the string used to hold 
            // the decrypted text. 
            SecureString secPw;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = HashSha256(dbguid, hash1);
                rijAlg.IV = GetFirstBytes(iv, 16);  //Key-Size/8 (=32)

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            secPw = ConvertToSecureString(srDecrypt.ReadToEnd());
                        }
                    }
                }
            }
            return secPw;
        }

        /// <summary>
        /// Generates a SHA512-Hash of a string
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        private static byte[] HashSha512(byte[] part1, byte[] part2)
        {
            var alg = SHA512.Create();
            var part3 = new byte[part1.Length + part2.Length];
            part1.CopyTo(part3, 0);
            part2.CopyTo(part3, part1.Length);
            var result = alg.ComputeHash(part3);
            return (result);
        }

        /// <summary>
        /// Generates a SHA256-Hash of a string
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        private static byte[] HashSha256(byte[] part1, byte[] part2)
        {
            var alg = SHA256.Create();
            var part3 = new byte[part1.Length + part2.Length];
            part1.CopyTo(part3, 0);
            part2.CopyTo(part3, part1.Length);
            var result = alg.ComputeHash(part3);
            return (result);
        }

        private static byte[] GetFirstBytes(byte[] data, int lenght)
        {
            if (data.Length < lenght)
                return (new byte[0]);

            var res = new byte[lenght];
            for (var i = 0; i < res.Length; i++)
                res[i] = data[i];

            return (res);
        }

        /// <summary>
        /// Generates a Salt with the defined lenght
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt(int lenght)
        {
            var salt = new byte[lenght];

            var csprng = new RNGCryptoServiceProvider();
            //Fill variable salt with the cryptographic random number
            csprng.GetBytes(salt);

            return (salt);
        }

        /// <summary>
        /// Generates the PBKDF2 hash
        /// </summary>
        /// <param name="password">The password hash</param>
        /// <param name="salt">The salt</param>
        /// <param name="count">The number of iterations</param>
        /// <param name="hashLength">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        // ReSharper disable once InconsistentNaming
        private static byte[] PBKDF2(byte[] password, byte[] salt, int count, int hashLength)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, count);
            return pbkdf2.GetBytes(hashLength);
        }

        public static byte[] GetRijndaelKey(SecureString password, byte[] salt1, string dbGuid)
        {
            return (HashSha512(Encoding.UTF8.GetBytes(dbGuid), GetHash1(password, salt1)));
        }

        public static byte[] GetPasswordHash(SecureString password, byte[] salt1, byte[] salt2)
        {
            return(PBKDF2(HashSha512(GetHash1(password, salt1),  salt2), salt2, 100, salt2.Length));
        }

        public static byte[] GetHash1(byte[] salt1)
        {
            return (PBKDF2(HashSha512(Encoding.UTF8.GetBytes(ConvertToUnsecureString(_UserPassword)), salt1), salt1, 100, salt1.Length));
        }

        public static byte[] GetHash1(SecureString password, byte[] salt1)
        {
            return (PBKDF2(HashSha512(Encoding.UTF8.GetBytes(ConvertToUnsecureString(password)), salt1), salt1, 100, salt1.Length));
        }

        #endregion

        /// <summary>
        /// Checks if the string is an Integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInteger(string s)
        {
            int output;
            return int.TryParse(s, out output);
        }

        #region Encrypting/Decrypting
        /// <summary>
        /// Encrypts the string. From dotnet-snippets.de
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="Key">The key.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        private static byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        /// <summary>
        /// Encrypts the string. From dotnet-snippets.de
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public static string EncryptString(string clearText, string Password)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string. From dotnet-snippets.de
        /// </summary>
        /// <param name="cipherData">The cipher data.</param>
        /// <param name="Key">The key.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        private static byte[] DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
                byte[] decryptedData = ms.ToArray();
                return decryptedData;
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Exception, "Tried to decrypt string with impossible parameters", ea);
                return(new byte[0]);
            }
        }

        /// <summary>
        /// Decrypts the string. From dotnet-snippets.de
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }
        #endregion

        #region (De-)Serialisation
        public static string SerializeBase64(object o)
        {
            if (o == null)
                return "";

            // Serialize to a base 64 string
            var ws = new MemoryStream();
            var sf = new BinaryFormatter();
            sf.Serialize(ws, o);
            var bytes = ws.GetBuffer();
            var encodedData = bytes.Length + ":" + Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None);
            return encodedData;
        }

        public static object DeserializeBase64(string s)
        {
            if (s == "")
                return null;

            // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
            var p = s.IndexOf(':');
            var length = Convert.ToInt32(s.Substring(0, p));

            // Extract data from the base 64 string!
            var memorydata = Convert.FromBase64String(s.Substring(p + 1));
            var rs = new MemoryStream(memorydata, 0, length);
            var sf = new BinaryFormatter();
            var o = sf.Deserialize(rs);
            return o;
        }
        #endregion
    }
}

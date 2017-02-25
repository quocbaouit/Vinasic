using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Web.Configuration;

namespace Dynamic.Framework.Security
{
    public static class SerializeObject
    {
        [ConfigurationProperty("machineKey")]
        private static MachineKeySection MachineKey
        {
            get
            {
                return (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
            }
        }

        public static byte[] Serialize(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            new BinaryFormatter().Serialize((Stream)memoryStream, obj);
            byte[] numArray = memoryStream.ToArray();
            memoryStream.Close();
            return numArray;
        }

        public static object Deserialize(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            object obj = new BinaryFormatter().Deserialize((Stream)memoryStream);
            memoryStream.Close();
            return obj;
        }

        public static string Encrypt(byte[] data)
        {
            Aes aes = Aes.Create();
            if (SerializeObject.MachineKey.DecryptionKey.Length == 48)
            {
                byte[] numArray1 = Enumerable.ToArray<byte>(Enumerable.Select<char, byte>((IEnumerable<char>)Enumerable.ToArray<char>((IEnumerable<char>)SerializeObject.MachineKey.DecryptionKey.Substring(0, 32)), (Func<char, byte>)(c => Convert.ToByte(c))));
                byte[] numArray2 = Enumerable.ToArray<byte>(Enumerable.Select<char, byte>((IEnumerable<char>)Enumerable.ToArray<char>((IEnumerable<char>)SerializeObject.MachineKey.DecryptionKey.Substring(32, 16)), (Func<char, byte>)(c => Convert.ToByte(c))));
                aes.Key = numArray1;
                aes.IV = numArray2;
            }
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            string str = Convert.ToBase64String(memoryStream.ToArray());
            memoryStream.Close();
            return str;
        }

        public static byte[] Decrypt(string data)
        {
            byte[] buffer = Convert.FromBase64String(data);
            Aes aes = Aes.Create();
            if (SerializeObject.MachineKey.DecryptionKey.Length == 48)
            {
                byte[] numArray1 = Enumerable.ToArray<byte>(Enumerable.Select<char, byte>((IEnumerable<char>)Enumerable.ToArray<char>((IEnumerable<char>)SerializeObject.MachineKey.DecryptionKey.Substring(0, 32)), (Func<char, byte>)(c => Convert.ToByte(c))));
                byte[] numArray2 = Enumerable.ToArray<byte>(Enumerable.Select<char, byte>((IEnumerable<char>)Enumerable.ToArray<char>((IEnumerable<char>)SerializeObject.MachineKey.DecryptionKey.Substring(32, 16)), (Func<char, byte>)(c => Convert.ToByte(c))));
                aes.Key = numArray1;
                aes.IV = numArray2;
            }
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.Close();
            byte[] numArray = memoryStream.ToArray();
            memoryStream.Close();
            return numArray;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Rose.Services;

namespace Rose.Server
{
    public class Preprocessor : IRosePreprocessor
    {
        private readonly string AesIV, AesKey;





        public Preprocessor()
        {
            AesIV = Starter.Config.GetValue("services/preprocessor/aesIV").PadRight(16);
            AesKey = Starter.Config.GetValue("services/preprocessor/aesKey").PadRight(16);
        }


        public bool BeforeRequestHandling(HttpListenerContext httpContext, ref string messageBody)
        {
            var scriptManager = Aegis.NamedObjectManager.Find("ScriptManager");
            return scriptManager?.Call_BeforeRequestHandling(httpContext, ref messageBody) ?? true;
        }


        public void AfterRequestHandled(HttpListenerContext httpContext)
        {
        }


        public void BeforeResponseHandling(HttpListenerResponse httpResponse, ref string response)
        {
            var scriptManager = Aegis.NamedObjectManager.Find("ScriptManager");
            scriptManager?.Call_BeforeResponseHandling(httpResponse, ref response);
        }


        /*
        public virtual void Encrypt(string iv, string key)
        {
            //  Padding
            {
                int paddingByte, blockSizeInByte = 16;


                //  (Size - 2) = Encrypt data size
                //  -4 = CRC data
                paddingByte = blockSizeInByte - (Size - 2) % blockSizeInByte - 4;
                if (paddingByte < 0)
                    paddingByte = blockSizeInByte + paddingByte;

                if (paddingByte > 0)
                    Write(_tempBuffer, 0, paddingByte);
            }


            //  CRC
            {
                uint crc = GetCRC32(Buffer, 2, Size - 2);
                PutUInt32(crc);
            }


            //  Encrypt
            using (Aes aes = Aes.Create())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Key = Encoding.ASCII.GetBytes(key);
                aes.IV = Encoding.ASCII.GetBytes(iv);


                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    ushort packetSize = Size;

                    cryptoStream.Write(Buffer, 2, packetSize - 2);
                    byte[] encrypted = memoryStream.ToArray();
                    Overwrite(encrypted, 0, encrypted.Length, 2);

                    cryptoStream.Close();
                }

                memoryStream.Close();
            }
        }



        public virtual bool Decrypt(string iv, string key)
        {
            ushort packetSize = Size;


            //  Block Size가 일치하지 않으면 복호화를 할 수 없다.
            if ((packetSize - 2) % 16 != 0)
            {
                Logger.Err(LogMask.Aegis, "BlockSize is not match(packetsize={0}).", packetSize);
                return false;
            }


            //  Decrypt AES
            {
                using (Aes aes = Aes.Create())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.None;
                    aes.KeySize = 128;
                    aes.BlockSize = 128;
                    aes.Key = Encoding.ASCII.GetBytes(key);
                    aes.IV = Encoding.ASCII.GetBytes(iv);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(Buffer, 2, packetSize - 2);
                        byte[] decrypted = memoryStream.ToArray();
                        Overwrite(decrypted, 0, decrypted.Length, 2);

                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }


            //  Check CRC
            {
                uint crc = GetCRC32(Buffer, 2, packetSize - 4 - 2);
                uint packetCRC = BitConverter.ToUInt32(Buffer, packetSize - 4);


                if (crc != packetCRC)
                {
                    Logger.Err(LogMask.Aegis, "Invalid CRC.");
                    return false;
                }
            }

            return true;
        }
         */
    }
}

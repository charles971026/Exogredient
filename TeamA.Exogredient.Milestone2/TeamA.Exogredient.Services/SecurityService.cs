﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace TeamA.Exogredient.Services
{
    public static class SecurityService
    {
        /*
        private readonly static RSACryptoServiceProvider FieldRSA = new RSACryptoServiceProvider();
        // Static RSA Public key.
        private readonly static byte[] _publicKey = FieldRSA.ExportCspBlob(false);
        // Static RSA Private key.
        private readonly static byte[] _privateKey = FieldRSA.ExportCspBlob(true);
        */

        /// <summary>
        /// Return the static public key for RSA
        /// </summary>
        /// <returns>
        /// RSAParameters
        /// </returns>
        public static byte[] GetRSAPublicKey()
        {
            return StringUtilityService.HexStringToBytes(Environment.GetEnvironmentVariable("PUBLICKEY",EnvironmentVariableTarget.User));
        }

        // RSA Private key getter
        public static byte[] GetRSAPrivateKey()
        {
            return StringUtilityService.HexStringToBytes(Environment.GetEnvironmentVariable("PRIVATEKEY", EnvironmentVariableTarget.User));
        }

        // Reference:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netstandard-2.0
        /// <summary>
        /// Encrypt a string with AES.
        /// </summary>
        /// <param name="plainData"> string to be encrypted </param>
        /// <param name="key"> key to be used for encryption </param>
        /// <param name="IV"> Initialization vectore to be used for encryption </param>
        /// <returns> byte array of the encrypted data</returns>
        public static byte[] EncryptAES(string plainData, byte[] key, byte[] IV)
        {
            try
            {
                byte[] encryptedData;

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    // Create AES encryptor using the given key and IV.
                    ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);
                    MemoryStream ms = new MemoryStream();

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            // Write data to be encrypted to the crypto stream.
                            sw.Write(plainData);
                        }
                        // Get the encrypted data from the memory stream in an array.
                        encryptedData = ms.ToArray();
                    }

                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        // Reference:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netstandard-2.0
        /// <summary>
        /// Decrypt data encrypted with AES.
        /// </summary>
        /// <param name="encryptedData"> data encrypted with AES </param>
        /// <param name="key"> key used for AES encryption </param>
        /// <param name="IV"> IV used for AES encryption </param>
        /// <returns> the string of decrypted data </returns>
        public static string DecryptAES(byte[] encryptedData, byte[] key, byte[] IV)
        {
            try
            {
                string decryptedData;
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    // Create AES decryptor using the given key and IV.
                    ICryptoTransform decryptor = aes.CreateDecryptor(key, IV);

                    // Pass the encrypted data to the memory stream.
                    MemoryStream ms = new MemoryStream(encryptedData);
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            // Read the decrypted data as string from the stream reader.
                            decryptedData = sr.ReadToEnd();
                        }
                    }

                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        // Reference:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider?view=netstandard-2.0
        /// <summary>
        /// Encrypt a byte array with RSA.
        /// </summary>
        /// <param name="plainData"> a byte array to be encrypted </param>
        /// <param name="publicKey"> public key used for encryption </param>
        /// <returns> a decrypted byte array </returns>
        public static byte[] EncryptRSA(byte[] plainData, byte[] publicKey)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Pass the public key for encryption.
                    rsa.ImportCspBlob(publicKey);
                    // Encrypt the plain data.
                    decryptedData = rsa.Encrypt(plainData, false);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        // Reference:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider?view=netstandard-2.0
        /// <summary>
        /// Decrypt an encrypted byte array with RSA.
        /// </summary>
        /// <param name="encryptedData"> a byte array encrypted with RSA </param>
        /// <param name="privateKey"> private key that is paired with public key used for encryption </param>
        /// <returns> a decrypted byte array </returns>
        public static byte[] DecryptRSA(byte[] encryptedData, byte[] privateKey)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Pass the private key for decryption.
                    rsa.ImportCspBlob(privateKey);
                    // Decrypt the encrypted data.
                    decryptedData = rsa.Decrypt(encryptedData, false);
                }

                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        // Reference:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=netstandard-2.0
        /// <summary>
        /// Hash a password with PBKDF2 based on HMACSHA1.
        /// </summary>
        /// <param name="password"> password to be hashed</param>
        /// <param name="salt"> salt used for hashing </param>
        /// <param name="iterations"> number of iterations for hashing </param>
        /// <param name="hashLength"> the length of the output hashcode </param>
        /// <returns></returns>
        public static string HashWithHMACSHA256(string data)
        {
            byte[] dataBytes = StringUtilityService.HexStringToBytes(data);
            HMACSHA256 hmac = new HMACSHA256();
            byte[] hashBytes = hmac.ComputeHash(dataBytes);
            return StringUtilityService.BytesToHexString(hashBytes);

        }

        public static string HashWithKDF(string password, byte[] salt, int hashLength = 32)
        {
            IDigest SHA256 = new Sha256Digest();
            byte[] ikm = StringUtilityService.HexStringToBytes(password);
            HkdfBytesGenerator hkdf = new HkdfBytesGenerator(SHA256);
            hkdf.Init(new HkdfParameters(ikm, salt, null));
            byte[] derivedBytes = new byte[hashLength];
            hkdf.GenerateBytes(derivedBytes,0,hashLength);

            return StringUtilityService.BytesToHexString(derivedBytes);

            // Pass the password, salt, and the number of iterations.
            //asswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", iterations);
            //Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, iterations);
            // Generate a hashcode in byte array that is hashLength bytes long
            //byte[] hashBytes = pdb.GetBytes(hashLength);
            // Convert the byte array to a hex string.
            //return BytesToHexString(hashBytes);
        }

        /// <summary>
        /// Hashes the string with SHA1.
        /// </summary>
        /// <param name="str">The input to be hashed </param>
        /// <returns>Hex string of the hashed input</returns>
        public static string HashWithSHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            // Convert the password to ASCII byte array ->
            // Compute the hashcode in byte array with the ASCII byte array ->
            // Convert the hashcode byte array to a hex string
            return StringUtilityService.BytesToHexString(sha1.ComputeHash(Encoding.ASCII.GetBytes(str)));
        }
        
        /// <summary>
        /// Generate a salt.
        /// </summary>
        /// <param name="saltLength"> length of the salt in bytes </param>
        /// <returns> byte array of salt </returns>
        public static byte[] GenerateSalt(int saltLength = 8)
        {
            // Create a byte array for salt with the given salt length
            byte[] salt = new byte[saltLength];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // Fill the byte array for salt with random bytes 
                rng.GetBytes(salt);
            }
            return salt;
        }

        /* FOR TESTING PURPOSE, NEED TO BE IMPLEMENTED ON THE FRONTEND */
        /// <summary>
        /// Generate a key for AES encryption and decryption
        /// </summary>
        /// <returns> size 32 byte array of AES key </returns>
        public static byte[] GenerateAESKey()
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            return aes.Key;
        }

        /// <summary>
        /// Generate a IV for AES encryption and decryption
        /// </summary>
        /// <returns> size 16 byte array of IV </returns>
        public static byte[] GenerateAESIV()
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            return aes.IV;
        }

    }
}

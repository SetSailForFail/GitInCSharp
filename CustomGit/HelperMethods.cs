using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CustomGit
{
    public static class HelperMethods
    {
        
        // TODO accept either a file or a gitobject for hash_object method
        public static void WriteGitObject(Repository repository, string path, bool actuallyWrite = true)
        {
            if (!File.Exists(path)) return;
            GitObject obj = new GitObject.Blob(repository, path);
            WriteGitObject(repository, obj, actuallyWrite);
        }
        public static string WriteGitObject(Repository repository, GitObject obj, bool actuallyWrite = true)
        {
            var data = obj.Serialize();
            string result = obj.GetType().Name + ' ' + data.Length + ' ' + data;
            
            using SHA256 sha = new SHA256Managed();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(result));


            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }
            var hashString = sBuilder.ToString();
            
            if (actuallyWrite)
            {
                var destinationPath = Path.Combine(repository.GitDirectory.FullName, "objects",
                    hashString.Substring(0, 2), hashString.Substring(2));

                FileInfo file = new FileInfo(Path.Combine(destinationPath));
                file.Directory?.Create();

                //TODO FIND A BETTER SOLUTION
                File.WriteAllText(destinationPath + "_temp", result);
                CompressFile(destinationPath + "_temp", destinationPath);
                File.Delete(destinationPath + "_temp");

            }
            return hashString;
        }

        public static string GetGitObjectName(MemoryStream stream)
        {
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            
            string[] firstLine = sr.ReadLine()?.Split();
            return firstLine?[0];
        }

        public static GitObject ReadGitObject(Repository repository, string hash)
        {
            var decompressedFile = DecompressFile(repository, hash);
            var objectName = GetGitObjectName(decompressedFile);
            var data = GetGitObjectData(decompressedFile);
            
            switch (objectName)
            {
                case "Blob":
                    return new GitObject.Blob(repository, data);
                default:
                    return null;
            }
        }
        
        public static void CompressFile(string sourcePath, string destinationPath)
        {
            using FileStream inputStream = File.Open(sourcePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using FileStream outputStream = File.Open(Path.Combine(destinationPath),
                FileMode.OpenOrCreate, FileAccess.ReadWrite);
            
            using var compressor = new DeflateStream(outputStream, CompressionMode.Compress);
            inputStream.CopyTo(compressor);
        }

        public static MemoryStream DecompressFile(Repository repository, string hash)
        {
            var sourcePath = Path.Combine(repository.GitDirectory.FullName, "objects",
                hash.Substring(0, 2), hash.Substring(2));

            using FileStream inputStream = File.Open(sourcePath, FileMode.Open, FileAccess.ReadWrite);
            MemoryStream outputStream = new MemoryStream();
            using var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress);
            decompressor.CopyTo(outputStream);
            return outputStream;
        }

        public static string GetGitObjectData(MemoryStream ms)
        {
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            
            // ReSharper disable once RedundantJumpStatement
            while (sr.Peek() >= 0)
            {
                sr.Read();
                if ((char) sr.Peek() == '{')
                {
                    break;
                }
            }
            var json = sr.ReadToEnd();
            var parsedJson = JObject.Parse(json);
            return ((string) parsedJson["Data"]);
        }
    }
}
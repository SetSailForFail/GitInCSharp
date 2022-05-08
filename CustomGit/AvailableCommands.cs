using System;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace CustomGit
{
    public static class AvailableCommands
    {
        public static void Cat_File(Repository repo, string arg, string hash)
        {

            GitObject obj = HelperMethods.ReadGitObject(repo, hash);
            
            switch (arg)
            {
                case "-p":
                    Console.WriteLine(obj.Data);
                    break;
                case "-t":
                    Console.WriteLine(obj.GetType().Name);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        public static void Hash_Object(Repository repo, string file)
        {
            if (File.Exists(file)) HelperMethods.WriteGitObject(repo, file);
        }

        public static void Hash_Object(Repository repo, GitObject obj)
        {
            string hash = HelperMethods.WriteGitObject(repo, obj, false);
            Console.WriteLine(hash);
        }
    }
}
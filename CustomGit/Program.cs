using System;
using System.IO;
using System.Linq;
using System.Text;


namespace CustomGit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Repository repository = Start();

            FileInfo file = new FileInfo(Path.Combine(repository.GitDirectory.FullName, "code.txt"));
            
            GitObject.Blob firstBlob = new GitObject.Blob(repository, file.FullName);
            var hash = HelperMethods.WriteGitObject(repository, firstBlob);
            GitObject.Blob secondBlob = (GitObject.Blob)HelperMethods.ReadGitObject(repository, hash);
            AvailableCommands.Cat_File(repository, "-t", HelperMethods.WriteGitObject(repository, secondBlob, false));
            
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var pathToNewFile = Path.Combine(basePath, "mytest", "file.txt");
            AvailableCommands.Hash_Object(repository, pathToNewFile);
            AvailableCommands.Hash_Object(repository, secondBlob);
        }   

        public static Repository Start()
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (Directory.Exists(Path.Combine(desktopPath, "git")))
            {
                Directory.Delete(Path.Combine(desktopPath, "git"), true);
            }
            Repository repository = new Repository(desktopPath);
            
            var path = Path.Combine(desktopPath, "git", "code.txt");
            FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var content = "This is my first Line of Code";
            fs.Write(Encoding.ASCII.GetBytes(content), 0, content.Length);
            fs.Close();
            return repository;
        }
    }
}
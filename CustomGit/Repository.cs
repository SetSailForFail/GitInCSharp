using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace CustomGit
{
    public class Repository
    {
        public DirectoryInfo WorkingDirectory { get; set; }
        public DirectoryInfo GitDirectory { get; set; }
        public Repository(string path)
        {
            
            WorkingDirectory = new DirectoryInfo(path);
            GitDirectory = new DirectoryInfo(Path.Combine(path, "git"));

            Create(WorkingDirectory);
            Create(GitDirectory);
        }
        
        public void Create(DirectoryInfo dirInfo)
        {
            if (Directory.Exists(dirInfo.FullName))
            {
                return;
            }
            dirInfo.Create();

            if (dirInfo.Name == "git")
            {
                string objectPath = Path.Combine(dirInfo.FullName, "objects");
                Directory.CreateDirectory(objectPath);
            }
        }
    }
}
using System;
using System.IO;
using System.Windows.Input;
using CommandLine;

namespace CustomGit
{
    public class UserInput
    {

        [Verb("init", HelpText = "Initiate your local Repository")]
        public class Init
        {
            [Value(0, MetaName = "path", Required = true, HelpText = "Define where you want to create the Repository")]
            public string Path { get; set; }
        }

        [Verb("commit", HelpText = "Save your code changes")]
        public class Commit
        {
            [Option('m', Required = true, HelpText = "Explain what you have changed in your code")]
            public string Message { get; set; }
        }
    }
}
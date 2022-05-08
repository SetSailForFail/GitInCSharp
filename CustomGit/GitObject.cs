using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomGit
{
    public abstract class GitObject
    {
        [JsonIgnore]
        public Repository Repository { get; set; }
        public abstract string Serialize();
        public abstract void Deserialize(string data);
        public string Data { get; set; }
 
        public class Blob : GitObject

        {
            public Blob(Repository repository, string dataOrPath = null)
            {
                if (dataOrPath == null) throw new Exception();
                Data = File.Exists(dataOrPath) ? File.ReadAllText(dataOrPath) : dataOrPath;
                Repository = repository;
            }
            public override string Serialize()
            {
                var options = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };
                return JsonConvert.SerializeObject(this, options);
            }
            
            public override void Deserialize(string data)
            {
                var parsedJson = JObject.Parse(data);
                var token = (string)parsedJson["Data"];
                Data = token;
            }
        }

        public class Tree : GitObject
        {
            public Tree(Repository repository)
            {
            }

            public override string Serialize()
            {
                throw new NotImplementedException();
            }

            public override void Deserialize(string data)
            {
                throw new NotImplementedException();
            }
        }
    }
}
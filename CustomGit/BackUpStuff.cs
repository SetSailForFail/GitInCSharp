using System.IO;

namespace CustomGit
{
    public class BackUpStuff
    {
        public static void WriteToBinaryFile(string filePath, object objectToWrite, bool append = false)
        {
            using Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
        
        public static object ReadFromBinaryFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            using Stream stream = File.Open(filePath, FileMode.Open);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (object)binaryFormatter.Deserialize(stream);
        }
    }
}
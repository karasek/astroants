using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ants
{
    public class AssignmentJsonLoader : IAssignmentJsonLoader
    {
        public Assignment Load(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, false, 4096, true))
            {
                return JsonConvert.DeserializeObject<Assignment>(reader.ReadToEnd());
            }
        }

        public Assignment Load(string json)
        {
            return JsonConvert.DeserializeObject<Assignment>(json);
        }
    }
}
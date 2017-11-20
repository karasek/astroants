using System.IO;
using Utf8Json;
using Utf8Json.Resolvers;

namespace ants
{
    public class AssignmentJsonLoaderUtf8 : IAssignmentJsonLoader
    {
        public AssignmentJsonLoaderUtf8()
        {
            JsonSerializer.SetDefaultResolver(StandardResolver.CamelCase);
        }

        public Assignment Load(Stream stream)
        {
            return JsonSerializer.Deserialize<Assignment>(stream);
        }

        public Assignment Load(string json)
        {
            return JsonSerializer.Deserialize<Assignment>(json);
        }
    }
}
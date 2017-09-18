using System.IO;
using Newtonsoft.Json;

namespace ants
{
    public class Assignment
    {
        public ulong Id { get; set; }
        public ulong StartedTimestamp { get; set; }
        public AssignmentAreas Map { get; set; }
        public Position Astroants { get; set; }
        public Position Sugar { get; set; }
    }

    public class AssignmentAreas
    {
        public string[] Areas { get; set; }
    }

    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"[{X},{Y}]";
    }

    public class AssignmentJsonLoader
    {
        public Assignment Load(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Load(reader.ReadToEnd());
            }
        }

        public Assignment Load(string json)
        {
            return JsonConvert.DeserializeObject<Assignment>(json);
        }
    }
}
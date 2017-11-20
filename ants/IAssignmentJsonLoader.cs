using System.IO;

namespace ants
{
    public interface IAssignmentJsonLoader
    {
        Assignment Load(Stream stream);
        Assignment Load(string json);
    }
}
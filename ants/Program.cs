using System;
using System.Diagnostics;

namespace ants
{
    static class Program
    {
        static Assignment GetAssignment(QuadientClient client)
        {            
            var assignmentJson = client.GetAssignmentRaw().Result;
            var assignment = new AssignmentJsonLoader().Load(assignmentJson);
            return assignment;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Getting assignment.");
                var client = new QuadientClient();
                var assignment = GetAssignment(client);

                Console.WriteLine("Search path.");

                var sw = new Stopwatch();
                sw.Start();
                var path = new PathFinder().FindPath(assignment);
                Console.WriteLine($"Found path in {sw.ElapsedMilliseconds}ms: " + path);

                Console.WriteLine("Result validating response:");
                Console.WriteLine(client.CheckResponse(assignment.Id.ToString(), path).Result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed: " + e.Message);
            }
        }
    }
}

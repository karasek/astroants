namespace ants
{
    public class Assignment
    {
        public string Id { get; set; }
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
}

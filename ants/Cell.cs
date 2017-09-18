namespace ants
{
    public class Cell
    {
        public int Cost { get; set; }
        public int Price { get; set; }
        public char CameFrom { get; set; }
        public char[] Directions { get; set; } //RDLU

        public static Cell ParseFromAssignment(string assignment)
        {
            var cell = new Cell();
            var pos = assignment.IndexOf("-");
            cell.Cost = int.Parse(assignment.Substring(0, pos));
            cell.Directions = assignment.Substring(pos + 1).ToCharArray();
            cell.CameFrom = '-';
            cell.Price = int.MaxValue;
            return cell;
        }
    }
}
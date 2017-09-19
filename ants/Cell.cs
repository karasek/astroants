namespace ants
{
    public struct Cell
    {
        public int Cost { get; set; }
        public int Price { get; set; }
        public int CameFrom { get; set; }
        public int[] Directions { get; set; } //offsets
        public bool Evaluated { get; set; }

        public void Update(int price, int cameFrom)
        {
            Price = price;
            CameFrom = cameFrom;
        }

        public static Cell ParseFromAssignment(string assignment, int sizeSide)
        {
            var cell = new Cell();
            var pos = assignment.IndexOf("-");
            cell.Cost = int.Parse(assignment.Substring(0, pos));
            cell.Directions = BuildDirections(assignment, pos, sizeSide);
            cell.Price = int.MaxValue;
            return cell;
        }

        static int[] BuildDirections(string assignment, int pos, int sizeSide)
        {
            var directions = new int[assignment.Length - pos - 1];
            for (var i = pos + 1; i < assignment.Length; i++)
            {
                switch (assignment[i])
                {
                    case 'L':
                        directions[i - pos - 1] = -1;
                        break;
                    case 'R':
                        directions[i - pos - 1] = 1;
                        break;
                    case 'U':
                        directions[i - pos - 1] = -sizeSide;
                        break;
                    case 'D':
                        directions[i - pos - 1] = sizeSide;
                        break;
                }
            }
            return directions;
        }

        public static char ToDirectionLabel(int offset)
        {
            if (offset < 0)
                return offset < -1 ? 'U' : 'L';
            return offset > 1 ? 'D' : 'R';
        }
    }
}
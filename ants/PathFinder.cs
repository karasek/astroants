using System;
using System.Text;
using Priority_Queue;

namespace ants
{
    public class PathFinder
    {
        int _size, _sideSize;
        Cell[] _data;
        int _start, _goal, _goalColumn, _goalRow;
        DateTime _deadLine;

        class PartialSolutionNode : FastPriorityQueueNode
        {
            public int CellIndex { get; set; }
        }
        FastPriorityQueue<PartialSolutionNode> _partialSolutions;

        public string FindPath(Assignment assignment)
        {
            Initialize(assignment);

            while (_partialSolutions.Count > 0)
            {
                var current = _partialSolutions.Dequeue().CellIndex;
                var cell = _data[current];
                if (current == _goal && cell.Price < (_deadLine - DateTime.UtcNow).TotalMilliseconds)
                    return BuildPath(_goal);

                cell.Evaluated = true;
                _data[current] = cell;

                foreach (var d in cell.Directions)
                {
                    var neighbour = current + d;
                    var neighbourCell = _data[neighbour];
                    if (neighbourCell.Evaluated)
                        continue;
                    var pathPrice = cell.Price + neighbourCell.Cost;
                    if (pathPrice >= neighbourCell.Price)
                        continue;
                    neighbourCell.Update(pathPrice, d);
                    _data[neighbour] = neighbourCell;
                    _partialSolutions.Enqueue(new PartialSolutionNode { CellIndex = neighbour }, pathPrice + Estimate(neighbour));
                }
            }
            throw new Exception("die hard :-(");
        }

        string BuildPath(int goal)
        {
            var cell = _data[goal];
            var pos = goal;
            var path = new StringBuilder();
            while (cell.CameFrom != 0)
            {
                path.Append(Cell.ToDirectionLabel(cell.CameFrom));
                pos -= cell.CameFrom;
                cell = _data[pos];
            }
            var arr = path.ToString().ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        void Initialize(Assignment assignment)
        {
            var areas = assignment.Map.Areas;
            _size = areas.Length;
            _data = new Cell[_size];
            _sideSize = (int)Math.Sqrt(_size);
            _start = FromPosition(assignment.Astroants);
            _goal = FromPosition(assignment.Sugar);
            _goalColumn = assignment.Sugar.X;
            _goalRow = assignment.Sugar.Y;
            _partialSolutions = new FastPriorityQueue<PartialSolutionNode>(_size);
            _deadLine = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(assignment.StartedTimestamp)
                .AddMinutes(1);

            for (var i = 0; i < _size; i++)
            {
                var cell = Cell.ParseFromAssignment(areas[i], _sideSize);
                _data[i] = cell;
            }
            _data[_start].Price = 0;
            _data[_start].Cost = 0;
            _partialSolutions.Enqueue(new PartialSolutionNode { CellIndex = _start }, Estimate(_start));
        }

        int Estimate(int pos)
        {
            var row = pos / _sideSize;
            var column = pos - row * _sideSize;

            return Math.Abs(_goalRow - row) + Math.Abs(_goalColumn - column);
        }

        int FromPosition(Position pos) => pos.X + pos.Y * _sideSize;
    }
}
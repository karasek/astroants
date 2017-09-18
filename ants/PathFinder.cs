using System;
using System.Collections.Generic;
using System.Text;
using Priority_Queue;

namespace ants
{
    public class PathFinder
    {
        int _size, _sideSize;
        Cell[] _data;
        SimplePriorityQueue<int, int> _partialSolutions;
        Dictionary<int, object> _evaluated;
        int _start, _goal, _goalColumn, _goalRow;
        Dictionary<char, int> _offsets; //R->+1 D->+_sideSize ...
        DateTime _deadLine;

        public string FindPath(Assignment assignment)
        {
            Initialize(assignment);
            
            while (_partialSolutions.TryDequeue(out int current))
            {
                var cell = _data[current];
                if (current == _goal && cell.Price < (_deadLine - DateTime.UtcNow).TotalMilliseconds)
                    return BuildPath(_goal);

                _evaluated[current] = null;

                foreach (var d in cell.Directions)
                {
                    var neighbour = current + _offsets[d];
                    if (_evaluated.ContainsKey(neighbour))
                        continue;
                    var neighbourCell = _data[neighbour];
                    var pathPrice = cell.Price + neighbourCell.Cost;
                    if (pathPrice >= neighbourCell.Price)
                        continue;
                    neighbourCell.CameFrom = d;
                    neighbourCell.Price = pathPrice;
                    _partialSolutions.Enqueue(neighbour, pathPrice + Estimate(neighbour));
                }
            }
            throw new Exception("die hard :-(");
        }

        string BuildPath(int goal)
        {
            var cell = _data[goal];
            var pos = goal;
            var path = new StringBuilder();
            while (cell.CameFrom != '-')
            {
                path.Append(cell.CameFrom);
                pos -= _offsets[cell.CameFrom];
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
            _partialSolutions = new SimplePriorityQueue<int, int>();
            _evaluated = new Dictionary<int, object>();
            _offsets = new Dictionary<char, int> { ['R'] = 1, ['L'] = -1, ['U'] = -_sideSize, ['D'] = _sideSize };
            _deadLine = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(assignment.StartedTimestamp)
                .AddMinutes(1);

            for (var i = 0; i < _size; i++)
            {
                var cell = Cell.ParseFromAssignment(areas[i]);
                _data[i] = cell;
            }
            _data[_start].Price = 0;
            _data[_start].Cost = 0;
            _data[_goal].Cost = 0;
            _partialSolutions.Enqueue(_start, Estimate(_start));
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
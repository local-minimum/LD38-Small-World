using System.Collections.Generic;
using System.Linq;
using LocalMinimum.Arrays;

namespace LocalMinimum.Grid
{
    public static class GridPath
    {

        public static List<Coordinate> FindPath(Coordinate source, Coordinate target, int[,] array, int filter)
        {
            bool searching = source == target;
            Dictionary<Coordinate, KeyValuePair<int, List<Coordinate>>> explored = new Dictionary<Coordinate, KeyValuePair<int, List<Coordinate>>>();
            Dictionary<Coordinate, KeyValuePair<int, List<Coordinate>>> seen = new Dictionary<Coordinate, KeyValuePair<int, List<Coordinate>>>();
            Coordinate cur = source;
            List<Coordinate> curPath = new List<Coordinate>();
            curPath.Add(cur);
            int curLength = 1;

            while (searching) {

                explored.Add(cur, new KeyValuePair<int, List<Coordinate>>(curLength, curPath));

                int nextPathLength = explored[cur].Key + 1;
                foreach(Coordinate neigh in cur.GetNeighbours(Neighbourhood.Eight))
                {
                    if (!IsInRegion(neigh, array, filter) || explored.ContainsKey(neigh))
                    {
                        continue;
                    }
                    
                    if (neigh == target)
                    {
                        List<Coordinate> neighPath = new List<Coordinate>();
                        neighPath.AddRange(curPath);
                        neighPath.Add(neigh);
                        return neighPath;
                    }

                    if (!seen.ContainsKey(neigh) || seen[neigh].Key > nextPathLength)
                    {
                        List<Coordinate> neighPath = new List<Coordinate>();
                        neighPath.AddRange(curPath);
                        neighPath.Add(neigh);
                        seen[neigh] = new KeyValuePair<int, List<Coordinate>>(nextPathLength, neighPath);
                    }
                    
                }
                if (seen.Count == 0)
                {
                    break;
                }
                var next = seen.OrderBy(e => e.Value.Key).First();
                cur = next.Key;
                curPath.Clear();
                curPath.AddRange(next.Value.Value);
                curLength = next.Value.Key;
            }

            return new List<Coordinate>();
        }

        public static bool IsInside(Coordinate pos, int[,] array)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < array.GetLength(0) && pos.y < array.GetLength(1);
        }
        
        public static bool IsInRegion(Coordinate pos, int[,] array, int filter)
        {
            return IsInside(pos, array) && array[pos.x, pos.y] == filter;
        }

        public static bool LineInOneRegion(this bool[,] data, Coordinate source, Coordinate target)
        {
            bool sought = data[source.x, source.y];
            Coordinate cur = source;
            while (cur != target)
            {
                cur += (target - cur).NineNormalized;
                if (data[cur.x, cur.y] != sought)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client.Scripts.Extensions
{
    public class Graph
    {
        public Vector2Int Min;
        public Vector2Int Max;
        public bool[,] Edges;
        public int[,] Weights;

        public List<Vector2Int> Neighbors(Vector2Int coords)
        {
            List<Vector2Int> res = new List<Vector2Int>();
            if (coords.x > Min.x + 1 && coords.x < Max.x - 1 && coords.y > Min.y + 1 && coords.y < Max.y - 1)
            {
                if (Edges[coords.x + 1, coords.y]) res.Add(new Vector2Int(coords.x + 1, coords.y));
                if (Edges[coords.x - 1, coords.y]) res.Add(new Vector2Int(coords.x - 1, coords.y));
                if (Edges[coords.x, coords.y + 1]) res.Add(new Vector2Int(coords.x, coords.y + 1));
                if (Edges[coords.x, coords.y - 1]) res.Add(new Vector2Int(coords.x, coords.y - 1));
            }

            //Debug.Log("GRAPH: len:" + res.Count + " min: " + Min.x + " " + Min.y + " max: " + Max.x + " " + Max.y + " coords: " + coords.x + " " + coords.y);

            return res;
        }
    };


    public static class Bfs
    {
        public static void Search(Graph graph, Vector2Int start)
        {
            if (start.x <= graph.Min.x || start.x >= graph.Max.x || start.y <= graph.Min.y || start.y >= graph.Max.y)
            {
                Debug.Log("ERROR START");
                return;
            }
            var frontier = new Queue<Vector2Int>();
            frontier.Enqueue(start);
            int w = 0;
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            visited.Add(start);

            while (frontier.Count > 0)
            {
                Vector2Int current = frontier.Dequeue();
                graph.Weights[current.x, current.y] = w++;
                foreach (var next in graph.Neighbors(current))
                {
                    if (!visited.Contains(next))
                    {
                        frontier.Enqueue(next);
                        visited.Add(next);
                    }
                }
            }
        }
    }
}
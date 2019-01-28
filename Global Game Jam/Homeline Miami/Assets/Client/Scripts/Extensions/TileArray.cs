using UnityEngine;
using UnityEngine.Tilemaps;


namespace Client.Scripts.Extensions
{
    public static class TileArray
    {
        public static Graph Get(Tilemap tilemap, int size)
        {
            Graph graph = new Graph();
            graph.Min = new Vector2Int(0, 0);
            graph.Max = new Vector2Int(size, size);
            graph.Edges = new bool[size, size];
            graph.Weights = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    graph.Edges[i, k] = tilemap.GetTile(new Vector3Int(i, k, 0)) == null;
                }
            }
            return graph;
        }
    }
}
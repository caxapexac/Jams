using System.Collections.Generic;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    
    public class WorldTile : MonoBehaviour
    {
        public List<WorldTile> myNeighbours;
        public int gridX;
        public int gridY;
        public bool walkable;
    }
}
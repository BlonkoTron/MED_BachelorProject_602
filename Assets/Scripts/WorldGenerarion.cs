using System.Collections.Generic;
using UnityEngine;

public class WorldGenerarion : MonoBehaviour
{
    [Header("Testing")]
    public bool Generate;

     [Header("Spawn settings")]
     [Tooltip("Spawnrate of tiles")] 
     public int spawnrate;

     [Tooltip("Spawndistance to change the distance from tile to tile")] 
     public int spawdistance;

     [Tooltip("Spawncount to set how many tiles occur")] 
     public int spawcount;


    public List<Tile> tiles;

   [System.Serializable]
    public class Tile
    {
        public string name;
        public GameObject prefab;
        public int WorldWeight;
    }

    void FixedUpdate()
    {
        if (Generate)
        {
            Generate = false;
            Spawnsequence();
        }


    }

    void Spawnsequence()
    {
        Debug.Log("Spawning tiles");

        if (tiles.Count == 0)
        {
            Debug.LogWarning("No tiles assigned!");
            return;
        }

        if (tiles.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;

            for (int i = 0; i < spawcount; i++)
            {
            Tile randomTile = tiles[Random.Range(0, tiles.Count)];

            Instantiate(randomTile.prefab, spawnPosition, Quaternion.identity);

            // Move position forward for next tile
            spawnPosition += new Vector3(0, 0, spawdistance);
            }
    }
}

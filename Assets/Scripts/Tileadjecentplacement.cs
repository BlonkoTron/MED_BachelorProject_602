using System.Collections.Generic;
using UnityEngine;

public class Tileadjecentplacement : MonoBehaviour
{
    public GameObject hexPrefab; //Which gameobject should spawn
    public int radius = 1; // 1 = 7 tiles, 2 = 19 tiles, Controls the amount
    public float hexSize = 1f; //Size of hex, The bigger the more space between hexes

    public GameObject testnewtile;

    private Dictionary<Vector2Int, Tileinfo> grid = new Dictionary<Vector2Int, Tileinfo>(); // Creates a dictionary which acts like a grid/map where each position (Vector2Int) stores the tile data (Tileinfo).
    
    public Material[] variants; // Materialpick
    public bool testrandom = false;

    // Axial directions (6 neighbors)
    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };

    void Start()
    {
        GenerateGrid();
        AssignNeighbors();
    }

    private void Update()
    {
        if (testrandom == true)
        {
            Tileinfo randomTile = GetRandomTile();
            //randomTile = testnewtile.gameObject;
            testrandom = false;
        }
    }

    void GenerateGrid()
    {
        //Find Top gameobject to refrence its material later
        Transform topTransform = hexPrefab.transform.Find("Top");
        GameObject topHex = topTransform.gameObject;

        // Loop over q (one axis in axial hex coordinates)
        for (int q = -radius; q <= radius; q++) // Generates columns of hexes within the radius
        {
            // Loop over r (the other axis), but constrained so the shape becomes a hex, not a square
            for (int r = Mathf.Max(-radius, -q - radius); r <= Mathf.Min(radius, -q + radius); r++)
            {
                //Get random material and apply to topHex
                int randomIndex = Random.Range(0, variants.Length);
                topHex.GetComponent<Renderer>().material = variants[randomIndex];

                // Convert hex grid coordinates (q, r) into a world position (x, z in Unity)
                Vector3 worldPos = HexToWorld(q, r);

                // Create (spawn) a hex tile prefab at that position
                GameObject obj = Instantiate(hexPrefab, worldPos, Quaternion.identity, transform);

                // Get the Tileinfo component attached to the spawned object
                Tileinfo tile = obj.GetComponent<Tileinfo>();

                // Store the tile's grid coordinates inside the Tileinfo script
                tile.SetCoordinates(q, r);

                // Add this tile to the dictionary using its (q, r) as the key
                grid[new Vector2Int(q, r)] = tile;
            }
        }
    }

    void AssignNeighbors()
    {
        // Loop through every tile stored in the grid dictionary
        foreach (var kvp in grid)
        {
            Tileinfo tile = kvp.Value;

            // Find and assign all neighboring tiles for this tile
            tile.neighbors = GetNeighbors(tile);
        }
    }

    List<Tileinfo> GetNeighbors(Tileinfo tile)
    {
        // Create a list to store neighboring tiles
        List<Tileinfo> result = new List<Tileinfo>();

        // Loop through all possible hex directions (6 directions)
        foreach (var dir in directions)
        {
            // Calculate the position of the neighbor by adding direction offset
            Vector2Int neighborPos = new Vector2Int(tile.x + dir.x, tile.y + dir.y);

            // Check if a tile exists at that position in the grid
            if (grid.TryGetValue(neighborPos, out Tileinfo neighbor))
            {
                // If it exists, add it to the neighbor list
                result.Add(neighbor);
            }
        }
        // Return the list of valid neighbors
        return result;
    }

    // Convert axial hex coordinates (q, r) into Unity world position
    // This is for "pointy-top" hex layout
    Vector3 HexToWorld(int q, int r)
    {
        // Calculate horizontal position
        float x = hexSize * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);

        // Calculate depth (forward/back) position
        float z = hexSize * (3f / 2f * r);

        // Return the final 3D position (y is 0 since it's flat on ground)
        return new Vector3(x, 0, z);
    }

    public Tileinfo GetRandomTile()
    {
        if (grid.Count == 0)
            return null;

        int randomIndex = Random.Range(0, grid.Count);

        int i = 0;
        foreach (var tile in grid.Values)
        {
            if (i == randomIndex)
                return tile;
            i++;
        }

        return null; // fallback (should never hit)
    }
}
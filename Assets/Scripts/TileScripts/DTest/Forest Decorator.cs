using UnityEngine;

public class ForestDecorator : MonoBehaviour
{
    
    public GameObject spawnPrefab;
    public Transform spawnLocation;
    public float spawnYOffset = 0f;
    [Range(0, 100)] public float spawnChance = 50f;


    public LayerMask forestLayer;
    public float searchRadius = 2f;
    public float searchHeight = 5f;

    void Awake()
    {
        if (Application.isPlaying) Invoke("CheckNeighbors", 0.5f);
    }

    void CheckNeighbors()
    {
        Vector3 p1 = transform.position + Vector3.up * (searchHeight / 2);
        Vector3 p2 = transform.position + Vector3.down * (searchHeight / 2);

        Collider[] hits = Physics.OverlapCapsule(p1, p2, searchRadius, forestLayer);

        int forestCount = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (hit.CompareTag("Forest")) forestCount++;
        }

        if (forestCount >= 6 && Random.Range(0f, 100f) <= spawnChance)
        {
            if (spawnPrefab && spawnLocation)
            {
               
                Vector3 finalSpawnPos = spawnLocation.position + new Vector3(0, spawnYOffset, 0);
                Instantiate(spawnPrefab, finalSpawnPos, Quaternion.identity, spawnLocation);
            }
        }
    }
    /*
    // Draws a simple cyan box representing the search area in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(searchRadius * 2, searchHeight, searchRadius * 2));
    }
    */
}
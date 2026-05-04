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
    public float searchYOffset = 0f;

    void Awake()
    {
        if (Application.isPlaying) Invoke("CheckNeighbors", 0.5f);
    }

    void CheckNeighbors()
    {
        Vector3 searchCenter = transform.position + new Vector3(0, searchYOffset, 0);

        Vector3 halfExtents = new Vector3(searchRadius, searchHeight / 2f, searchRadius);
        Collider[] hits = Physics.OverlapBox(searchCenter, halfExtents, Quaternion.identity, forestLayer);

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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 gizmoCenter = transform.position + new Vector3(0, searchYOffset, 0);

        // Drawing the 1:1 Cube representation
        // We multiply searchRadius by 2 because the wire cube uses total size, not extents
        Vector3 gizmoSize = new Vector3(searchRadius * 2, searchHeight, searchRadius * 2);
        Gizmos.DrawWireCube(gizmoCenter, gizmoSize);
    }
    */
}
using UnityEngine;

public class ForestSwapper : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] randomPrefabs;
    public GameObject poofPrefab;

    void Start()
    {
        Swap();
    }

    void Swap()
    {
        GameObject spawnedObject = null;

        // 1. Spawn the random prefab
        if (randomPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, randomPrefabs.Length);

            // Spawn as child of current parent (Addon AttachPoint)
            spawnedObject = Instantiate(
                randomPrefabs[randomIndex],
                transform.position,
                transform.rotation,
                transform.parent
            );

            spawnedObject.transform.localScale = transform.localScale;
        }

        Tile parentTile = GetComponentInParent<Tile>();
        if (parentTile != null && spawnedObject != null)
        {
            parentTile.currentAddOn = spawnedObject;
        }
        else if (parentTile == null)
        {
            Debug.LogWarning("ForestSwapper couldn't find a 'Tile' script in parents!");
        }

        if (poofPrefab != null)
        {
            Instantiate(poofPrefab, Vector3.zero, Quaternion.identity);
        }

        // 4. Cleanup
        Destroy(gameObject);
    }
}
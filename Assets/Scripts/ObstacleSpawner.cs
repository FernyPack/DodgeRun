using UnityEngine;

// Spawns obstacles and the occasional health pickup
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;
    public float secondsBetweenSpawns = 0.75f;
    [Range(0f, 1f)] public float pickupChance = 0.15f;
    public float spawnWidth = 9f;

    float spawnTimer;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        // count real time, not frames
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= secondsBetweenSpawns)
        {
            spawnTimer -= secondsBetweenSpawns;
            Spawn();
        }
    }

    void Spawn()
    {
        bool spawnPickup = pickupPrefab != null && Random.value < pickupChance;
        GameObject prefab = spawnPickup ? pickupPrefab : obstaclePrefab;
        if (prefab == null) return;

        Vector3 pos = transform.position + new Vector3(Random.Range(-spawnWidth, spawnWidth), 0f, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}

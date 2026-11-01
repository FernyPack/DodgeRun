using UnityEngine;

// Spawns obstacles (and the occasional health pickup) at the far end of the track.
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;
    public int framesBetweenSpawns = 45;
    [Range(0f, 1f)] public float pickupChance = 0.15f;
    public float spawnWidth = 9f;

    int frameCounter;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        frameCounter++;
        if (frameCounter >= framesBetweenSpawns)
        {
            frameCounter = 0;
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

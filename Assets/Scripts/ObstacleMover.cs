using UnityEngine;

// Moves obstacles and pickups down the track toward the player.
public class ObstacleMover : MonoBehaviour
{
    public float speed = 0.3f;        // distance moved every frame
    public float despawnZ = -15f;

    void Update()
    {
        transform.Translate(Vector3.back * speed, Space.World);

        if (transform.position.z < despawnZ)
            Destroy(gameObject);
    }
}

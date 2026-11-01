using UnityEngine;

// Dodge Run - moves the player and stores the player's health.
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.15f;   // distance moved every frame
    public float xLimit = 9f;
    public float zMin = -2f;
    public float zMax = 4f;

    [Header("Health")]
    public int maxHealth = 3;
    public int health = 3;            // public, so any script can change it

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, 0f, v).normalized;

        transform.position += move * moveSpeed;

        // keep the player on the track
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -xLimit, xLimit);
        p.z = Mathf.Clamp(p.z, zMin, zMax);
        transform.position = p;
    }
}

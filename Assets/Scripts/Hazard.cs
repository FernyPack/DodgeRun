using UnityEngine;

// Obstacle that hurts the player on contact.
public class Hazard : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        player.health -= damage;
        Destroy(gameObject);
    }
}

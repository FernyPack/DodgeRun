using UnityEngine;

// Obstacle that hurts the player on contact.
public class Hazard : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        player.Health -= damage;      // property clamps at 0 and restarts the game if needed
        Destroy(gameObject);
    }
}

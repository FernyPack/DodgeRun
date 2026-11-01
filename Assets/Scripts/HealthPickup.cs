using UnityEngine;

// Green orb that gives the player health back.
public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        player.Health += healAmount;  // property clamps at MaxHealth
        Destroy(gameObject);
    }
}

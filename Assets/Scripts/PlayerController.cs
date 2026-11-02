using UnityEngine;
using UnityEngine.InputSystem;

// Dodge Run - moves the player and owns the player's health.
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 9f;      // units per SECOND (same feel as 0.15/frame at 60 FPS)
    public float xLimit = 9f;
    public float zMin = -2f;
    public float zMax = 4f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    private int health;               // private backing field - only the property touches it
    private bool isDead;

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    // Every change to health goes through here, so the rules are enforced in one place:
    // the value is clamped to 0..maxHealth, and reaching 0 restarts the game automatically.
    public int Health
    {
        get { return health; }
        set
        {
            if (isDead) return;                          // ignore hits/heals after death

            health = Mathf.Clamp(value, 0, maxHealth);

            if (health == 0)
            {
                isDead = true;
                if (GameManager.Instance != null)
                    GameManager.Instance.RestartGame();
            }
        }
    }

    void Awake()
    {
        health = maxHealth;
    }

    void Update()
    {
        // Read WASD / arrow keys
        float h = 0f, v = 0f;
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  h -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v -= 1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v += 1f;
        }
        Vector3 move = new Vector3(h, 0f, v).normalized;

        // units/second * seconds this frame = distance this frame
        transform.position += move * moveSpeed * Time.deltaTime;

        // keep the player on the track
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -xLimit, xLimit);
        p.z = Mathf.Clamp(p.z, zMin, zMax);
        transform.position = p;
    }
}
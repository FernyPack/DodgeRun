# Refactor & Encapsulate — Dodge Run

**Repository:** https://github.com/YOUR-USERNAME/DodgeRun
**Before/after diff:** see the commit *"Refactor: frame-rate independent movement + encapsulated Health property"* in the repo history.

Dodge Run is a 3D dodging game: move left/right (and a little forward/back) to avoid red blocks rushing down the track, and grab green orbs to heal. Each hit costs 1 health; at 0 health the game restarts.

---

## 1) Frame-rate independence

### Before
```csharp
// PlayerController.cs
public float moveSpeed = 0.15f;                      // distance per FRAME
transform.position += move * moveSpeed;

// ObstacleMover.cs
public float speed = 0.3f;
transform.Translate(Vector3.back * speed, Space.World);

// CameraFollow.cs
transform.position = Vector3.Lerp(transform.position, desired, 0.1f);

// ObstacleSpawner.cs
frameCounter++;
if (frameCounter >= framesBetweenSpawns) { ... }     // 45 frames
```

### After
```csharp
// PlayerController.cs
public float moveSpeed = 9f;                         // units per SECOND
transform.position += move * moveSpeed * Time.deltaTime;

// ObstacleMover.cs
public float speed = 18f;
transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

// CameraFollow.cs
float t = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
transform.position = Vector3.Lerp(transform.position, desired, t);

// ObstacleSpawner.cs
spawnTimer += Time.deltaTime;
if (spawnTimer >= secondsBetweenSpawns) { ... }      // 0.75 s
```
(Also fixed: `Rotator` now uses degrees per second, and the survival timer adds `Time.deltaTime` instead of assuming every frame is 1/60 s.)

### Explanation
Before the refactor, every moving script moved a fixed amount each frame, so the speed of the game depended on how fast the device could draw frames. On a 144 FPS laptop the player and obstacles moved more than twice as fast as on a 60 FPS monitor, while on a phone running at 30 FPS everything moved in slow motion, so players on different devices were not playing the same game. `Time.deltaTime` is how many seconds the last frame took, so multiplying a speed in units per second by it gives the correct distance for that frame at any frame rate. I applied this to the player, camera, obstacles and their rotation, and replaced the frame-counting spawner and timer with real seconds. Now the game runs at the same speed everywhere, which I checked by locking the game to 30, 60 and 144 FPS with the 1/2/3 keys.

---

## 2) Encapsulated Health

### Before
```csharp
// PlayerController.cs
public int maxHealth = 3;
public int health = 3;                 // any script can set this to anything

// Hazard.cs / HealthPickup.cs
player.health -= damage;               // can go below 0
player.health += healAmount;           // can go above maxHealth

// GameManager.cs — checked manually every frame
if (player.health <= 0) { RestartGame(); }
```

### After
```csharp
// PlayerController.cs
[SerializeField] private int maxHealth = 3;
private int health;                    // private backing field
private bool isDead;

public int MaxHealth { get { return maxHealth; } }

public int Health
{
    get { return health; }
    set
    {
        if (isDead) return;
        health = Mathf.Clamp(value, 0, maxHealth);
        if (health == 0)
        {
            isDead = true;
            GameManager.Instance.RestartGame();
        }
    }
}

// Hazard.cs / HealthPickup.cs
player.Health -= damage;
player.Health += healAmount;

// GameManager.cs — the per-frame health check is gone
```

### Explanation
Before, health was a public variable, so the obstacle and pickup scripts changed it directly, which let health drop below 0 or go above the maximum when you grabbed an orb at full health. The GameManager also had to check `health <= 0` every frame to know when to restart. Now health is a private backing field, and the only way to change it is through the public `Health` property, whose setter clamps the value between 0 and `MaxHealth`. When the value reaches 0, the setter calls `GameManager.RestartGame()` itself, so no other script has to remember to check for death. This keeps all the health rules in one place, which makes the code safer and easier to change later.

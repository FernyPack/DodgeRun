# Dodge Run

A small 3D Unity game: dodge the red blocks rushing down the track, grab green orbs to heal, survive as long as you can. Built for the **Refactor & Encapsulate** activity — see [SUBMISSION.md](SUBMISSION.md) for the before/after snippets and explanations.

## Controls
| Key | Action |
|---|---|
| A / D or ← / → | Move left / right |
| W / S or ↑ / ↓ | Move forward / back a little |
| 1 / 2 / 3 / 0 | Lock frame rate to 30 / 60 / 144 / unlimited (for testing) |

## Open it in Unity
Works with Unity 2022.3 LTS or Unity 6 (Built-in, URP or HDRP).

1. In **Unity Hub**, click **Add → Add project from disk** and select this `DodgeRun` folder. Pick an installed editor version if asked. Unity creates the missing `Library`, `Packages` and `ProjectSettings` folders on first open.
   - *If Hub won't add it:* create a new **3D** project called `DodgeRun` in Hub, close Unity, copy everything from this folder (including the hidden `.git` folder and `.gitignore`) into the new project folder, and reopen.
2. In the menu bar, click **Tools → Dodge Run → Build Scene**. This creates the track, player, camera, obstacle/pickup prefabs and saves `Assets/Scenes/DodgeRun.unity`.
3. Press **Play**.

**Error "You are trying to read Input using the UnityEngine.Input class…"?** Go to **Edit → Project Settings → Player → Other Settings → Active Input Handling** and choose **Both**, then let Unity restart.

## Seeing the refactor in action
The repo has two code commits:
1. `Dodge Run: initial version (before refactor)` — movement is per frame, health is a public field.
2. `Refactor: frame-rate independent movement + encapsulated Health property` — the improved version.

To feel the difference, check out the first commit, press **1** (30 FPS) then **3** (144 FPS) while playing: everything changes speed. On the refactored version the speed stays the same.

## Put it on GitHub
1. On github.com, create a new **empty** repository named `DodgeRun` (no README, no .gitignore).
2. After opening the project in Unity and building the scene, run in this folder:
   ```bash
   git add -A
   git commit -m "Add Unity project settings, scene and prefabs"
   git remote add origin https://github.com/YOUR-USERNAME/DodgeRun.git
   git push -u origin main
   ```
   Or with **GitHub Desktop**: File → Add local repository → choose this folder → commit → Publish repository.

## Scripts
| Script | What it does |
|---|---|
| `PlayerController` | Movement + the encapsulated `Health` property |
| `CameraFollow` | Smooth, frame-rate independent follow camera |
| `ObstacleMover` | Moves obstacles/pickups toward the player |
| `Rotator` | Spins obstacles/pickups |
| `ObstacleSpawner` | Spawns obstacles and occasional pickups on a timer |
| `Hazard` / `HealthPickup` | Damage / heal the player on contact |
| `GameManager` | Survival timer, HUD, restart |
| `FrameRateTester` | Lock FPS to test frame-rate independence |
| `Editor/DodgeRunSceneBuilder` | One-click scene setup |

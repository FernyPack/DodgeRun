using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Builds the whole Dodge Run scene for you.
// In Unity: Tools > Dodge Run > Build Scene, then press Play.
public static class DodgeRunSceneBuilder
{
    const string ScenePath = "Assets/Scenes/DodgeRun.unity";

    [MenuItem("Tools/Dodge Run/Build Scene")]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        EnsureFolder("Assets", "Scenes");
        EnsureFolder("Assets", "Materials");
        EnsureFolder("Assets", "Prefabs");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Works with Built-in, URP or HDRP: reuse whatever shader a default cube gets.
        Shader shader = DefaultShader();
        Material groundMat = MakeMaterial("Ground", new Color(0.20f, 0.22f, 0.27f), shader);
        Material playerMat = MakeMaterial("Player", new Color(0.20f, 0.55f, 1.00f), shader);
        Material obstacleMat = MakeMaterial("Obstacle", new Color(0.95f, 0.30f, 0.25f), shader);
        Material pickupMat = MakeMaterial("Pickup", new Color(0.30f, 0.90f, 0.40f), shader);

        // Track: 20 wide, 100 long
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0f, 0f, 35f);
        ground.transform.localScale = new Vector3(2f, 1f, 10f);
        ground.GetComponent<Renderer>().sharedMaterial = groundMat;

        // Player
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);
        player.GetComponent<Renderer>().sharedMaterial = playerMat;
        PlayerController controller = player.AddComponent<PlayerController>();

        // Prefabs
        GameObject obstaclePrefab = BuildMovingPrefab("Obstacle", PrimitiveType.Cube, 1.5f, obstacleMat, true);
        GameObject pickupPrefab = BuildMovingPrefab("HealthPickup", PrimitiveType.Sphere, 0.9f, pickupMat, false);

        // Spawner at the far end of the track
        GameObject spawnerGo = new GameObject("ObstacleSpawner");
        spawnerGo.transform.position = new Vector3(0f, 0.75f, 70f);
        ObstacleSpawner spawner = spawnerGo.AddComponent<ObstacleSpawner>();
        spawner.obstaclePrefab = obstaclePrefab;
        spawner.pickupPrefab = pickupPrefab;

        // Game manager + frame-rate testing tool
        GameObject managerGo = new GameObject("GameManager");
        GameManager manager = managerGo.AddComponent<GameManager>();
        manager.player = controller;
        managerGo.AddComponent<FrameRateTester>();

        // Camera
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0f, 6f, -9f);
            cam.transform.LookAt(new Vector3(0f, 1f, 4f));
            CameraFollow follow = cam.gameObject.AddComponent<CameraFollow>();
            follow.target = player.transform;
        }

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuildSettings(ScenePath);
        AssetDatabase.SaveAssets();

        Debug.Log("Dodge Run scene built at " + ScenePath + ". Press Play!");
    }

    static GameObject BuildMovingPrefab(string name, PrimitiveType shape, float size, Material mat, bool isHazard)
    {
        GameObject go = GameObject.CreatePrimitive(shape);
        go.name = name;
        go.transform.localScale = Vector3.one * size;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        go.GetComponent<Collider>().isTrigger = true;

        // Trigger events need a Rigidbody on one side; kinematic so physics doesn't push it.
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        go.AddComponent<ObstacleMover>();
        go.AddComponent<Rotator>();
        if (isHazard) go.AddComponent<Hazard>();
        else go.AddComponent<HealthPickup>();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, "Assets/Prefabs/" + name + ".prefab");
        Object.DestroyImmediate(go);
        return prefab;
    }

    static Shader DefaultShader()
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Shader shader = temp.GetComponent<Renderer>().sharedMaterial.shader;
        Object.DestroyImmediate(temp);
        return shader;
    }

    static Material MakeMaterial(string name, Color color, Shader shader)
    {
        string path = "Assets/Materials/" + name + ".mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }
        mat.color = color;
        EditorUtility.SetDirty(mat);
        return mat;
    }

    static void EnsureFolder(string parent, string child)
    {
        if (!AssetDatabase.IsValidFolder(parent + "/" + child))
            AssetDatabase.CreateFolder(parent, child);
    }

    static void AddSceneToBuildSettings(string path)
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!scenes.Exists(s => s.path == path))
        {
            scenes.Insert(0, new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}

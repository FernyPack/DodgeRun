using UnityEngine;

// Testing tool: lock the game to different frame rates to see how movement behaves.
// 1 = 30 FPS, 2 = 60 FPS, 3 = 144 FPS, 0 = unlimited.
public class FrameRateTester : MonoBehaviour
{
    int target = -1;
    int frames;
    float timer;
    float measuredFps;
    GUIStyle style;

    void Start()
    {
        QualitySettings.vSyncCount = 0;   // vSync would override targetFrameRate
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetTarget(30);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetTarget(60);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetTarget(144);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SetTarget(-1);

        // average the real FPS over half a second
        frames++;
        timer += Time.unscaledDeltaTime;
        if (timer >= 0.5f)
        {
            measuredFps = frames / timer;
            frames = 0;
            timer = 0f;
        }
    }

    void SetTarget(int fps)
    {
        target = fps;
        Application.targetFrameRate = fps;
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label) { fontSize = 18, alignment = TextAnchor.UpperRight };
            style.normal.textColor = Color.yellow;
        }

        string targetText = target < 0 ? "unlimited" : target.ToString();
        GUI.Label(new Rect(Screen.width - 420, 15, 400, 60),
            "FPS: " + measuredFps.ToString("0") + "  (target " + targetText + ")\n1=30  2=60  3=144  0=unlimited", style);
    }
}

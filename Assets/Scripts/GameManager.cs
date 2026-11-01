using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tracks survival time, draws the HUD and restarts the game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;
    public float restartDelay = 1.5f;

    public bool IsGameOver { get; private set; }

    float survivalTime;
    GUIStyle hudStyle;
    GUIStyle bigStyle;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (IsGameOver) return;

        survivalTime += Time.deltaTime;   // real seconds, whatever the frame rate

        // No health check here anymore - PlayerController.Health calls RestartGame() itself.
    }

    // Called by PlayerController.Health when it reaches 0.
    public void RestartGame()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnGUI()
    {
        if (hudStyle == null)
        {
            hudStyle = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.Bold };
            hudStyle.normal.textColor = Color.white;
            bigStyle = new GUIStyle(hudStyle) { fontSize = 48, alignment = TextAnchor.MiddleCenter };
        }

        GUI.Label(new Rect(20, 15, 500, 40), "Health: " + player.Health + " / " + player.MaxHealth, hudStyle);
        GUI.Label(new Rect(20, 50, 500, 40), "Time: " + survivalTime.ToString("0.0") + "s", hudStyle);

        if (IsGameOver)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "GAME OVER\nrestarting...", bigStyle);
    }
}

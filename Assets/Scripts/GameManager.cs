using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void GameOver(string reason)
    {
        Debug.Log($"GAME OVER: {reason}");
        // if (gameOverPanel) gameOverPanel.SetActive(true);
        // Time.timeScale = 0f;
    }
}

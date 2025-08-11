using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public static GameManager Instance;

    public int puntuacionTotal;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Se mantiene al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Asegúrate de que el juego corre al entrar a cualquier escena

    }

    public void SetPuntaje(int puntos)
    {
        Debug.Log($"nuevos puntos en gm {puntos}");
        puntuacionTotal = puntos;
    }

    public void ReiniciarPuntaje()
    {
        puntuacionTotal = 0;
    }
    public void GameOver(string reason)
    {
        Debug.Log($"GAME OVER: {reason}");
        SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);
        // if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}



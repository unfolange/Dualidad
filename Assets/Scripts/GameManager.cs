using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int puntuacionTotal;

    private void Awake()
    {
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

    public void SumarPuntaje(int puntos)
    {
        puntuacionTotal += puntos;
    }

    public void ReiniciarPuntaje()
    {
        puntuacionTotal = 0;
    }

    public void GameOverChangeScene()
    {
        // Cambia a la escena de Game Over
        SceneManager.LoadScene("Game Over");
    }
}
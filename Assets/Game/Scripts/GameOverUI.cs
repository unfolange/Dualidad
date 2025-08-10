using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text puntajeTexto;

    private void Start()
    {
        // Mostrar puntaje
        if (GameManager.Instance != null)
        {
            puntajeTexto.text = "Puntaje: " + GameManager.Instance.puntuacionTotal;
        }
        else
        {
            puntajeTexto.text = "Puntaje: 0";
        }
    }

        // Asignar botones
    public void OnClickReintentar()
    {
        SceneManager.LoadScene("room");
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

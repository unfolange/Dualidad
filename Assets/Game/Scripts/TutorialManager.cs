using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Paneles del tutorial")]
    [SerializeField] private GameObject[] panelesTutorial;

    [Header("Botones")]
    [SerializeField] private Button botonSiguiente;
    [SerializeField] private Button botonComenzar;
    [SerializeField] private Button botonSkip;

    private int panelActual = 0;

    private void Start()
    {
        MostrarSoloPanel(panelActual);

        botonSiguiente.onClick.AddListener(MostrarSiguientePanel);
        botonComenzar.onClick.AddListener(OnClickStart);
        botonSkip.onClick.AddListener(OmitirTutorial);

        botonComenzar.gameObject.SetActive(false); // Ocultamos el botón comenzar al inicio
    }

    private void MostrarSiguientePanel()
    {
        panelActual++;

        if (panelActual < panelesTutorial.Length - 1)
        {
            MostrarSoloPanel(panelActual);
        }
        else
        {
            // Último panel
            MostrarSoloPanel(panelActual);
            botonSiguiente.gameObject.SetActive(false);     // Oculta botón Next
            botonComenzar.gameObject.SetActive(true);       // Muestra botón Comenzar
        }
    }

    private void MostrarSoloPanel(int index)
    {
        for (int i = 0; i < panelesTutorial.Length; i++)
        {
            panelesTutorial[i].SetActive(i == index);
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("room"); 
    }

    private void OmitirTutorial()
    {
        panelActual = panelesTutorial.Length - 1;
        MostrarSoloPanel(panelActual);

        // Cambia botones
        botonSiguiente.gameObject.SetActive(false);
        botonComenzar.gameObject.SetActive(true);
    }
}

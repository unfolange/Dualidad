using UnityEngine;
using TMPro; // Importante si usas TextMeshProUGUI
using UnityEngine.UI; 

public class NinoController : MonoBehaviour
{
    public Animator animator; // Debe tener parámetro int "estado"
    public EstadoNino estadoActual = EstadoNino.Playing;

    public int score = 0;
    public float pointsPerSecond = 10f; // puntos por segundo mientras se mantiene presionado
    public TextMeshProUGUI scoreText;   // arrástralo en el Inspector

    float scoreAccum = 0f; // acumulador en float para sumar suave por Time.deltaTime

    //barra de carga simula avance del juego del niño
    [SerializeField] private Slider barraDeCarga;
    [SerializeField] private float velocidadCarga = 0.15f; // Llenado de la barra por segundo

    void Start()
    {
        // Mostrar el score inicial y avisar si no está asignado
        if (scoreText == null)
        {
            Debug.LogWarning("[NinoController] scoreText NO está asignado en el Inspector. " +
                             "Crea un TextMeshPro - Text (UI) en un Canvas y arrástralo aquí.");
        }
        else
        {
            scoreText.text = "Score: " + score;
        }
    }

    void Update()
    {
        bool pressing = Input.GetMouseButton(0) || Input.touchCount > 0;
        if (Input.GetKeyDown(KeyCode.Space)) // pulsa barra espaciadora
        {
            Debug.Log($"Viewport dimensions: {Screen.width} x {Screen.height}");
        }
        if (pressing && estadoActual == EstadoNino.Playing)
        {
            scoreAccum += pointsPerSecond * Time.deltaTime;

            // Actualizar la barra de carga
            LlenarBarra();

            int newScore = Mathf.FloorToInt(scoreAccum);
            if (newScore != score)
            {
                score = newScore;

                if (scoreText != null)
                    scoreText.text = "Score: " + score;
                else
                    Debug.LogWarning("[NinoController] scoreText es NULL: asigna el TextMeshProUGUI en el Inspector.");
            }
        }
    }

    private void LlenarBarra()
    {
        if (barraDeCarga.value < barraDeCarga.maxValue)
        {
            barraDeCarga.value += velocidadCarga * Time.deltaTime;
        }
    }

    public void SetEstado(EstadoNino nuevoEstado)
    {
        if (estadoActual == nuevoEstado) return;

        estadoActual = nuevoEstado;
        if (animator != null)
        {
            animator.SetInteger("estado", (int)estadoActual);
            Debug.Log($"Estado cambiado a: {estadoActual} ({(int)estadoActual})");
        }
    }
}

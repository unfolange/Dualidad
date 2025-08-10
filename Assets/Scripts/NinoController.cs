using UnityEngine;

public class NinoController : MonoBehaviour
{
    public Animator animator; // Debe tener parámetro int "estado"
    public EstadoNino estadoActual = EstadoNino.Playing;
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;
    float scoreAccum = 0f; // acumulador en float para sumar suave por Time.deltaTime
    public float pointsPerSecond = 10f; // puntos por segundo mientras se mantiene presionado

    void Update()
    {
        // ¿Está presionando (mouse o touch)?
        bool pressing = Input.GetMouseButton(0) || Input.touchCount > 0;

        // Si quieres ignorar clics sobre UI, usa esto en lugar de la línea de arriba:
        /*
        bool pressing =
            (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) ||
            (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
        */

        // Sumar puntos solo si el estado actual es Playing y se mantiene presionado
        if (pressing && estadoActual == EstadoNino.Playing)
        {
            scoreAccum += pointsPerSecond * Time.deltaTime;

            int newScore = Mathf.FloorToInt(scoreAccum);
            if (newScore != score)
            {
                score = newScore;
                if (scoreText) scoreText.text = "Score: " + score;
            }
        }
    }

    public void SetEstado(EstadoNino nuevoEstado)
    {
        if (estadoActual == nuevoEstado)
            return;

        estadoActual = nuevoEstado;
        if (animator != null)
        {
            animator.SetInteger("estado", (int)estadoActual);
            Debug.Log($"Estado cambiado a: {estadoActual} ({(int)estadoActual})");
        }
    }
}
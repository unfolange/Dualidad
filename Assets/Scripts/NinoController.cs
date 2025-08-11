using UnityEngine;
using TMPro; // Importante si usas TextMeshProUGUI
using System.Collections;

public class NinoController : MonoBehaviour
{
    public Animator animator; // Debe tener parámetro int "estado"
    public EstadoNino estadoActual = EstadoNino.Idle;

    public int score = 0;
    public float pointsPerSecond = 10f; // puntos por segundo mientras se mantiene presionado
    public TextMeshProUGUI scoreText;   // arrástralo en el Inspector

    float scoreAccum = 0f; // acumulador en float para sumar suave por Time.deltaTime
    public Transform puntoEstudio;
    public Transform puntoOficio;
    public Transform puntoIdle;
    Rigidbody2D rb;
    Coroutine moverCo;

    void Start()
    {
        // Mostrar el score inicial y avisar si no está asignado
        rb = GetComponent<Rigidbody2D>();
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

        if (pressing && estadoActual == EstadoNino.Playing)
        {
            scoreAccum += pointsPerSecond * Time.deltaTime;

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

    public void SetEstado(EstadoNino nuevoEstado)
    {
        // Si ya estoy en el mismo estado...
        if (estadoActual == nuevoEstado)
        {
            // ...pero es Playing, forzar reinicio del clip
            if (nuevoEstado == EstadoNino.Playing && animator != null)
            {
                animator.Play("Playing", 0, 0f); // nombre EXACTO del estado en tu Animator
            }
            return; // para otros estados iguales no hacemos nada
        }

        // Cambió el estado
        estadoActual = nuevoEstado;

        if (animator != null)
        {
            animator.SetInteger("estado", (int)estadoActual);

            // Al entrar a Playing por primera vez, reiniciar el clip también
            if (nuevoEstado == EstadoNino.Playing)
            {
                animator.Play("Playing", 0, 0f);
            }
        }
        if (nuevoEstado == EstadoNino.Studying || nuevoEstado == EstadoNino.Cleaning || nuevoEstado == EstadoNino.Idle)
        {
            // Mover al punto correspondiente
            Transform destino =
                nuevoEstado == EstadoNino.Studying ? puntoEstudio :
                nuevoEstado == EstadoNino.Cleaning ? puntoOficio :
                puntoIdle;
            IrAPunto(destino);
        }
        Debug.Log($"Estado cambiado a: {estadoActual} ({(int)estadoActual})");

    }


    public void IrAPunto(Transform destino)
    {
        if (moverCo != null) StopCoroutine(moverCo);
        moverCo = StartCoroutine(MoverA(destino.position));
    }

    private IEnumerator MoverA(Vector3 destino)
    {
        while (Vector2.Distance(rb.position, destino) > 0.02f)
        {
            Vector2 next = Vector2.MoveTowards(rb.position, destino, 100 * Time.fixedDeltaTime);
            rb.MovePosition(next);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(destino);
        // opcional: al llegar, poner Idle
        //SetEstado(EstadoNino.Idle);
        moverCo = null;
    }
}

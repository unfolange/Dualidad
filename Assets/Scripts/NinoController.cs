using UnityEngine;
using TMPro; // Importante si usas TextMeshProUGUI
using System.Collections;
using UnityEngine.UI;

public class NinoController : MonoBehaviour
{
    public Animator animator; // Debe tener parámetro int "estado"
    public EstadoNino estadoActual = EstadoNino.Idle;
    [SerializeField] private Slider barraDeCarga;
    [SerializeField] private float velocidadCarga = 0.25f;

    public int score = 0;
    public float pointsPerSecond = 10f; // puntos por segundo mientras se mantiene presionado
    public TextMeshProUGUI scoreText;   // arrástralo en el Inspector

    float scoreAccum = 0f; // acumulador en float para sumar suave por Time.deltaTime
    public Transform puntoEstudio;
    public Transform puntoOficio;
    public Transform puntoIdle;
    Rigidbody2D rb;
    Coroutine moverCo;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipEstudio;
    [SerializeField] private AudioClip clipOficio;
    [SerializeField] private AudioClip clipJuego;

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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool pressing = Input.GetMouseButton(0) || Input.touchCount > 0;

        // Si está en Playing pero se dejó de presionar, detener el audio
        if (!pressing && estadoActual == EstadoNino.Playing && audioSource.isPlaying)
        {
            if (audioSource.clip == clipJuego)
                audioSource.Stop();
        }

        if (pressing && estadoActual == EstadoNino.Playing)
        {
            float turnScore = pointsPerSecond * Time.deltaTime;

            scoreAccum += turnScore;

            LlenarBarra();

            int newScore = Mathf.FloorToInt(scoreAccum);
            Debug.Log($"Turn score: {turnScore} {scoreAccum} {newScore}");
            GameManager.Instance.SetPuntaje(newScore);

            if (newScore != score)
            {
                score = newScore;

                if (scoreText != null)
                    scoreText.text = "Puntaje: " + score;
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
        // Detener cualquier sonido que esté sonando
        if (audioSource.isPlaying) audioSource.Stop();

        // Reproducir sonido según el estado
        switch (nuevoEstado)
        {
            case EstadoNino.Studying:
                if (clipEstudio != null)
                    audioSource.PlayOneShot(clipEstudio);
                break;

            case EstadoNino.Cleaning:
                if (clipOficio != null)
                    audioSource.PlayOneShot(clipOficio);
                break;

            case EstadoNino.Playing:
                if (clipJuego != null)
                {
                    audioSource.clip = clipJuego;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                break;
        }

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

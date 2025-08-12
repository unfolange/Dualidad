using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{

    public enum ParentType { Mom, Dad }

    [Header("Tipo")]
    public ParentType tipo = ParentType.Mom;

    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int direction = 1; // 1 = derecha, -1 = izquierda
    [Tooltip("Voltear el sprite al cambiar de dirección (si tiene SpriteRenderer).")]
    [SerializeField] private bool flipSpriteOnTurn = true;
    public NinoController nino;
    public DangerZone dangerZone;

    [Header("Delay en giros")]
    [SerializeField] private float minDelay = 1f;
    [SerializeField] private float maxDelay = 5f;

    private Rigidbody2D rb;

    private SpriteRenderer sr;
    public Animator animator;
    private bool isWaiting = false;
    private bool isWatchingKid = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        direction = Mathf.Clamp(direction, -1, 1);
        if (direction == 0) direction = 1;
    }

    void FixedUpdate()
    {
        Debug.Log($"iswaiting {isWaiting}");
        if (!isWaiting && !dangerZone.GetComponent<DangerZone>().isDangerZoneOccupied)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
        else
        {
            // Quieto mientras espera
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (isWatchingKid)
            {
                bool ok = false;
                if (nino != null)
                {
                    // EstadoNino: Playing=0, Studying=1, Cleaning=2
                    if (tipo == ParentType.Mom)
                        ok = nino.estadoActual == EstadoNino.Cleaning;
                    else
                        ok = nino.estadoActual == EstadoNino.Studying;
                }

                if (!ok)
                {
                    if (GameManager.Instance != null)
                        GameManager.Instance.GameOver($"{tipo} te pilló (estado niño: {nino?.estadoActual})");
                    else
                        Debug.LogWarning("GameManager no asignado: debería disparar GameOver aquí.");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Cambiar dirección según nombre del trigger
        if (other.CompareTag("Untagged")) { /* no es obligatorio usar tags */ }

        if (other.name.Equals("LeftLimit"))
        {
            Debug.Log($"{name} -> Enter LEFT trigger");
            StartCoroutine(TurnWithDelay(1));
        }
        else if (other.name.Equals("RightLimit"))
        {
            Debug.Log($"{name} -> Enter RIGHT trigger");
            StartCoroutine(TurnWithDelay(-1));
        }
        else if (other.name.Equals("DangerZone"))
        {
            Debug.Log($"{name} -> Enter DANGER ZONE trigger");
            if (!dangerZone.GetComponent<DangerZone>().isDangerZoneOccupied)
            {
                dangerZone.GetComponent<DangerZone>().setDangerZoneOccupancy(true);
                WatchKid();
                StartCoroutine(StopWatchingKid());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("LeftLimit"))
        {
            Debug.Log($"{name} -> Exit LEFT trigger");
        }
        else if (other.name.Equals("RightLimit"))
        {
            Debug.Log($"{name} -> Exit RIGHT trigger");
        }
        else if (other.name.Equals("DangerZone"))
        {
            Debug.Log($"{name} -> Exit DANGER ZONE trigger");
        }
    }

    private IEnumerator TurnWithDelay(int newDirection)
    {
        isWaiting = true;

        float waitTime = Random.Range(minDelay, maxDelay);
        Debug.Log($"{name} esperando {waitTime:F2} seg antes de moverse");

        yield return new WaitForSeconds(waitTime);

        direction = newDirection;
        if (flipSpriteOnTurn && sr)
            sr.flipX = direction < 0;

        isWaiting = false;
    }

    private void WatchKid()
    {
        SetAnimWalk(true);
        isWaiting = true;
        isWatchingKid = true;
    }

    private IEnumerator StopWatchingKid()
    {
        float waitTime = 3f;
        Debug.Log($"{name} esperando {waitTime:F2} seg antes de moverse");

        yield return new WaitForSeconds(waitTime);

        dangerZone.GetComponent<DangerZone>().setDangerZoneOccupancy(false);
        isWaiting = false;
        SetAnimWalk(true);
        isWatchingKid = false;
    }

    // Opcional: para depurar o cambiar desde otros scripts/UI
    public void SetSpeed(float newSpeed) => speed = Mathf.Max(0f, newSpeed);
    public void SetDirection(int dir)
    {
        direction = Mathf.Clamp(dir, -1, 1);
        if (direction == 0) direction = 1;
        if (flipSpriteOnTurn && sr) sr.flipX = direction < 0;
    }

    void SetAnimWalk(bool walking)
    {
        if (!animator) return;
        // estado: 0 = idle, 1 = walk
        animator.SetInteger("estado", walking ? 1 : 0);
        string animacion = walking ? "caminando" : "esperando";
        Debug.Log($"Cambiando animacion a {animacion}");
    }
}

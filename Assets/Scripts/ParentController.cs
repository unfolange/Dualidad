using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ParentController : MonoBehaviour
{
    public enum ParentType { Mom, Dad }

    [Header("Tipo")]
    public ParentType tipo = ParentType.Mom;

    [Header("Refs")]
    public NinoController nino;        // arrastra el Nino
    public GameManager gameManager;    // arrastra el GameManager
    public Animator animator;          // Animator del papá/mamá (con parámetro int "estado")

    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int direction = 1; // 1=der, -1=izq
    [SerializeField] private bool flipSpriteOnTurn = true;

    [Header("Delays")]
    [SerializeField] private float minTurnDelay = 0f;   // al tocar left/right
    [SerializeField] private float maxTurnDelay = 0.4f;
    [SerializeField] private float minDoorIdle = 0.2f;  // pausa al llegar a puerta
    [SerializeField] private float maxDoorIdle = 0.8f;

    [Header("Nombres de triggers")]
    public string leftName = "left";
    public string rightName = "right";
    public string doorName = "DangerZone";   // o "Danger Zone" si así se llama

    Rigidbody2D rb;
    SpriteRenderer sr;
    bool isWaiting = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        direction = Mathf.Clamp(direction, -1, 1);
        if (direction == 0) direction = 1;
        ApplyFlip();
        SetAnimWalk(true); // empezar caminando
        if (!animator) animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        if (!isWaiting)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals(leftName))
        {
            StartCoroutine(TurnAfterDelay(1));   // ir a la derecha
            Debug.Log($"{name} -> Enter LEFT");
        }
        else if (other.name.Equals(rightName))
        {
            StartCoroutine(TurnAfterDelay(-1));  // ir a la izquierda
            Debug.Log($"{name} -> Enter RIGHT");
        }
        else if (other.name.Equals(doorName))
        {
            Debug.Log($"{name} -> Enter DOOR");
            StartCoroutine(DoorCheckRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals(leftName)) Debug.Log($"{name} -> Exit LEFT");
        if (other.name.Equals(rightName)) Debug.Log($"{name} -> Exit RIGHT");
        if (other.name.Equals(doorName)) Debug.Log($"{name} -> Exit DOOR");
    }

    IEnumerator TurnAfterDelay(int newDir)
    {
        isWaiting = true;
        float wait = Random.Range(minTurnDelay, maxTurnDelay);
        yield return new WaitForSeconds(wait);
        direction = newDir;
        ApplyFlip();
        isWaiting = false;
        SetAnimWalk(true);
    }

    IEnumerator DoorCheckRoutine()
    {
        // parar y mirar al niño
        isWaiting = true;
        SetAnimWalk(false);  // idle
        FaceTowardChild();

        // pausa aleatoria frente a la puerta
        float idle = Random.Range(minDoorIdle, maxDoorIdle);
        yield return new WaitForSeconds(idle);

        // verificar estado del niño
        bool ok = false;
        if (nino != null)
        {
            // EstadoNino: Playing=0, Studying=1, Cleaning=2
            if (tipo == ParentType.Mom)
                ok = (nino.estadoActual == EstadoNino.Cleaning);
            else
                ok = (nino.estadoActual == EstadoNino.Studying);
        }

        if (!ok)
        {
            if (gameManager != null)
                gameManager.GameOver($"{tipo} te pilló (estado niño: {nino?.estadoActual})");
            else
                Debug.LogWarning("GameManager no asignado: debería disparar GameOver aquí.");
        }
        else
        {
            // se va: media vuelta y caminar
            direction *= -1;
            ApplyFlip();
            isWaiting = false;
            SetAnimWalk(true);
        }
    }

    void ApplyFlip()
    {
        if (flipSpriteOnTurn && sr)
            sr.flipX = (direction < 0);
    }

    void FaceTowardChild()
    {
        if (!nino || !sr) return;
        bool childIsRight = (nino.transform.position.x > transform.position.x);
        sr.flipX = !childIsRight; // si el niño está a la derecha, mirar a la derecha (flipX=false)
    }

    void SetAnimWalk(bool walking)
    {
        if (!animator) return;
        // estado: 0 = idle, 1 = walk
        // animator.SetInteger("estado", walking ? 1 : 0);
    }
}

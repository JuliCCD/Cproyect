using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa TextMeshPro

public class PlayerController : MonoBehaviour
{
    public float velocidad = 10f;
    public float fuerzaSalto = 12.5f;
    
    public GameObject kunaiPrefab;
    public int kunaisDisponibles = 5;

    public Transform groundCheck;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    private bool isGrounded = true;
    private string direccion = "Derecha";
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;

    [Header("Parámetros de salto")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField] private TextMeshProUGUI enemigosMuertosText;
    [SerializeField] private TextMeshProUGUI vidasRestantesText;

    private int enemigosEliminados = 0;
    private int vidasRestantes = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Iniciando PlayerController");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Busca los textos en la jerarquía
        enemigosMuertosText = GameObject.Find("EnemigosMuertosText").GetComponent<TextMeshProUGUI>();
        vidasRestantesText = GameObject.Find("VidasRestantesText").GetComponent<TextMeshProUGUI>();

        defaultGravityScale = rb.gravityScale;

        ActualizarTextos();
    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupMoverseVertical();
        SetupSalto();
        SetUpLanzarKunai();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            Debug.Log($"Colision con Enemigo: ${zombie.puntosVida}");
            Destroy(collision.gameObject);

            enemigosEliminados++;
            ActualizarTextos();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro") {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }

    void SetupMoverseHorizontal() {
        
        Debug.Log($"isGrounded: {isGrounded}, {rb.linearVelocityY}");
        if (isGrounded && rb.linearVelocityY == 0) {
            animator.SetInteger("Estado", 0);
        }

        rb.linearVelocityX = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = velocidad;
            sr.flipX = false;
            direccion = "Derecha";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -velocidad;
            sr.flipX = true;
            direccion = "Izquierda";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
    }

    void SetupMoverseVertical() {
        
        if (!puedeMoverseVerticalMente) return;
        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = velocidad;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -velocidad;
        }
    }

    void SetupSalto()
    {
        // Verifica si el personaje está en el suelo
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // --- Coyote Time ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (rb.linearVelocity.y > 5f)
                animator.SetInteger("Estado", 3);
        }

        // --- Jump Buffer ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // --- Ejecutar salto ---
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Cambiado a rb.velocity
            jumpBufferCounter = 0f;
        }

        // --- Ajuste de gravedad para caída más rápida ---
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    void SetUpLanzarKunai() {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;
        if (Input.GetKeyUp(KeyCode.K))
        {
            GameObject kunai = Instantiate(kunaiPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            kunai.GetComponent<KunaiController>().SetDirection(direccion);
            kunaisDisponibles -= 1;
        }
    }

    public void RecibirDaño()
    {
        vidasRestantes--;
        ActualizarTextos();

        if (vidasRestantes <= 0)
        {
            Debug.Log("Game Over");
            // Aquí puedes implementar lógica para reiniciar el nivel o mostrar pantalla de Game Over
        }
    }

    private void ActualizarTextos()
    {
        enemigosMuertosText.text = $"Enemigos Eliminados: {enemigosEliminados}";
        vidasRestantesText.text = $"Vidas Restantes: {vidasRestantes}";
    }

    // Visualiza el groundCheck en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }
}

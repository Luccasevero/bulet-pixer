using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float speed = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Lê o input do teclado (WASD ou setas)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;

        // Guarda a última direção não-zero (útil depois para mirar o tiro)
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput;
        }

        // Alimenta o Animator com a velocidade atual
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }

        // Vira o sprite pro lado que o personagem anda
        if (spriteRenderer != null && Mathf.Abs(moveInput.x) > 0.01f)
        {
            spriteRenderer.flipX = moveInput.x < 0f;
        }
    }

    void FixedUpdate()
    {
        // Move o Rigidbody2D (FixedUpdate = física, mais suave e consistente)
        rb.linearVelocity = moveInput * speed;
    }

    // Direção que o player está olhando/andando, útil para o script de tiro mirar
    public Vector2 GetFacingDirection()
    {
        return lastMoveDirection;
    }
}

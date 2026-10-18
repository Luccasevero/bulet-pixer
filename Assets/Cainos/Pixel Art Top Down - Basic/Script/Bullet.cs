using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    [Header("Atravessar inimigos")]
    [SerializeField] private int maxPierce = 1; // 1 = some no primeiro inimigo. O PlayerShooting sobrescreve esse valor ao atirar.

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;
    private int hits = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // some sozinha depois de um tempo, mesmo sem acertar nada
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Vira a bala pra apontar na direção do tiro
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Chamado pelo PlayerShooting: quantos inimigos este tiro atravessa antes de sumir
    public void SetPierce(int value)
    {
        maxPierce = Mathf.Max(1, value);
    }

    void Start()
    {
        rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        // Por enquanto só destrói o inimigo direto.
        // Quando criarmos vida/dano, isso vira: other.GetComponent<EnemyHealth>().TakeDamage(...)
        Destroy(other.gameObject);

        hits++;
        if (hits >= maxPierce)
        {
            Destroy(gameObject);
        }
    }
}
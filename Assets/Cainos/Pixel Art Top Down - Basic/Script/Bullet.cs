using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // some sozinha depois de um tempo, mesmo sem acertar nada
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Vira a bala pra apontar na direção do tiro (bom se o sprite for uma flecha/seta)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Start()
    {
        rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Por enquanto só destrói o inimigo direto.
            // Quando criarmos vida/dano, isso vira: other.GetComponent<EnemyHealth>().TakeDamage(...)
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

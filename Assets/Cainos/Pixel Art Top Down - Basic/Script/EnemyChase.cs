using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    // Marque se o desenho original do inimigo olha para a ESQUERDA.
    // Se ele estiver virando ao contrário do que deveria, é só alternar esta caixinha no Inspector.
    [SerializeField] private bool spriteFacesLeft = false;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private float speedMultiplier = 1f;

    public void SetSpeedMultiplier(float multiplier) {
        speedMultiplier = multiplier;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed * speedMultiplier;

        // Vira o sprite para o lado em que está andando
        if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.01f)
        {
            bool movingLeft = direction.x < 0f;
            spriteRenderer.flipX = spriteFacesLeft ? !movingLeft : movingLeft;
        }
    }
}
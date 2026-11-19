using UnityEngine;

// Coloque este script em cada inimigo que deve machucar o jogador ao encostar.
public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    // Chamado enquanto o inimigo estiver encostando em outro collider
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth health = collision.gameObject.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            // O PlayerHealth já ignora dano repetido durante o tempo de invulnerabilidade
            health.TakeDamage(damage);
        }
    }
}
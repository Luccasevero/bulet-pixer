using UnityEngine;

// Coloque este script em cada inimigo que deve machucar o jogador ao encostar.
public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool dieOnHit = true; // o inimigo some depois de encostar no herói

    [Header("Áudio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField, Range(0f, 1f)] private float hitVolume = 1f;

    private bool alreadyHit;

    // Chamado enquanto o inimigo estiver encostando em outro collider
    void OnCollisionStay2D(Collision2D collision)
    {
        if (alreadyHit) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth health = collision.gameObject.GetComponentInParent<PlayerHealth>();
        if (health == null) return;

        // O PlayerHealth já ignora dano repetido durante o tempo de invulnerabilidade
        health.TakeDamage(damage);

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, hitVolume);
        }

        if (dieOnHit)
        {
            alreadyHit = true;
            Destroy(gameObject);
        }
    }
}
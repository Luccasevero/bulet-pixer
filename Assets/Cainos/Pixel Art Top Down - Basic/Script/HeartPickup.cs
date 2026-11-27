using UnityEngine;

// Coração que o inimigo solta ao morrer (com uma chance). Recupera vida do hero.
public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float lifetime = 15f; // some sozinho se ninguém pegar

    [Header("Áudio")]
    [SerializeField] private AudioClip healSound;
    [SerializeField, Range(0f, 1f)] private float healVolume = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Stay (e não Enter): se o hero estiver em cima do coração com a vida cheia
    // e perder vida depois, ele consegue pegar sem precisar sair e voltar.
    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;

        // Só consome o coração se realmente curou (vida cheia não gasta o coração)
        if (health.Heal(healAmount))
        {
            if (UpgradeMessage.Instance != null)
            {
                UpgradeMessage.Instance.Show("+" + healAmount + " de vida");
            }

                        if (healSound != null)
            {
                AudioSource.PlayClipAtPoint(healSound, transform.position, healVolume);
            }

            Destroy(gameObject);
        }
    }
}
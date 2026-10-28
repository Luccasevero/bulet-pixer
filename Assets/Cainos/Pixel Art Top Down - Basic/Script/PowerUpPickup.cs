using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private int pierceBonus = 1;     // quantos inimigos a mais o tiro passa a atravessar
    [SerializeField] private float lifetime = 20f;    // some sozinho se ninguém pegar

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();
        if (shooting == null) return;

        shooting.AddPierce(pierceBonus);
        Destroy(gameObject);
    }
}

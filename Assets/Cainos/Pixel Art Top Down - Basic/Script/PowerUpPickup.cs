using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private enum PowerUpType
    {
        Pierce,     // o corte atravessa mais inimigos
        FireRate,   // o hero ataca com mais frequência
        Damage,     // cada corte causa mais dano
        MoveSpeed   // o hero anda mais rápido
    }

    [Header("Quanto cada efeito aumenta (o tipo é sorteado ao pegar o item)")]
    [SerializeField] private int pierceBonus = 1;
    [SerializeField] private float fireRateBonus = 0.25f;  // tiros por segundo a mais
    [SerializeField] private int damageBonus = 1;
    [SerializeField] private float moveSpeedBonus = 0.5f;

    [SerializeField] private float lifetime = 20f;          // some sozinho se ninguém pegar

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
        if (shooting == null) return;

        // Sorteia um dos efeitos, com a mesma chance para cada um
        int typeCount = System.Enum.GetValues(typeof(PowerUpType)).Length;
        PowerUpType type = (PowerUpType)Random.Range(0, typeCount);

        string message = "";

        switch (type)
        {
            case PowerUpType.Pierce:
                shooting.AddPierce(pierceBonus);
                message = "Corte atravessa +" + pierceBonus + " inimigo!";
                break;
            case PowerUpType.FireRate:
                shooting.AddFireRate(fireRateBonus);
                message = "Ataque mais rápido!";
                break;
            case PowerUpType.Damage:
                shooting.AddDamage(damageBonus);
                message = "Mais dano! (+" + damageBonus + ")";
                break;
            case PowerUpType.MoveSpeed:
                if (movement != null) movement.AddSpeed(moveSpeedBonus);
                message = "Você está mais rápido!";
                break;
        }

        // Mostra na tela (se existir a mensagem na cena) e também no Console
        if (UpgradeMessage.Instance != null)
        {
            UpgradeMessage.Instance.Show(message);
        }
        Debug.Log("Aprimoramento pego: " + type);

        Destroy(gameObject);
    }
}
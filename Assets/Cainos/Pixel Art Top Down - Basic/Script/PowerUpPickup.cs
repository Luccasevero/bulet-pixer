using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private enum PowerUpType
    {
        Pierce,        // o corte atravessa mais inimigos
        FireRate,      // o hero ataca com mais frequência
        Damage,        // cada corte causa mais dano
        MoveSpeed,     // o hero anda mais rápido
        PickupRange,   // as gemas são atraídas de mais longe
        SpecialCharge  // o hero passa a poder guardar mais uma carga da habilidade
    }

    [Header("Quanto cada efeito aumenta (o tipo é sorteado ao pegar o item)")]
    [SerializeField] private int pierceBonus = 1;
    [SerializeField] private float fireRateBonus = 0.25f;   // tiros por segundo a mais
    [SerializeField] private int damageBonus = 1;
    [SerializeField] private float moveSpeedBonus = 0.5f;
    [SerializeField] private float pickupRangeBonus = 0.75f;

    [SerializeField] private float lifetime = 20f;           // some sozinho se ninguém pegar

    [Header("Áudio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)] private float pickupVolume = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();
        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();
        PlayerExperience experience = other.GetComponentInParent<PlayerExperience>();
        PlayerSpecial special = other.GetComponentInParent<PlayerSpecial>();
        if (shooting == null) return;

        PowerUpType type = RollType(special);
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
            case PowerUpType.PickupRange:
                if (experience != null) experience.AddPickupRange(pickupRangeBonus);
                message = "Coleta de gemas maior!";
                break;
            case PowerUpType.SpecialCharge:
                // Aumenta a capacidade e já entrega uma carga de brinde
                if (special != null && special.AddMaxCharge())
                {
                    special.GrantCharge();
                }
                message = "Você guarda mais uma habilidade!";
                break;
        }

        // Mostra na tela (se existir a mensagem na cena) e também no Console
        if (UpgradeMessage.Instance != null)
        {
            UpgradeMessage.Instance.Show(message);
        }
        Debug.Log("Aprimoramento pego: " + type);

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
        }

        Destroy(gameObject);
    }

    // Sorteia um dos efeitos com a mesma chance. Se o sorteio cair em "mais uma carga"
    // mas a capacidade já está no teto, sorteia de novo (a moeda não é desperdiçada).
    private PowerUpType RollType(PlayerSpecial special)
    {
        int typeCount = System.Enum.GetValues(typeof(PowerUpType)).Length;
        PowerUpType type;

        do
        {
            type = (PowerUpType)Random.Range(0, typeCount);
        }
        while (type == PowerUpType.SpecialCharge && (special == null || !special.CanAddMaxCharge));

        return type;
    }
}
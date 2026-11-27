using UnityEngine;

// Gema de experiência que o inimigo solta ao morrer.
public class XPGem : MonoBehaviour
{
    [SerializeField] private int xpValue = 1;
    [SerializeField] private float magnetRange = 3f;  // usado só se o hero não tiver o PlayerExperience
    [SerializeField] private float magnetSpeed = 8f;  // velocidade com que ela voa até o hero

    [Header("Áudio")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField, Range(0f, 1f)] private float collectVolume = 0.5f;

    private Transform player;
    private PlayerExperience playerExperience;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerExperience = playerObj.GetComponentInParent<PlayerExperience>();
        }
    }

    // Chamado pelo EnemyHealth pra definir quanto esta gema vale
    public void SetValue(int value)
    {
        xpValue = value;
    }

    void Update()
    {
        if (player == null) return;

        // O alcance vem do hero (e cresce com os aprimoramentos)
        float range = playerExperience != null ? playerExperience.PickupRange : magnetRange;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= range)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, magnetSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerExperience experience = other.GetComponentInParent<PlayerExperience>();
        if (experience == null) return;

        experience.AddXp(xpValue);

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, collectVolume);
        }

        Destroy(gameObject);
    }
}
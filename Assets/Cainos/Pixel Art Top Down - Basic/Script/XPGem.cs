using UnityEngine;

// Gema de experiência que o inimigo solta ao morrer.
public class XPGem : MonoBehaviour
{
    [SerializeField] private int xpValue = 1;
    [SerializeField] private float magnetRange = 3f;  // distância em que o hero começa a atrair a gema
    [SerializeField] private float magnetSpeed = 8f;  // velocidade com que ela voa até o hero

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
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

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= magnetRange)
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
        Destroy(gameObject);
    }
}
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float flashTime = 0.08f; // tempo que o inimigo fica vermelho ao levar dano

    [Header("Experiência")]
    [SerializeField] private GameObject xpGemPrefab;  // gema que ele solta ao morrer
    [SerializeField] private int xpValue = 1;         // quanta experiência a gema vale

    [Header("Coração (recupera vida)")]
    [SerializeField] private GameObject heartPrefab;                  // coração que pode cair ao morrer
    [SerializeField, Range(0f, 1f)] private float heartDropChance = 0.05f; // 0,05 = 5% de chance

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(Flash());
        }
    }

    private void Die()
    {
        // Conta a morte na pontuação
        if (GameStats.Instance != null)
        {
            GameStats.Instance.AddKill();
        }

        // Solta a gema de experiência no lugar onde o inimigo morreu
        if (xpGemPrefab != null)
        {
            GameObject gem = Instantiate(xpGemPrefab, transform.position, Quaternion.identity);
            XPGem xp = gem.GetComponent<XPGem>();
            if (xp != null)
            {
                xp.SetValue(xpValue);
            }
        }

        // Com uma chance, solta também um coração (um pouco deslocado pra não ficar em cima da gema)
        if (heartPrefab != null && Random.value < heartDropChance)
        {
            Vector3 offset = (Vector3)(Random.insideUnitCircle * 0.4f);
            Instantiate(heartPrefab, transform.position + offset, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // Pisca em vermelho pra mostrar que o tiro acertou (mas o inimigo ainda está vivo)
    private System.Collections.IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = originalColor;
    }
}
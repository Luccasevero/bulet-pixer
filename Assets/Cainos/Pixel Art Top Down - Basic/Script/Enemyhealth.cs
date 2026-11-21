using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float flashTime = 0.08f; // tempo que o inimigo fica vermelho ao levar dano

    [Header("Experiência")]
    [SerializeField] private GameObject xpGemPrefab;  // gema que ele solta ao morrer
    [SerializeField] private int xpValue = 1;         // quanta experiência a gema vale

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
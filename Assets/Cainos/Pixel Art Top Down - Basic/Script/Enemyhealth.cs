using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float flashTime = 0.08f; // tempo que o inimigo fica vermelho ao levar dano

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
            // Conta a morte na pontuação antes de destruir o inimigo
            if (GameStats.Instance != null)
            {
                GameStats.Instance.AddKill();
            }

            Destroy(gameObject);
            return;
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(Flash());
        }
    }

    // Pisca em vermelho pra mostrar que o tiro acertou (mas o inimigo ainda está vivo)
    private System.Collections.IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = originalColor;
    }
}
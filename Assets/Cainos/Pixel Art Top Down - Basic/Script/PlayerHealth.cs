using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float invulnerableTime = 1f; // segundos sem levar dano depois de ser atingido
    [SerializeField] private Slider healthBar;             // barra de vida na tela (opcional)

    private int currentHealth;
    private bool invulnerable;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateBar();
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable || currentHealth <= 0) return;

        currentHealth -= amount;
        UpdateBar();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    // Recupera vida (sem passar do máximo). Retorna true se realmente curou alguma coisa,
    // e false se a vida já estava cheia (ou o hero já morreu).
    public bool Heal(int amount)
    {
        if (currentHealth <= 0 || currentHealth >= maxHealth) return false;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateBar();
        return true;
    }

    // Fica vermelho e translúcido durante o tempo em que não pode levar dano
    private IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;

        Color originalColor = Color.white;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1f, 0.4f, 0.4f, 0.6f);
        }

        yield return new WaitForSeconds(invulnerableTime);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        invulnerable = false;
    }

    private void Die()
    {
        if (GameOverScreen.Instance != null)
        {
            GameOverScreen.Instance.Show();
        }
        else
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }
    }

    private void UpdateBar()
    {
        if (healthBar == null) return;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
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

// 26/11
        [Header("Áudio")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField, Range(0f, 1f)] private float deathVolume = 1f;
    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f; //

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

                // Toca o som de morte (fora do inimigo, para não cortar quando ele for destruído)
        if (deathSound != null)
        {
            PlaySoundAtPoint(deathSound, transform.position, deathVolume);
        }

        Destroy(gameObject);
    }

        // Toca um som 2D sem depender do inimigo, com pequena variação de pitch
    private void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume)
    {
        GameObject temp = new GameObject("DeathSound");
        temp.transform.position = position;

        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.spatialBlend = 0f; // 0 = som 2D, sempre audível independente da distância
        source.Play();

        // Destroi o objeto temporário quando o som acabar (ajustado pelo pitch)
        Destroy(temp, clip.length / source.pitch);
    }

    // Pisca em vermelho pra mostrar que o tiro acertou (mas o inimigo ainda está vivo)
    private System.Collections.IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = originalColor;
    }
}
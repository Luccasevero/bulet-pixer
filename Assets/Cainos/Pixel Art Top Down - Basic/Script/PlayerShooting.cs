using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;         // tiros por segundo
    [SerializeField] private float shotDelay = 0.2f;      // segundos entre o início do ataque e a saída da bala
    [SerializeField] private float detectionRadius = 4f;  // alcance de detecção de inimigos
    [SerializeField] private LayerMask enemyLayer;         // layer usada pelos inimigos

    [Header("Aprimoramentos")]
    [SerializeField] private int pierce = 1;               // 1 = o tiro some no primeiro inimigo
    [SerializeField] private int damage = 1;
    [SerializeField] private float maxFireRate = 4f;       // teto pra não ficar rápido demais

    [Header("Áudio")]
    [SerializeField] private AudioClip shootSound;         // Som do tiro
    [SerializeField] private AudioClip attackSound;

    private float fireCooldown;
    private Animator animator;
    private AudioSource audioSource;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Pega o componente AudioSource que está no herói
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não encontrado no herói! Certifique-se de adicionar um componente AudioSource.");
        }
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        Transform target = FindNearestEnemy();
        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / fireRate;
        }
    }
    public void RealeasePower()
    {
        PlaySound(attackSound);
    }

    // Métodos chamados pelos itens e pelo nível pra melhorar o hero
    public void AddDamage(int amount)
    {
        damage += amount;
    }

    public void AddFireRate(float amount)
    {
        fireRate = Mathf.Min(fireRate + amount, maxFireRate);
    }

    public void AddPierce(int amount)
    {
        pierce += amount;
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        Transform nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    private void Shoot(Transform target)
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;

        // Animação de ataque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Vira o hero pro lado do inimigo
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = direction.x < 0f;
        }

        // A bala e o som saem depois do atraso, no momento do golpe
        StartCoroutine(SpawnBulletAfterDelay(direction));
    }

    private System.Collections.IEnumerator SpawnBulletAfterDelay(Vector2 direction)
    {
        yield return new WaitForSeconds(shotDelay);

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction);
            bullet.SetPierce(pierce);
            bullet.SetDamage(damage);
        }

        // Som do ataque
    
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            audioSource.Play();
        }
        
    }
    private void PlaySound(AudioClip clip)
    {
        if(audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }   
    }

    // Desenha o raio de detecção na Scene view (só aparece no Editor, ajuda a visualizar o alcance)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
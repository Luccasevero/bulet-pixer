using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;         // tiros por segundo
    [SerializeField] private float shotDelay = 0.2f; // segundos entre o início do ataque e a saída da bala
    [SerializeField] private float detectionRadius = 4f;  // alcance de detecção de inimigos
    [SerializeField] private LayerMask enemyLayer;         // layer usada pelos inimigos

    private float fireCooldown;

   private Animator animator;
    void Awake() {
        animator = GetComponent<Animator>();
    }

    [Header("Aprimoramentos")]
    [SerializeField] private int pierce = 1; // 1 = o tiro some no primeiro inimigo

    public void AddPierce(int amount) {
        pierce += amount;
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

    private void Shoot(Transform target) {
    if (bulletPrefab == null || firePoint == null) return;

    Vector2 direction = (target.position - firePoint.position).normalized;

    if (animator != null) animator.SetTrigger("Attack");

    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr != null) sr.flipX = direction.x < 0f;

    StartCoroutine(SpawnBulletAfterDelay(direction));
    }

    private System.Collections.IEnumerator SpawnBulletAfterDelay(Vector2 direction){
    yield return new WaitForSeconds(shotDelay);

    GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    Bullet bullet = bulletObj.GetComponent<Bullet>();
    if (bullet != null)
    {
        bullet.SetDirection(direction);
        bullet.SetPierce(pierce);
    }
    }

    // Desenha o raio de detecção na Scene view (só aparece no Editor, ajuda a visualizar o alcance)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

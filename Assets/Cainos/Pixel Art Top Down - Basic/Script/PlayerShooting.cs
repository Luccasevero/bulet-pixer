using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;         // tiros por segundo
    [SerializeField] private float detectionRadius = 4f;  // alcance de detecção de inimigos
    [SerializeField] private LayerMask enemyLayer;         // layer usada pelos inimigos

    private float fireCooldown;

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

    private void Shoot(Transform target)
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction);
        }
    }

    // Desenha o raio de detecção na Scene view (só aparece no Editor, ajuda a visualizar o alcance)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

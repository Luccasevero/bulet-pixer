using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Inimigo")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Área de spawn (retângulo: os inimigos nascem nas bordas)")]
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 12f);

    [Header("Ritmo da horda")]
    [SerializeField] private float startInterval = 2f;               // segundos entre spawns no começo
    [SerializeField] private float minInterval = 0.4f;               // o mais rápido que pode ficar
    [SerializeField] private float intervalDecreasePerSecond = 0.02f; // quanto o intervalo diminui por segundo
    [SerializeField] private int maxEnemies = 50;                    // limite de inimigos vivos ao mesmo tempo

    [Header("Velocidade crescente")]
    [SerializeField] private float speedIncreasePerSecond = 0.01f;
    [SerializeField] private float maxSpeedMultiplier = 1.75f;

private float currentSpeedMultiplier = 1f;

    private float timer;
    private float currentInterval;

    void Start()
    {
        currentInterval = startInterval;
        timer = currentInterval;
    }

    void Update()
    {
        // A cada segundo que passa, os inimigos nascem um pouco mais rápido
        currentInterval = Mathf.Max(minInterval, currentInterval - intervalDecreasePerSecond * Time.deltaTime);

        currentSpeedMultiplier = Mathf.Min(maxSpeedMultiplier,
        currentSpeedMultiplier + speedIncreasePerSecond * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = currentInterval;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (enemyPrefab == null) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies) return;

        GameObject enemy = Instantiate(enemyPrefab, GetRandomEdgePosition(), Quaternion.identity);
        EnemyChase chase = enemy.GetComponent<EnemyChase>();
        if (chase != null) chase.SetSpeedMultiplier(currentSpeedMultiplier);
    }

    // Sorteia um ponto em uma das 4 bordas do retângulo
    private Vector2 GetRandomEdgePosition()
    {
        Vector2 half = areaSize / 2f;
        float x;
        float y;

        switch (Random.Range(0, 4))
        {
            case 0: // topo
                x = Random.Range(-half.x, half.x);
                y = half.y;
                break;
            case 1: // base
                x = Random.Range(-half.x, half.x);
                y = -half.y;
                break;
            case 2: // esquerda
                x = -half.x;
                y = Random.Range(-half.y, half.y);
                break;
            default: // direita
                x = half.x;
                y = Random.Range(-half.y, half.y);
                break;
        }

        return areaCenter + new Vector2(x, y);
    }

    // Desenha o retângulo de spawn na Scene view quando o objeto está selecionado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}

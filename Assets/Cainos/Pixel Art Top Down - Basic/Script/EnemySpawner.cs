using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnStage
    {
        public string name = "Etapa";
        public float startTime = 0f;       // segundo de jogo em que esta etapa começa
        public GameObject[] enemies;        // inimigos sorteados nesta etapa (um ou mais prefabs)
        public float startInterval = 2f;    // segundos entre spawns quando a etapa começa
        public float minInterval = 0.5f;    // o mais rápido que o spawn pode ficar nesta etapa
        public int enemiesPerSpawn = 2; // quantidade de inimigos por nascimento
    }

    [Header("Etapas (a que tiver o maior Start Time já alcançado é a ativa)")]
    [SerializeField] private SpawnStage[] stages;

    [Header("Área de spawn (retângulo: os inimigos nascem nas bordas)")]
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 12f);

    [Header("Ritmo")]
    [SerializeField] private float intervalDecreasePerSecond = 0.02f; // quanto o intervalo diminui por segundo dentro da etapa
    [SerializeField] private int maxEnemies = 50;                    // limite de inimigos vivos ao mesmo tempo

    private float elapsed;
    private float timer;

    void Start()
    {
        timer = (stages != null && stages.Length > 0) ? stages[0].startInterval : 1f;
    }

    void Update()
    {
        if (stages == null || stages.Length == 0) return;

        elapsed += Time.deltaTime;

        SpawnStage stage = GetCurrentStage();
        if (stage == null) return;

        float timeInStage = elapsed - stage.startTime;
        float interval = Mathf.Max(stage.minInterval, stage.startInterval - intervalDecreasePerSecond * timeInStage);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = interval;
            TrySpawn(stage);
        }
    }

    private SpawnStage GetCurrentStage()
    {
        SpawnStage current = null;
        foreach (SpawnStage s in stages)
        {
            if (elapsed >= s.startTime && (current == null || s.startTime >= current.startTime))
            {
                current = s;
            }
        }
        return current;
    }

    private void TrySpawn(SpawnStage stage)
{
    if (stage.enemies == null || stage.enemies.Length == 0) return;

    for (int i = 0; i < stage.enemiesPerSpawn; i++)
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies)
            return;

        GameObject prefab = stage.enemies[Random.Range(0, stage.enemies.Length)];

        if (prefab == null) continue;

        Instantiate(prefab, GetRandomEdgePosition(), Quaternion.identity);
    }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private GameObject pickupPrefab;

    [Header("Área onde os itens podem aparecer (retângulo preenchido)")]
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private Vector2 areaSize = new Vector2(16f, 9f);

    [Header("Ritmo")]
    [SerializeField] private float firstDelay = 20f;     // segundos até o primeiro item aparecer
    [SerializeField] private float minInterval = 25f;    // intervalo mínimo entre itens
    [SerializeField] private float maxInterval = 40f;    // intervalo máximo entre itens
    [SerializeField] private int maxPickupsOnMap = 1;    // quantos itens podem existir ao mesmo tempo

    private float timer;

    void Start()
    {
        timer = firstDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = Random.Range(minInterval, maxInterval);
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (pickupPrefab == null) return;
        if (FindObjectsByType<PowerUpPickup>(FindObjectsSortMode.None).Length >= maxPickupsOnMap) return;

        Vector2 half = areaSize / 2f;

        // Tenta algumas vezes achar um lugar livre (sem objeto do cenário em cima)
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = areaCenter + new Vector2(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y));

            if (Physics2D.OverlapCircle(pos, 0.5f) == null)
            {
                Instantiate(pickupPrefab, pos, Quaternion.identity);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}

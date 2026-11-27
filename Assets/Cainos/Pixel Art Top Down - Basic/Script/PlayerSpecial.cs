using TMPro;
using UnityEngine;

// Habilidade especial do hero: ao apertar ESPAÇO, ele atira em TODOS os inimigos do mapa.
// O hero pode guardar mais de uma carga, dependendo da capacidade (que cresce com moedas e níveis).
public class PlayerSpecial : MonoBehaviour
{
    [Header("Habilidade especial")]
    [SerializeField] private KeyCode key = KeyCode.E;
    [SerializeField] private GameObject bulletPrefab;      // o mesmo prefab do corte
    [SerializeField] private Transform firePoint;          // de onde os tiros saem
    [SerializeField] private int specialDamage = 2;        // dano de cada tiro da habilidade
    [SerializeField] private int specialPierce = 99;       // atravessa inimigos pra garantir que cada alvo seja atingido

    [Header("Cargas")]
    [SerializeField] private int maxCharges = 1;           // quantas cargas o hero pode guardar agora
    [SerializeField] private int maxChargesCap = 3;        // o máximo que a capacidade pode chegar

    [Header("Áudio")]
    [SerializeField] private AudioClip specialSound;       // som ao usar a habilidade
    [SerializeField, Range(0f, 1f)] private float specialVolume = 1f;

    [Header("Interface (opcional)")]
    [SerializeField] private TMP_Text specialText;
    [SerializeField] private SpecialChargesUI chargesUI;

    private int charges;
    private AudioSource audioSource;

    // Dá pra aumentar a capacidade ainda? (usado pelo sorteio da moeda)
    public bool CanAddMaxCharge
    {
        get { return maxCharges < maxChargesCap; }
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não encontrado no herói! Adicione um componente AudioSource.");
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Não funciona com o jogo congelado (game over, e futuramente pausa ou escolha de nível)
        if (Time.timeScale == 0f) return;

        if (charges > 0 && Input.GetKeyDown(key))
        {
            UseSpecial();
        }
    }

    // Chamado ao subir de nível. Retorna true se ganhou uma carga,
    // e false se já estava com a capacidade cheia (nesse caso não acumula além do limite).
    public bool GrantCharge()
    {
        if (charges >= maxCharges) return false;

        charges++;
        UpdateUI();
        return true;
    }

    // Aumenta quantas cargas o hero pode guardar. Retorna true se aumentou (false se já está no teto).
    public bool AddMaxCharge()
    {
        if (!CanAddMaxCharge) return false;

        maxCharges++;
        UpdateUI();
        return true;
    }

    private void UseSpecial()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return; // sem inimigos no mapa, não gasta a habilidade

        // Toca o som uma única vez, não uma vez por bala
        if (audioSource != null && specialSound != null)
        {
            audioSource.PlayOneShot(specialSound, specialVolume);
        }

        foreach (GameObject enemy in enemies)
        {
            Vector2 direction = (enemy.transform.position - firePoint.position).normalized;

            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDirection(direction);
                bullet.SetPierce(specialPierce);
                bullet.SetDamage(specialDamage);
            }
        }

        charges--;
        UpdateUI();
    }

        private void UpdateUI()
    {
        if (chargesUI != null)
        {
            chargesUI.Refresh(charges, maxCharges);
        }

        if (specialText == null) return;

        if (charges > 0)
        {
            specialText.text = "[" + key + "] ataque especial PRONTO (" + charges + "/" + maxCharges + ")";
        }
        else
        {
            specialText.text = "Ataque especial: suba de nível (0/" + maxCharges + ")";
        }
    }
}
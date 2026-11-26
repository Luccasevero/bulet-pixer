using TMPro;
using UnityEngine;

// Habilidade especial do hero: ao apertar ESPAÇO, ele atira em TODOS os inimigos do mapa.
// Só existe uma carga por vez: subir de nível com a carga cheia não acumula.
public class PlayerSpecial : MonoBehaviour
{
    [Header("Habilidade especial")]
    [SerializeField] private KeyCode key = KeyCode.Space;
    [SerializeField] private GameObject bulletPrefab;      // o mesmo prefab do corte
    [SerializeField] private Transform firePoint;          // de onde os tiros saem
    [SerializeField] private int specialDamage = 2;        // dano de cada tiro da habilidade
    [SerializeField] private int specialPierce = 99;       // atravessa inimigos pra garantir que cada alvo seja atingido

    [Header("Interface (opcional)")]
    [SerializeField] private TMP_Text specialText;

    private bool hasCharge;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Não funciona com o jogo congelado (game over, e futuramente pausa ou escolha de nível)
        if (Time.timeScale == 0f) return;

        if (hasCharge && Input.GetKeyDown(key))
        {
            UseSpecial();
        }
    }

    // Chamado ao subir de nível. Retorna true se a habilidade ficou pronta agora,
    // e false se ela já estava pronta (nesse caso não acumula).
    public bool GrantCharge()
    {
        if (hasCharge) return false;

        hasCharge = true;
        UpdateUI();
        return true;
    }

    private void UseSpecial()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return; // sem inimigos no mapa, não gasta a habilidade

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

        hasCharge = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (specialText == null) return;

        specialText.text = hasCharge
            ? "ESPAÇO: ataque especial PRONTO!"
            : "Ataque especial: suba de nível";
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Fica no hero. Guarda a experiência, o nível e o alcance de coleta das gemas,
// e atualiza a barra e o texto na tela.
public class PlayerExperience : MonoBehaviour
{
    [Header("Quanta experiência precisa pra subir de nível")]
    [SerializeField] private int baseXpToLevel = 5;       // experiência necessária no nível 1
    [SerializeField] private int xpIncreasePerLevel = 3;  // quanto a exigência aumenta a cada nível

    [Header("Habilidade especial")]
    [SerializeField] private int levelsPerExtraCharge = 5; // a cada quantos níveis o hero pode guardar +1 carga (0 = desligado)

    [Header("Coleta de gemas")]
    [SerializeField] private float pickupRange = 3f;      // distância em que as gemas começam a voar até o hero
    [SerializeField] private float maxPickupRange = 8f;   // teto do alcance

    [Header("Interface")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;

    private int level = 1;
    private int currentXp;
    private int xpToNext;
    private PlayerSpecial special;

    public int Level
    {
        get { return level; }
    }

    // Lido pelas gemas (XPGem) pra saber a que distância devem ser atraídas
    public float PickupRange
    {
        get { return pickupRange; }
    }

    void Awake()
    {
        special = GetComponent<PlayerSpecial>();
        xpToNext = baseXpToLevel;
        UpdateUI();
    }

    public void AddXp(int amount)
    {
        currentXp += amount;

        // O while permite subir mais de um nível de uma vez, se a experiência for grande
        while (currentXp >= xpToNext)
        {
            currentXp -= xpToNext;
            level++;
            xpToNext = baseXpToLevel + (level - 1) * xpIncreasePerLevel;
            OnLevelUp();
        }

        UpdateUI();
    }

    // Chamado pelo item de aprimoramento
    public void AddPickupRange(float amount)
    {
        pickupRange = Mathf.Min(pickupRange + amount, maxPickupRange);
    }

    private void OnLevelUp()
    {
        string message = "Nível " + level + "!";

        if (special != null)
        {
            // A cada X níveis, o hero passa a poder guardar mais uma carga
            bool gainedCapacity = false;
            if (levelsPerExtraCharge > 0 && level % levelsPerExtraCharge == 0)
            {
                gainedCapacity = special.AddMaxCharge();
            }

            // Todo nível dá uma carga (até o limite da capacidade)
            bool gainedCharge = special.GrantCharge();

            if (gainedCapacity)
            {
                message += " Agora você guarda mais uma habilidade!";
            }
            else if (gainedCharge)
            {
                message += " Habilidade pronta (tecla E)";
            }
        }

        if (UpgradeMessage.Instance != null)
        {
            UpgradeMessage.Instance.Show(message);
        }
    }

    private void UpdateUI()
    {
        if (xpBar != null)
        {
            xpBar.maxValue = xpToNext;
            xpBar.value = currentXp;
        }

        if (levelText != null)
        {
            levelText.text = "Nível " + level;
        }
    }

    // Desenha o alcance de coleta na Scene view (círculo azul) quando o hero está selecionado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
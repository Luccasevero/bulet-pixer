using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Fica no hero. Guarda a experiência e o nível, e atualiza a barra e o texto na tela.
public class PlayerExperience : MonoBehaviour
{
    [Header("Quanta experiência precisa pra subir de nível")]
    [SerializeField] private int baseXpToLevel = 5;       // experiência necessária no nível 1
    [SerializeField] private int xpIncreasePerLevel = 3;  // quanto a exigência aumenta a cada nível

    [Header("Interface")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;

    private int level = 1;
    private int currentXp;
    private int xpToNext;

    public int Level
    {
        get { return level; }
    }

    void Awake()
    {
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

    private void OnLevelUp()
    {
        // Por enquanto só avisa na tela. Na próxima etapa, aqui abre a escolha de aprimoramento.
        if (UpgradeMessage.Instance != null)
        {
            UpgradeMessage.Instance.Show("Nível " + level + "!");
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
}
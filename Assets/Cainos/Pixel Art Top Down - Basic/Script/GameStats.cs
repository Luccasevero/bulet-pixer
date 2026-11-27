using TMPro;
using UnityEngine;

// Conta o tempo sobrevivido e os inimigos derrotados, e mostra no canto da tela.
public class GameStats : MonoBehaviour
{
    // Permite chamar GameStats.Instance.AddKill() de qualquer script
    public static GameStats Instance { get; private set; }

    [SerializeField] private TMP_Text hudText; // texto fixo no canto da tela

    private float elapsedTime;
    private int kills;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Com o jogo congelado no Game Over (timeScale = 0), o deltaTime é 0 e o tempo para sozinho
        elapsedTime += Time.deltaTime;
        UpdateHud();
    }

    public void AddKill()
    {
        kills++;
    }

    // Texto usado pela tela de Game Over
    public string GetSummary()
    {
        return "Tempo: " + FormatTime(elapsedTime) + "\nInimigos derrotados: " + kills;
    }

    private void UpdateHud()
    {
        if (hudText == null) return;

        hudText.text = FormatTime(elapsedTime) + "\nMortes: " + kills;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
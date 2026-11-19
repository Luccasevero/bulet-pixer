using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Fica num objeto sempre ativo da cena (não dentro do painel, que começa desligado).
public class GameOverScreen : MonoBehaviour
{
    // Permite que o PlayerHealth chame GameOverScreen.Instance.Show()
    public static GameOverScreen Instance { get; private set; }

    [SerializeField] private GameObject panel;     // o painel com "Game Over" e o botão
    [SerializeField] private TMP_Text resultText;  // texto com o resumo da partida (tempo e mortes)

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // garante que a partida começa rodando (importante depois de reiniciar)

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Show()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (resultText != null && GameStats.Instance != null)
        {
            resultText.text = GameStats.Instance.GetSummary();
        }

        Time.timeScale = 0f; // congela o jogo atrás da tela
    }

    // Ligado ao botão "Jogar de novo"
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
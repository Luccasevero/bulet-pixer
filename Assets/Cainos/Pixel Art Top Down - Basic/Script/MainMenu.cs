using UnityEngine;
using UnityEngine.SceneManagement;

// Fica num objeto da cena do menu. Os botões chamam Play() e Quit().
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "SC All Props"; // nome exato da cena do jogo

    void Start()
    {
        // Garante que o tempo roda normal (importante se o jogador voltar ao menu depois de um game over)
        Time.timeScale = 1f;
    }

    // Ligado ao botão "Jogar"
    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Ligado ao botão "Sair"
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        // No editor o Application.Quit não faz nada, então paramos o modo Play
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
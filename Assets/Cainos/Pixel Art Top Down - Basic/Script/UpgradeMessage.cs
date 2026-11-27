using System.Collections;
using TMPro;
using UnityEngine;

// Mostra uma mensagem curta na tela (ex: "Mais dano!") e some depois de alguns segundos.
// Fica no objeto de texto (TextMeshPro) dentro do Canvas.
public class UpgradeMessage : MonoBehaviour
{
    // Permite que qualquer script chame UpgradeMessage.Instance.Show("...")
    public static UpgradeMessage Instance { get; private set; }

    [SerializeField] private TMP_Text text;
    [SerializeField] private float showTime = 2f;

    private Coroutine current;

    void Awake()
    {
        Instance = this;

        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }

        text.text = ""; // começa vazio
    }

    public void Show(string message)
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        current = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        text.text = message;
        yield return new WaitForSeconds(showTime);
        text.text = "";
        current = null;
    }
}
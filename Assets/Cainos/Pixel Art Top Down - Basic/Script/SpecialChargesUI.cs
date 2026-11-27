using UnityEngine;
using UnityEngine.UI;

// Mostra bolinhas indicando quantas cargas da habilidade especial o hero tem.
public class SpecialChargesUI : MonoBehaviour
{
    [SerializeField] private Image[] dots;                       // as 3 bolinhas, da esquerda para a direita
    [SerializeField] private Color fullColor = new Color(1f, 0.85f, 0.1f, 1f);   // carga disponível
    [SerializeField] private Color emptyColor = new Color(0.3f, 0.3f, 0.3f, 0.8f); // espaço vazio
    [SerializeField] private bool hideLockedDots = true;         // esconde os espaços ainda não desbloqueados

    public void Refresh(int charges, int maxCharges)
    {
        Debug.Log("Refresh: " + charges + "/" + maxCharges + " em " + gameObject.name, this);
        for (int i = 0; i < dots.Length; i++)
{
    if (dots[i] != null)
        Debug.Log("Dot " + i + " -> " + dots[i].transform.root.name + "/" + dots[i].name + " cor: " + dots[i].color, dots[i]);
}
        for (int i = 0; i < dots.Length; i++)
        {
            if (dots[i] == null) continue;

            bool unlocked = i < maxCharges;
            dots[i].gameObject.SetActive(unlocked || !hideLockedDots);
            dots[i].color = (i < charges) ? fullColor : emptyColor;
        }
    }
}
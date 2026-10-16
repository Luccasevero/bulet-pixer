
using UnityEngine;

public class BalaJogador : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Inimigo"))
        {
            Destroy(colisao.gameObject);
            Destroy(gameObject);
        }
    }
}

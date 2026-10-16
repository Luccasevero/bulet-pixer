
using UnityEngine;

public class TiroJogador : MonoBehaviour
{
    public GameObject prefabTiro;
    public float velocidadeTiro = 8f;
    public float intervaloTiro = 0.3f;

    private float tempoProximoTiro;

    void Update()
    {
        Vector2 direcao = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            direcao = Vector2.up;

        else if (Input.GetKey(KeyCode.DownArrow))
            direcao = Vector2.down;

        else if (Input.GetKey(KeyCode.LeftArrow))
            direcao = Vector2.left;

        else if (Input.GetKey(KeyCode.RightArrow))
            direcao = Vector2.right;

        if (direcao != Vector2.zero &&
            Time.time >= tempoProximoTiro)
        {
            Atirar(direcao);
            tempoProximoTiro = Time.time + intervaloTiro;
        }
    }

    void Atirar(Vector2 direcao)
    {
        if (prefabTiro == null)
            return;

        GameObject tiro = Instantiate(
            prefabTiro,
            transform.position,
            Quaternion.identity
        );

        Rigidbody2D rb = tiro.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direcao * velocidadeTiro;
        }

        Destroy(tiro, 3f);
    }
}
//teste

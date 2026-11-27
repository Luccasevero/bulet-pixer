using UnityEngine;

public class coletado : MonoBehaviour
{
    [Header("Áudio")]
    [SerializeField] private AudioClip somColeta;
    [SerializeField] private float pitchMin = 0.92f;
    [SerializeField] private float pitchMax = 1.08f;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    private bool jaColetado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jaColetado || !other.CompareTag("Player")) return;

        jaColetado = true;

        if (somColeta != null && other.TryGetComponent<AudioSource>(out var audioSource))
        {
            float pitchAnterior = audioSource.pitch;
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.PlayOneShot(somColeta, volume);
            audioSource.pitch = pitchAnterior;
        }

        // Aqui você chama a lógica de somar moeda ou dar o item ao jogador
        Destroy(gameObject);
    }
}
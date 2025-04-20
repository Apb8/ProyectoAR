using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioClip[] canciones = new AudioClip[2];
    [SerializeField] private AudioSource audioSource;
    [SerializeField] [Range(0f, 1f)] private float volumen = 0.5f;
    
    private int indiceActual = -1;
    private bool cambiandoCancion = false;

    void Start()
    {
        // Validaciones iniciales
        if (canciones.Length != 2)
        {
            Debug.LogError("You need 2 audio clips assigned to the MusicManager.");
            return;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        // Reproducir primera canción aleatoria
        ReproducirSiguienteCancion();
    }

    void Update()
    {
        // Detectar fin de canción de forma eficiente
        if (!cambiandoCancion && !audioSource.isPlaying)
        {
            StartCoroutine(CambiarCancion());
        }
    }

    void ReproducirSiguienteCancion()
    {
        // Selección aleatoria inicial
        if (indiceActual == -1)
        {
            indiceActual = Random.Range(0, canciones.Length);
        }
        else // Alternar entre las dos canciones
        {
            indiceActual = (indiceActual + 1) % canciones.Length;
        }

        audioSource.clip = canciones[indiceActual];
        audioSource.volume = volumen;
        audioSource.Play();
    }

    IEnumerator CambiarCancion()
    {
        cambiandoCancion = true;
        
        // Esperar 0.5 segundos adicionales entre canciones
        yield return new WaitForSeconds(0.5f);
        
        ReproducirSiguienteCancion();
        cambiandoCancion = false;
    }

    // Método público para cambiar manualmente
    public void SaltarCancion()
    {
        if (!cambiandoCancion)
        {
            StartCoroutine(CambiarCancion());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.2f;
    [SerializeField] private GameObject destroyEffectPrefab; // Opcional, para un efecto visual al ser destruido

    [SerializeField] private int scoreValue = 100;

    private AudioSource deathSound;

    private void Start()
    {
        deathSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckForPlayerAttack(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForPlayerAttack(other.gameObject);
    }

    private void CheckForPlayerAttack(GameObject collidedObject)
    {
        // Comprueba si la colisi�n es con un objeto que tiene el tag "player_atack"
        if (collidedObject.CompareTag("player_atack"))
        {
            // Inicia la secuencia de destrucci�n
            StartCoroutine(DestroyEnemy());
        }
    }

    private IEnumerator DestroyEnemy()
    {
        // Primero desactivamos la f�sica y los colliders para evitar m�s interacciones
        DisablePhysicsComponents();

        // Reproducir sonido de muerte si existe
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Instanciar efecto visual de destrucci�n si se ha asignado
        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);

            // Destruir el efecto despu�s de 2 segundos (o el tiempo que consideres adecuado)
            Destroy(effect, 2f);
        }

        // Sumar puntuaci�n si existe un game manager (implementaci�n opcional)
        AddScore();

        // Esperar un peque�o delay antes de destruir el objeto
        yield return new WaitForSeconds(destroyDelay);

        // Destruir el enemigo
        Destroy(gameObject);
    }

    private void DisablePhysicsComponents()
    {
        // Desactivar todos los colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Desactivar rigidbody si existe
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Desactivar comportamientos del enemigo para evitar que siga actuando
        // (Asumiendo que puedes tener otros scripts controlando el enemigo)
        MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            // No desactivar este script
            if (behaviour != this)
            {
                behaviour.enabled = false;
            }
        }
    }

    private void AddScore()
    {
    }
}
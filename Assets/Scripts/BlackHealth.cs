using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHealth : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.2f;
    [SerializeField] private GameObject destroyEffectPrefab;

    [SerializeField] private int scoreValue = 100;

    private AudioSource deathSound;
    public Score_logic scorelogic;
    private void Start()
    {
        deathSound = GetComponent<AudioSource>();
        scorelogic = FindObjectOfType<Score_logic>();
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
        if (collidedObject.CompareTag("player_atack"))
        {

            StartCoroutine(DestroyEnemy());
        }
    }

    private IEnumerator DestroyEnemy()
    {
        DisablePhysicsComponents();

        if (deathSound != null)
        {
            deathSound.Play();
        }

        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);

            Destroy(effect, 2f);
        }

        AddScore();

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }

    private void DisablePhysicsComponents()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour != this)
            {
                behaviour.enabled = false;
            }
        }
    }

    private void AddScore()
    {
        scorelogic.score += 100;
        scorelogic.JackoLantern += 1;
    }
}
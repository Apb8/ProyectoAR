using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    // Prefab del cubo que quieres spawnar
    public GameObject EnemyPrefab;

    // C�mara AR (puede ser la c�mara principal)
    public Camera arCamera;

    // Radio de spawneo en metros
    public float spawnRadius = 3f;

    void Start()
    {
        // Si no se asigna la c�mara, usa la c�mara principal
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }

        // Inicia la corutina de spawneo
        StartCoroutine(SpawnCubes());
    }

    // Corutina para spawneo aleatorio de cubos
    IEnumerator SpawnCubes()
    {
        while (true) // Bucle infinito para spawns continuos
        {
            // Espera un tiempo aleatorio entre 1 y 5 segundos
            float randomWaitTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(randomWaitTime);

            // Genera una posici�n aleatoria dentro del radio
            Vector3 randomPosition = GenerateRandomPositionAroundCamera();

            // Spawna el cubo en la posici�n generada
            Instantiate(EnemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    // M�todo para generar una posici�n aleatoria alrededor de la c�mara
    Vector3 GenerateRandomPositionAroundCamera()
    {
        // Genera coordenadas aleatorias dentro de un c�rculo
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;

        // Obt�n la posici�n de la c�mara
        Vector3 cameraPosition = arCamera.transform.position;

        // Crea un vector 3D usando la posici�n de la c�mara y el punto aleatorio
        // Usa el plano horizontal (ignorando la altura vertical)
        Vector3 spawnPosition = new Vector3(
            cameraPosition.x + randomCirclePoint.x,
            cameraPosition.y,
            cameraPosition.z + randomCirclePoint.y
        );

        return spawnPosition;
    }
}
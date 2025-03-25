using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    // Prefab del cubo que quieres spawnar
    public GameObject EnemyPrefab;

    // Cámara AR (puede ser la cámara principal)
    public Camera arCamera;

    // Radio de spawneo en metros
    public float spawnRadius = 3f;

    void Start()
    {
        // Si no se asigna la cámara, usa la cámara principal
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

            // Genera una posición aleatoria dentro del radio
            Vector3 randomPosition = GenerateRandomPositionAroundCamera();

            // Spawna el cubo en la posición generada
            Instantiate(EnemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    // Método para generar una posición aleatoria alrededor de la cámara
    Vector3 GenerateRandomPositionAroundCamera()
    {
        // Genera coordenadas aleatorias dentro de un círculo
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;

        // Obtén la posición de la cámara
        Vector3 cameraPosition = arCamera.transform.position;

        // Crea un vector 3D usando la posición de la cámara y el punto aleatorio
        // Usa el plano horizontal (ignorando la altura vertical)
        Vector3 spawnPosition = new Vector3(
            cameraPosition.x + randomCirclePoint.x,
            cameraPosition.y,
            cameraPosition.z + randomCirclePoint.y
        );

        return spawnPosition;
    }
}
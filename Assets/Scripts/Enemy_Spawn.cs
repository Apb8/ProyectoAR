using UnityEngine;
using System.Collections;

public class SimpleEnemySpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    public GameObject enemyPrefab; // Prefab del enemigo/cubo
    public int maxEnemies = 5; // N�mero m�ximo de enemigos
    public float spawnRadius = 5f; // Radio de spawn esf�rico
    public float minSpawnTime = 3f; // Tiempo m�nimo entre spawns
    public float maxSpawnTime = 8f; // Tiempo m�ximo entre spawns
    public float enemyLifeTime = 5f; // Tiempo que durar� cada enemigo

    private Camera mainCamera;
    private int currentEnemyCount = 0;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            // Esperar tiempo aleatorio
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Spawnear si no se ha alcanzado el m�ximo
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        // Generar posici�n aleatoria en una esfera alrededor de la c�mara
        Vector3 randomSpherePoint = Random.insideUnitSphere * spawnRadius;
        Vector3 spawnPosition = mainCamera.transform.position + randomSpherePoint;

        // Instanciar enemigo
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;

        // Orientar hacia la c�mara
        enemy.transform.LookAt(mainCamera.transform);

        // Modificar rotaci�n para que solo rote en Y (evitar inclinaci�n)
        Vector3 currentRotation = enemy.transform.eulerAngles;
        enemy.transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);

        // Destruir despu�s de un tiempo
        StartCoroutine(DestroyEnemyAfterTime(enemy));
    }

    IEnumerator DestroyEnemyAfterTime(GameObject enemy)
    {
        // Esperar el tiempo de vida
        yield return new WaitForSeconds(enemyLifeTime);

        // Destruir y reducir contador
        if (enemy != null)
        {
            Destroy(enemy);
            currentEnemyCount--;
        }
    }
}
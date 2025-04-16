using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 5;
    public float spawnRadius = 5f;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 8f;
    public float enemyLifeTime = 5f;

    public float moveSpeed = 0.5f;
    public float moveRadius = 1f;

    // Agregar referencia al prefab del indicador para pasarlo a los enemigos
    public GameObject indicatorPrefab;

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

            // Spawnear si no se ha alcanzado el máximo
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomSpherePoint = Random.insideUnitSphere * spawnRadius;
        Vector3 spawnPosition = mainCamera.transform.position + randomSpherePoint;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;

        enemy.transform.LookAt(mainCamera.transform.position);

        EnemyBehavior behavior = enemy.AddComponent<EnemyBehavior>();
        behavior.target = mainCamera.transform;
        behavior.moveSpeed = moveSpeed;
        behavior.attackActiveTime = 0.5f;
        behavior.attackCooldownTime = 1.0f;

        // Añadir y configurar el indicador
        EnemyIndicator indicator = enemy.AddComponent<EnemyIndicator>();
        indicator.indicatorPrefab = this.indicatorPrefab;

        StartCoroutine(DestroyEnemyAfterTime(enemy));
    }

    IEnumerator DestroyEnemyAfterTime(GameObject enemy)
    {
        yield return new WaitForSeconds(enemyLifeTime);

        if (enemy != null)
        {
            Destroy(enemy);
            currentEnemyCount--;
        }
    }
}
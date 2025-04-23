using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 5;
    public float spawnRadius = 5f;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 8f;
    public float enemyLifeTime = 5f;

    public float moveSpeed = 0.5f;
    public float moveRadius = 1f;

    public GameObject indicatorPrefab;

    private Camera mainCamera;
    private int currentEnemyCount = 0;

    void Start()
    {
        mainCamera = Camera.main;
        Application.targetFrameRate = 60;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("No hay prefabs de enemigos asignados");
            return;
        }

        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {

            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        if (selectedPrefab == null)
        {
            Debug.LogWarning("El prefab seleccionado es nulo");
            return;
        }

        Vector3 randomSpherePoint = Random.insideUnitSphere * spawnRadius;
        Vector3 spawnPosition = mainCamera.transform.position + randomSpherePoint;

        GameObject enemy = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;

        enemy.transform.LookAt(mainCamera.transform.position);

        EnemyBehavior behavior = enemy.GetComponent<EnemyBehavior>();
        if (behavior == null)
        {
            behavior = enemy.AddComponent<EnemyBehavior>();
        }

        behavior.target = mainCamera.transform;
        behavior.moveSpeed = moveSpeed;
        behavior.attackActiveTime = 0.5f;
        behavior.attackCooldownTime = 1.0f;

        EnemyIndicator indicator = enemy.GetComponent<EnemyIndicator>();
        if (indicator == null)
        {
            indicator = enemy.AddComponent<EnemyIndicator>();
        }
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
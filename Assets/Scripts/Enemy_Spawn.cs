using UnityEngine;
using System.Collections;

public class SimpleEnemySpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    public GameObject enemyPrefab; 
    public int maxEnemies = 5; 
    public float spawnRadius = 5f; 
    public float minSpawnTime = 3f; 
    public float maxSpawnTime = 8f; 
    public float enemyLifeTime = 5f; 

    [Header("Movement Configuration")]
    public float moveSpeed = 0.5f;
    public float moveRadius = 1f; 

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

 class EnemyBehavior : MonoBehaviour
{
    [Header("Target and Movement")]
    public Transform target;
    public float moveSpeed = 0.5f;
    public float attackDistance = 1.5f; 

    [Header("Attack Cycle")]
    public float attackActiveTime = 0.5f; 
    public float attackCooldownTime = 1.0f; 

    private Collider attackCollider;
    private bool isNearTarget = false;
    private float attackTimer = 0f;
    private bool isAttackActive = false;

    void Start()
    {
        
        attackCollider = GetComponent<Collider>();

        
        if (attackCollider == null)
        {
            attackCollider = GetComponentInChildren<Collider>();
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void Update()
    {
        if (target == null || attackCollider == null) return;

       
        transform.LookAt(target);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackDistance)
        {

            if (!isNearTarget)
            {
                isNearTarget = true;
                attackTimer = 0f; 
            }

            HandleAttackCycle();
        }
        else
        {

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            if (isNearTarget)
            {
                isNearTarget = false;
                isAttackActive = false;
                attackCollider.enabled = false;
            }
        }
    }

    void HandleAttackCycle()
    {
  
        attackTimer += Time.deltaTime;

        if (isAttackActive)
        {

            if (attackTimer >= attackActiveTime)
            {
                
                attackCollider.enabled = false;
                isAttackActive = false;
                attackTimer = 0f;
            }
        }
        else
        {
            
            if (attackTimer >= attackCooldownTime)
            {
                attackCollider.enabled = true;
                isAttackActive = true;
                attackTimer = 0f;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform == target)
        {
            Debug.Log("¡Colisión con el jugador!");
        }
    }
}
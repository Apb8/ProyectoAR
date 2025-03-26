using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;

public class ARBlockSpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    public GameObject blockPrefab;
    public int maxBlocks = 5;
    public float spawnRadius = 5f;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 8f;
    public float spawnHeightOffset = 0.5f;

    private ARRaycastManager raycastManager;
    private ARSessionOrigin sessionOrigin;
    private Camera arCamera;
    private List<GameObject> spawnedBlocks = new List<GameObject>();

    void Awake()
    {
        // Buscar componentes de manera m�s exhaustiva
        raycastManager = FindObjectOfType<ARRaycastManager>();
        sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        arCamera = Camera.main;

        // Validaciones de componentes
        if (raycastManager == null)
            Debug.LogError("No se encontr� ARRaycastManager. Aseg�rate de tener uno en la escena.");

        if (sessionOrigin == null)
            Debug.LogError("No se encontr� ARSessionOrigin. Aseg�rate de tener uno en la escena.");

        if (arCamera == null)
            Debug.LogError("No se encontr� la c�mara principal.");

        if (blockPrefab == null)
            Debug.LogError("No has asignado un prefab de bloque.");
    }

    void Start()
    {
        // Iniciar la corutina solo si todos los componentes est�n presentes
        if (raycastManager != null && arCamera != null && blockPrefab != null)
        {
            StartCoroutine(SpawnBlocksRandomly());
        }
        else
        {
            Debug.LogError("No se puede iniciar el spawner debido a componentes faltantes.");
        }
    }

    IEnumerator SpawnBlocksRandomly()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            Debug.Log($"Esperando {waitTime} segundos para spawnear un bloque");

            yield return new WaitForSeconds(waitTime);

            // Verificar si a�n no hemos alcanzado el m�ximo de bloques
            if (spawnedBlocks.Count < maxBlocks)
            {
                SpawnBlock();
            }
            else
            {
                Debug.Log("M�ximo de bloques alcanzado");
            }
        }
    }

    void SpawnBlock()
    {
        Vector3 randomPosition = GetRandomPositionNearCamera();

        if (randomPosition != Vector3.zero)
        {
            Debug.Log($"Spawneando bloque en posici�n: {randomPosition}");

            // Instanciar el bloque usando el sessionOrigin para posicionamiento correcto
            GameObject newBlock = Instantiate(blockPrefab, randomPosition, Quaternion.identity, sessionOrigin.transform);
            spawnedBlocks.Add(newBlock);

            StartCoroutine(RemoveBlockAfterDelay(newBlock));
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posici�n v�lida para spawnear");
        }
    }

    Vector3 GetRandomPositionNearCamera()
    {
        if (arCamera == null) return Vector3.zero;

        // Generar una posici�n aleatoria dentro del radio
        Vector2 randomPointInCircle = Random.insideUnitCircle * spawnRadius;

        // Calcular la posici�n en el mundo
        Vector3 spawnPosition = arCamera.transform.position +
            new Vector3(randomPointInCircle.x, 0, randomPointInCircle.y);

        // Lista para almacenar resultados del raycast
        var hits = new List<ARRaycastHit>();

        // Realizar raycast para encontrar un punto en el plano
        if (raycastManager.Raycast(spawnPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Obtener el primer punto de impacto
            Vector3 hitPoint = hits[0].pose.position;

            // A�adir un peque�o offset en altura
            hitPoint.y += spawnHeightOffset;

            return hitPoint;
        }

        return Vector3.zero;
    }

    IEnumerator RemoveBlockAfterDelay(GameObject block, float delay = 30f)
    {
        yield return new WaitForSeconds(delay);

        if (block != null)
        {
            spawnedBlocks.Remove(block);
            Destroy(block);
        }
    }
}
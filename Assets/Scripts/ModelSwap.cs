using UnityEngine;
using UnityEngine.UI;

public class PrefabSwitcher : MonoBehaviour
{
    public GameObject[] prefabs;
    public Text displayText;
    public RectTransform imageToMove; 
    public Vector2[] imagePositions; 

    private GameObject currentInstance;
    private int currentIndex = 0;

    void Start()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("No hay prefabs asignados en el inspector!");
            return;
        }

        if (imagePositions == null || imagePositions.Length != prefabs.Length)
        {
            Debug.LogError("Las posiciones de imagen no coinciden con la cantidad de prefabs!");
            return;
        }

        SwitchPrefab(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchPrefab(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchPrefab(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchPrefab(2);
        }
    }

    public void SwitchPrefab(int index)
    {
        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogWarning("Índice de prefab no válido");
            return;
        }

        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        currentInstance = Instantiate(prefabs[index], transform.position, transform.rotation, transform);
        currentIndex = index;

        if (displayText != null)
        {
            displayText.text = "Prefab actual: " + prefabs[index].name;
        }

        if (imageToMove != null)
        {
            imageToMove.anchoredPosition = imagePositions[index];
        }
    }

    public void SwitchToLantern()
    {
        SwitchPrefab(0);
    }

    public void SwitchToFrost()
    {
        SwitchPrefab(1);
    }

    public void SwitchToBlack()
    {
        SwitchPrefab(2);
    }

    public void CycleNext()
    {
        int nextIndex = (currentIndex + 1) % prefabs.Length;
        SwitchPrefab(nextIndex);
    }
}
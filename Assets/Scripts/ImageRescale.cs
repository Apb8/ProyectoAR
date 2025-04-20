using UnityEngine;
using UnityEngine.UI;

public class CambioRapidoTamanoImagen : MonoBehaviour
{
    [Range(0.1f, 2f)]
    public float intervaloCambio = 0.5f; 

    [Range(1.01f, 1.5f)]
    public float escalaAumentada = 1.1f;

    [Tooltip("Velocidad de la animación (opcional)")]
    [Range(1f, 10f)]
    public float velocidadAnimacion = 5f;

    private RectTransform rectTransform;
    private Vector3 escalaNormal;
    private float temporizador = 0f;
    private bool estaAumentado = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            escalaNormal = rectTransform.localScale;
        }
        else
        {
            Debug.LogError("Este script requiere un componente RectTransform.");
        }
    }

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= intervaloCambio)
        {
            temporizador = 0f;
            CambiarTamano();
        }
    }

    void CambiarTamano()
    {
        if (rectTransform == null) return;

        if (estaAumentado)
        {
            rectTransform.localScale = escalaNormal;
        }
        else
        {
            rectTransform.localScale = escalaNormal * escalaAumentada;
        }

        estaAumentado = !estaAumentado;
    }

    void OnDisable()
    {
        if (rectTransform != null)
        {
            rectTransform.localScale = escalaNormal;
        }
    }
}
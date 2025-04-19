using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MapFadeController : MonoBehaviour
{
    [Header("Contenedor del Mapa")]
    [SerializeField] private GameObject mapContainer; // Padre con todos los elementos
    [SerializeField] private float fadeDuration = 0.5f;
    
    [Header("Configuración")]
    [SerializeField] private Button toggleButton;
    [SerializeField] private bool startHidden = true;

    private bool isFading;
    private bool isMapVisible;

    void Start()
    {
        SetInitialState();
        toggleButton.onClick.AddListener(ToggleMap);
    }

    void SetInitialState()
    {
        SetChildrenAlpha(startHidden ? 0 : 1);
        mapContainer.SetActive(!startHidden);
        isMapVisible = !startHidden;
    }

    void ToggleMap()
    {
        if(!isFading)
            StartCoroutine(FadeMap());
    }

    IEnumerator FadeMap()
    {
        isFading = true;
        float currentAlpha = isMapVisible ? 1 : 0;
        float targetAlpha = isMapVisible ? 0 : 1; // Invertir alpha basado en estado actual
        
        // Activar contenedor ANTES del fade-in
        if(targetAlpha > 0.1f) 
            mapContainer.SetActive(true);

        float elapsed = 0;
        while(elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed/fadeDuration);
            SetChildrenAlpha(newAlpha);
            yield return null;
        }

        // Actualizar estado DESPUÉS del fade
        isMapVisible = !isMapVisible; // <--- Corrección clave aquí
        mapContainer.SetActive(isMapVisible); // <--- Estado actualizado
        SetChildrenAlpha(targetAlpha);
        
        isFading = false;
    }

    void SetChildrenAlpha(float alpha)
    {
        foreach(Transform child in mapContainer.transform)
        {
            // Para imágenes
            if(child.TryGetComponent<Image>(out var image))
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            // Para textos
            if(child.TryGetComponent<TextMeshProUGUI>(out var text))
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

            // Para botones
            if(child.TryGetComponent<Button>(out var button))
                button.interactable = alpha > 0.9f;
        }
    }
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SmartMapController : MonoBehaviour
{
    [System.Serializable]
    public class TimeGroup
    {
        [Tooltip("Contenedor padre del grupo")]
        public GameObject groupContainer;
        
        [Tooltip("Hora de inicio (0-23)")]
        [Range(0, 23)] public int startHour = 8;
        
        [Tooltip("Hora de fin (0-23)")]
        [Range(0, 23)] public int endHour = 18;
    }

    [Header("Configuración del Mapa")]
    [SerializeField] private GameObject mapContainer;
    [SerializeField] private Button toggleButton;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool startHidden = true;

    [Header("Grupos Horarios")]
    [SerializeField] private List<TimeGroup> timeGroups = new List<TimeGroup>();
    [SerializeField] private float timeCheckInterval = 10f;

    [Header("Debug")]
    [SerializeField] private bool debugTime = false;
    [SerializeField] [Range(0, 23)] private int debugHour = 12;

    private bool isMapVisible;
    private bool isFading;
    private DateTime currentTime;

    void Start()
    {
        InitializeMap();
        toggleButton.onClick.AddListener(ToggleMap);
        StartCoroutine(TimeUpdateLoop());
    }

    void InitializeMap()
    {
        SetMapAlpha(startHidden ? 0 : 1);
        mapContainer.SetActive(!startHidden);
        isMapVisible = !startHidden;
    }

    IEnumerator TimeUpdateLoop()
    {
        while(true)
        {
            UpdateTime();
            UpdateGroupsVisibility();
            yield return new WaitForSeconds(timeCheckInterval);
        }
    }

    void UpdateTime()
    {
        currentTime = debugTime ? 
            new DateTime(2023, 1, 1, debugHour, 0, 0) : 
            DateTime.Now;
    }

    void ToggleMap()
    {
        if(!isFading)
            StartCoroutine(AnimateMap());
    }

    IEnumerator AnimateMap()
    {
        isFading = true;
        float targetAlpha = isMapVisible ? 0 : 1;
        float startAlpha = GetCurrentAlpha();

        if(targetAlpha > 0) 
            mapContainer.SetActive(true);

        float elapsed = 0;
        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetMapAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsed/fadeDuration));
            yield return null;
        }

        SetMapAlpha(targetAlpha);
        isMapVisible = !isMapVisible;
        mapContainer.SetActive(isMapVisible);
        isFading = false;
    }

    void UpdateGroupsVisibility()
    {
        foreach(var group in timeGroups)
        {
            if(group.groupContainer == null) continue;

            bool shouldShow = CheckTimeWindow(group.startHour, group.endHour);
            group.groupContainer.SetActive(shouldShow);
        }
    }

    bool CheckTimeWindow(int start, int end)
    {
        int currentHour = currentTime.Hour;
        return (start < end) ? 
            (currentHour >= start && currentHour < end) : 
            (currentHour >= start || currentHour < end);
    }

    float GetCurrentAlpha()
    {
        if(mapContainer.transform.childCount == 0) return 1f;
        return mapContainer.transform.GetChild(0).GetComponent<Image>().color.a;
    }

    void SetMapAlpha(float alpha)
    {
        foreach(Transform child in mapContainer.transform)
        {
            if(child.TryGetComponent<Image>(out Image img))
                img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

            if(child.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }
}
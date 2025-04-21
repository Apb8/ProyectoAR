using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkUI : MonoBehaviour
{
    public float blinkSpeed = 0.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 1f;
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(blinkSpeed);
            canvasGroup.alpha = 0f;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}

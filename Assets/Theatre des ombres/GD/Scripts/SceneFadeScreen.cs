using UnityEngine;

public class SceneFadeScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("Scene fade screen initialized - screen is BLACK");
        }
    }

    public void FadeOut(float duration, System.Action onComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(duration, onComplete));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration, System.Action onComplete)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup is null!");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("Scene fade screen faded out - screen is CLEAR");

        onComplete?.Invoke();
    }
}

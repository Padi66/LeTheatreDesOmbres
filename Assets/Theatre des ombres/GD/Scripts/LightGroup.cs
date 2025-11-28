using UnityEngine;

public class LightGroup : MonoBehaviour
{
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _intensity = 1f;

    public float Intensity
    {
        get => _intensity;
        set
        {
            _intensity = value;
            UpdateLightsIntensity();
        }
    }

    void Start()
    {
        UpdateLightsIntensity();
    }

    private void UpdateLightsIntensity()
    {
        foreach (Light light in _lights)
        {
            if (light != null)
            {
                light.intensity = _intensity;
            }
        }
    }

    public void SetIntensity(float intensity)
    {
        Intensity = intensity;
    }

    public void FadeTo(float targetIntensity, float duration)
    {
        StartCoroutine(FadeCoroutine(targetIntensity, duration));
    }

    private System.Collections.IEnumerator FadeCoroutine(float targetIntensity, float duration)
    {
        float startIntensity = _intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        Intensity = targetIntensity;
    }
}
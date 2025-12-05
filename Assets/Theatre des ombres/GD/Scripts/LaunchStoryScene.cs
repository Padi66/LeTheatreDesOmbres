using System.Collections;
using UnityEngine;

public class LaunchStoryScene : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private DialogueSequence _dialogueSequence;
    [SerializeField] private CurtainsMove _curtains;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private AudioSource _audioSource;
    
    [Header("Lights")]
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _lightFadeDuration = 2f;
    
    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(2f);

        _curtains.OpenCurtainsRight();
        _curtains.OpenCurtainsLeft();

        yield return new WaitForSeconds(2f);

        _dialogueSequence.StartDialogueBranch(12);
        _audioSource.Play();

        yield return new WaitForSeconds(2f);

        //StartCoroutine(FadeLightsOut(_lightFadeDuration));

        //yield return new WaitForSeconds(_lightFadeDuration);

        //_levelManager.LoadBackStage();
    }

    private IEnumerator FadeLightsOut(float duration)
    {
        if (_lights == null || _lights.Length == 0) yield break;

        float[] startIntensities = new float[_lights.Length];

        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                startIntensities[i] = _lights[i].intensity;
            }
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            for (int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i] != null)
                {
                    _lights[i].intensity = Mathf.Lerp(startIntensities[i], 0f, t);
                }
            }

            yield return null;
        }

        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                _lights[i].intensity = 0f;
                _lights[i].enabled = false;
            }
        }
    }

    private IEnumerator FadeLightsIn(float duration)
    {
        if (_lights == null || _lights.Length == 0) yield break;

        float[] targetIntensities = new float[_lights.Length];

        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                if (!_lights[i].enabled)
                {
                    _lights[i].enabled = true;
                    _lights[i].intensity = 0f;
                }
                
                targetIntensities[i] = 1f;
            }
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            for (int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i] != null)
                {
                    _lights[i].intensity = Mathf.Lerp(0f, targetIntensities[i], t);
                }
            }

            yield return null;
        }

        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                _lights[i].intensity = targetIntensities[i];
            }
        }
    }
}

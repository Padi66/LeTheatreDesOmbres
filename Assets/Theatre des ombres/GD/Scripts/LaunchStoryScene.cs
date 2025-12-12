using System.Collections;
using UnityEngine;

public class LaunchStoryScene : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private DialogueSequence _dialogueSequence;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private AnimationDelay _animationDelay;
    [SerializeField] private GameObject _projecteur;
    
    [Header("Lights")]
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _lightFadeDuration = 2f;
    
    [Header("Delay")]
    [SerializeField] private float _extraDelayAfterCompletion = 1f;
    
    void Start()
    {
        
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSecondsRealtime(5f);
        
        _animationDelay.ResumeAnimations();
        _projecteur.SetActive(true);
        StartCoroutine(FadeLightsOut(_lightFadeDuration));
        _dialogueSequence.StartDialogueBranch(12);
        _audioSource.Play();
        

        yield return StartCoroutine(WaitForDialoguesAndAudio());

        yield return new WaitForSeconds(_extraDelayAfterCompletion);
        _projecteur.SetActive(false);
        StartCoroutine(FadeLightsIn(_lightFadeDuration));
        
        _levelManager.LoadMainMenu();
    }

    private IEnumerator WaitForDialoguesAndAudio()
    {
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        
        Debug.Log("Dialogues et audio terminés !");
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
                
                targetIntensities[i] = 5f;
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
